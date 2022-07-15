import mysql.connector
import FSEngine
import json
import sys

from datetime import datetime

# This file sets up database access, and then by using FishScraperEngine file to fetch the data required.
# This data is then passed as JSON to the required database for storage and retrieval.
# Open connection to the fishrData database and get all existing tables and create those which are missing

request = sys.argv[1] # Request is either from backup server or from user to get data
#^^^^^^^^^^^^^^ change this to a method provided by website^^^^^^^^^^^^^^^^^^^

def connData():    

    fishrDB = mysql.connector.connect(
        host='192.168.86.24',
        user='fishrDB',
        password='fishrDB123',
        database='fishrData'
    )    
    return fishrDB

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

currentDB = connData()

currentDBData = tablesData(currentDB)

# Second step is to find all favourited fishing spots from all users on from the fishr database
fishr = mysql.connector.connect(
    host='192.168.86.24',
    user='fishr',
    password='fishr123',
    database='fishr'
)

cursor = fishr.cursor()
query = 'SELECT favspots FROM users'
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
                    if not currentDBData.__contains__(sep): #and not addToDb.__contains__(sep): #---may need to add this line back in
                        addToDb.append(sep)

# Can replace some of above code by using the 'CREATE TABLE IF NOT EXISTS statement' instead

cursor.close()
fishr.close()

cursor = currentDB.cursor()

if len(addToDb) > 0:
    for table in addToDb:      
        cursor.execute("""CREATE TABLE `%s` (`id` int(11) unsigned NOT NULL PRIMARY KEY AUTO_INCREMENT, `date_time` datetime NOT NULL, `dataSF` JSON NOT NULL)"""% (table,))
        print("Created: "+ table)
    
cursor.close()

# Now use the FSEngine for each table to fill in the data required. This is imported above.
# Will pass each fishingspot to FSengine, find the current times information and saves into the correct column as a JSON.

# fishingSpots = tablesData(currentDB)

# for fishingSpot in fishingSpots:
#     FSResults = FSEngine.surfForecast(["Thu"], fishingSpot)
#     print (FSResults)
    
# days = ["Fri","Sat","Sun","Mon","Tue","Wed","Thu"]
#days = ["Fri","Sat","Sun"]
#days = ["Fri"]

if request == "backup":
    for spot in currFavs:
        FSResults = json.dumps(FSEngine.surfForecast("next",spot))
        if json.loads(FSResults):
            cursor = currentDB.cursor()
            date = datetime.now() 
            dt_string = date.strftime('%Y-%m-%d %H:%M:%S')
            #print("INSERT INTO `%s` (`date_time`, `dataSF`) VALUES ('%s', '%s')"% (spot, dt_string, FSResults))
            try:
                cursor.execute("INSERT INTO `%s` (`date_time`, `dataSF`) VALUES ('%s', '%s')"% (spot, dt_string, FSResults))
                print("Added '%s' weather information"% (spot,))
            except:
                print("Failed to add json information for '%s'"% (spot,))
            cursor.close()
        else:
            print ("Not in json format for '%s'"% (spot,))
elif request == "request":
    days = sys.argv
    spot = days[2]
    reqRet = []
    del days[0:3]   
    # for spot in currFavs:
        #print(spot)
    for day in days:          
        FSResults = json.dumps(FSEngine.surfForecast(day,spot))
        if FSResults == 0:
            print("No data found for this spot on site")
            break
        reqRet.append([day,FSResults])
    #break #<-------------------------------------------------BREAK ON THIS LINE IS FOR TESTING ONLY, LIMITS TO ONE FISHING SPOT!------------------------------------------------
    print(reqRet)
else:
    print ("Input arguments incorrect")
    

currentDB.commit()            
currentDB.close()


