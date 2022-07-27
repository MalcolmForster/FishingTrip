import mysql.connector
import pyodbc
import FSEngine
import FSConnections
import json
import sys

from datetime import datetime, timedelta
from datetime import date

# This file sets up database access, and then by using FishScraperEngine file to fetch the data required.
# This data is then passed as JSON to the required database for storage and retrieval.
# Open connection to the fishrData database and get all existing tables and create those which are missing

request = sys.argv[1] # Request is either from backup server or from user to get data
#^^^^^^^^^^^^^^ change this to a method provided by website^^^^^^^^^^^^^^^^^^^
def connData():
    fishrDB = mysql.connector.connect(
        **FSConnections.fishrDBMysqlLinux
    )    
    return fishrDB

def connMSSql():
    try:
        #Try connect from linux machine
        conn = pyodbc.connect(
            FSConnections.FishingTripSQLLinux            
            )
    except:
        #Else try from windows machine
        conn = pyodbc.connect(
            FSConnections.FishingTripSQLWindows
            )
    return conn

def tablesData(database):
    cursor = database.cursor()
    query = 'SHOW TABLES'
    cursor.execute(query)

    dbTables = []

    for Table in cursor:
        for str in Table:
            dbTables.append(str)
            
    cursor.close()
    return dbTables

mysqlDB = connData()
mssqlDB = connMSSql()
mysqlDBData = tablesData(mysqlDB)

# Second step is to find all favourited fishing spots from all users on from the fishr database
fishr = mysql.connector.connect(
    **FSConnections.fishrMysqlLinux
)

cursor = fishr.cursor()
query = 'SELECT favspots FROM users' #_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_Need to adjust this to get favourites from the new database
cursor.execute(query)
addToDb = []
currFavs = []

for (UserFavs) in cursor:
    for fav in UserFavs:
        if  fav is not None:
            favs = fav.split(',')
            for sep in favs:
                if (not currFavs.__contains__(sep) and sep != ''):
                    currFavs.append(sep)
                    if not mysqlDBData.__contains__(sep): #and not addToDb.__contains__(sep): #---may need to add this line back in
                        addToDb.append(sep)

# Can replace some of above code by using the 'CREATE TABLE IF NOT EXISTS statement' instead

cursor.close()
fishr.close()

cursor = mysqlDB.cursor()

if len(addToDb) > 0:
    for table in addToDb:      
        cursor.execute("""CREATE TABLE `%s` (`id` int(11) unsigned NOT NULL PRIMARY KEY AUTO_INCREMENT, `date_time` datetime NOT NULL, `dataSF` JSON, `dataT4F` JSON)"""% (table,))
        print("Created: "+ table)

cursor.close()

# Now use the FSEngine for each table to fill in the data required. This is imported above.
# Will pass each fishingspot to FSengine, find the current times information and saves into the correct column as a JSON.

def getWeekFromToday():
    today = date.today()
    day = today.strftime("%a")
    weekFromToday = [day]
    for i in range(6):
        today = today + timedelta(days = 1)
        weekFromToday.append(today.strftime("%a"))
    return weekFromToday

def checkJSON(j, site):
    if (j != 0 and json.dumps(j)):      
        Results = json.dumps(j)
    else:
        ermsg = str.format("Not found on {0}",site)
        Results = json.dumps({"Spot":ermsg})
        print(str.format("Information not found on site {0} or json not returned in correct format",site))
    return Results

if request == "backup":
    for spot in currFavs:
        dn = datetime.now()        
        hour = (dn.strftime('%I %p'))
        if(hour[0] == '0'):
            hour = hour[1:]
        day = (datetime.today().strftime('%A'))[0:3]
        dt_string = dn.strftime('%Y-%m-%d %H:%M:%S')

        FSResults = json.dumps(FSEngine.surfForecast("next",spot))
        T4FBulk = FSEngine.tides4fishing(spot)

        for data in T4FBulk[day]:
            if hour == data:
                T4FhourData=T4FBulk[day][data]
                break
        T4FResults = json.dumps(T4FhourData)

        if json.loads(FSResults) == False:
            FSResults=json.dumps({"Spot not found":"On SurfForecast"})
        
        if json.loads(T4FResults) == False:
            T4FResults=json.dumps({"Spot not found":"On Tides4Fishing"}) 

        cursor = mysqlDB.cursor()
        try:
            cursor.execute("INSERT INTO `%s` (`date_time`, `dataSF`, `dataT4F`) VALUES ('%s', '%s', '%s')"% (spot, dt_string, FSResults, T4FResults))
            print("Added '%s' weather information"% (spot,))
        except:
            print("Failed to add json information for '%s'"% (spot,))
        cursor.close()










        

elif request == "request":
    days = sys.argv
    spot = days[2]
    reqRetSF = []
    reqRetT4F =[]
    del days[0:3]
    fullWeekResults = False
    if (days[0] == "FTW"): #stands for fishing trip website :)
        fullWeekResults = True
        days = getWeekFromToday()       

    reqRetSF=""
    for day in days:        
        data_soup = FSEngine.surfForecast(day,spot)        
        FSResults = checkJSON(data_soup,"Surf-forecast")
        # if data_soup != 0:      
        #     FSResults = json.dumps(data_soup)
        # else:
        #     FSResults = "{\"Spot\":\"Not found on Surf-forecast\"}"
        #     break
        if (FSResults.find("Spot") == -1):
            reqRetSF = (reqRetSF+"\""+day+"\":"+FSResults)
        else:
            reqRetSF = (reqRetSF + FSResults)
            break
    #break #<-------------------------------------------------BREAK ON THIS LINE IS FOR TESTING ONLY, LIMITS TO ONE FISHING SPOT!------------------------------------------------
    if fullWeekResults == True:
        reqRetT4F = checkJSON(FSEngine.tides4fishing(spot),"Tides4Fishing")
        dt_string = datetime.now().strftime('%Y-%m-%d %H:%M:%S')
        # if(json.loads(reqRetSF)):
        #     print("SF works")
        # if(json.loads(reqRetT4F)):
        #     print("T4F works")
        sqlCursor = mssqlDB.cursor()
        sqlCursor.execute("""INSERT INTO spotForecastsSF (spot,date_time, dataSF) VALUES ('%s', '%s','%s')"""% (spot, dt_string, reqRetSF))
        sqlCursor.execute("""INSERT INTO spotForecastsT4F (spot,date_time, dataSF) VALUES ('%s', '%s','%s')"""% (spot, dt_string, reqRetT4F))        
        sqlCursor.close()
    else:
        for day in days:
            T4F_data_soup = (FSEngine.tides4fishing(spot))[day] 
            T4FResults = checkJSON(T4F_data_soup, "Tides4Fishing")
            if (T4FResults.find("Spot") == -1):
                reqRetT4F.append([day,T4FResults])
            else:
                reqRetT4F.append(T4FResults)
                break
        print(reqRetSF)
        print(reqRetT4F)












elif request == "update":
    weekFromToday = getWeekFromToday()
    reqRet = []
    
    cursor = mysqlDB.cursor()
    cursor.execute('TRUNCATE favForecastsSF')
    cursor.close()

    sqlCursor = mssqlDB.cursor()
    sqlCursor.execute('TRUNCATE TABLE favForecastsSF')
    sqlCursor.close()

    cursor = mysqlDB.cursor()
    cursor.execute('TRUNCATE favForecastsT4F')
    cursor.close()

    sqlCursor = mssqlDB.cursor()
    sqlCursor.execute('TRUNCATE TABLE favForecastsT4F')
    sqlCursor.close()

    for spot in currFavs:
        #_______________________Uploads SurfForecast records to database____________________________

        reqRetSql = "{'Spot':'Not Found'}"
        print("Looking for "+spot+ " on SurfForecast")
        data_soup = FSEngine.SF_browser(spot) 
        if data_soup != 0:
            print("Found "+spot+ " on SurfForecast")
            #weekFromToday = ["Thu"] #<---------- for quick testing, don't need to run through entire week
            reqRetMySql="{"
            for day in weekFromToday:   
                FSResults = json.dumps(FSEngine.SF_info(day, data_soup))
                if FSResults == 0:
                    print("No data found for this spot on site")
                    break
                # else:
                #     print(FSResults)
                if day != weekFromToday[0]:
                    reqRetMySql = reqRetMySql+","
                reqRetMySql = (reqRetMySql+"\""+day+"\":"+FSResults)
            reqRetMySql = reqRetMySql + "}"
        else:
            reqRetMySql = "{\"Spot\":\"Not found on Surf-forecast\"}"
            
        date = datetime.now()
        dt_string = date.strftime('%Y-%m-%d %H:%M:%S')
        cursor = mysqlDB.cursor()
        sqlCursor = mssqlDB.cursor()
        cursor.execute("""INSERT INTO favForecastsSF (`spot`,`date_time`, `dataSF`) VALUES ('%s', '%s','%s')"""% (spot,dt_string, reqRetMySql))
        sqlCursor.execute("""INSERT INTO favForecastsSF (spot,date_time, dataSF) VALUES ('%s', '%s','%s')"""% (spot,dt_string, reqRetMySql))
        cursor.close()
        sqlCursor.close()

        #_______________________Uploads Tides4Fishing records to database____________________________

        reqRetSql = "{'Spot':'Not Found'}"
        print("Looking for "+spot +" on Tides4Fishing")
        dicT4F = FSEngine.tides4fishing(spot)
        if dicT4F != 0:
            print("Found "+spot+" on Tides4Fishing")
            reqRetMySql = json.dumps(dicT4F)
            # for day in weekFromToday:   
            #     FSResults = json.dumps(dicT4F)
            #     if FSResults == 0:
            #         print("No data found for this spot on site")
            #         break
            #     reqRetMySql = FSResults
        else:
            reqRetMySql = "{\"Spot\":\"Not found on Tides4Fishing\"}"
        
        #print(reqRetMySql)
        date = datetime.now()
        dt_string = date.strftime('%Y-%m-%d %H:%M:%S')
        cursor = mysqlDB.cursor()
        sqlCursor = mssqlDB.cursor()
        cursor.execute("""INSERT INTO favForecastsT4F (`spot`,`date_time`, `dataSF`) VALUES ('%s', '%s','%s')"""% (spot,dt_string, reqRetMySql))
        sqlCursor.execute("""INSERT INTO favForecastsT4F (spot,date_time, dataSF) VALUES ('%s', '%s','%s')"""% (spot,dt_string, reqRetMySql))
        cursor.close()
        sqlCursor.close()
else:
    print ("Input arguments incorrect")    

mysqlDB.commit()  
mssqlDB.commit()          
mysqlDB.close()
mssqlDB.close()
