import mechanicalsoup
from datetime import datetime

#------------------------------------The following code is for SURF-FORECAST website scraping ------------------------------------------------------------------------------------------------------

def surfForecast(day, location):

    browser = mechanicalsoup.StatefulBrowser() #<-------------------------Changed to stateful browser
    url = "http://www.surf-forecast.com"
    browser.open(url)    #also changed to open to suit stateful browser
    browser.select_form('.main-location-search')
    browser.get_current_form()
    browser["query"]=location
    browser.submit_selected()
    
    hourlyURL = browser.url.replace("/six_day","")
    browser.open(hourlyURL)
    data_soup = browser.get_current_page()    
    
    try:
        table = data_soup.select("#forecast-table > div > table > tbody > tr.forecast-table__row.forecast-table-days")[0]  
    except:
        return 0

    #first day in table can have 1 2 3 4  or 3 entries according to the time of day, need to count how many
    #Using is-day-end to find final third of the the first day
    #function to extract information for each day given in 'days' list

    def SF_info(day):
        #finding which columns have the correct days in them and importing into list
        z = 0
        column = []
        
        if day == "next":
            column.insert(len(column), 0)

            now = datetime.strftime(datetime.now(), "%I %p")
            if now.startswith('0'):
                now = now[1:]    
            
            # print(now)
            
            i = 2
            while i<=8:
                string = str(data_soup.select("#forecast-table > div > table > tbody > tr.forecast-table__row.forecast-table-time > td:nth-child(" + str(i) + ")"))
                string =  ((str(string).split(">")[2]).split("<")[0]).strip() + " " + ((str(string).split(">")[4]).split("<")[0]).strip()
                if string.find(now) != -1:
                    now_column = i 
                    break
                i+= 1
        else:
            for item in table:
                string = str(item)
                if day in string:
                    column.insert(len(column), z)   
                z += 1
                
        i = 2
        while i <= 8:
            string = str(data_soup.select("#forecast-table > div > table > tbody > tr.forecast-table__row.forecast-table-time > td:nth-child(" + str(i) + ")"))
            if string.find("is-day-end") != -1:
                #column_num_int = 2 
                fd = i - 2
                break
            i+= 1

        day_num = column[0]
         #<------ need to adjust suit the length of the first day (perhaps something like: 8 - number of columns in first day?

        if day == "next":
            c = 0
            column_num_int = now_column
        elif day_num == 1:
            c = fd
            column_num_int = 2                        
        else:
            column_num_int =8*(day_num-1)+(7-fd)
            c = 7
            
        slots = c + 1

        day_wave_height = [float] * slots
        day_wave_power = [int] * slots
        day_wave_dir = [str]* slots
        day_high_tide = [str] * slots
        day_low_tide = [str] * slots
        day_wind_speed = [int] * slots
        day_wind_dir = [str] * slots
        day_rain = [str] * slots
        day_temp = [int] * slots
        day_temp_chill = [int] * slots
        day_sunrise = [str] * slots
        day_sunset = [str] * slots
        
        
        #wave_soup = data_soup.select("#forecast-table > div > table > tbody > tr.forecast-table__row.forecast-table-time > td:nth-child(" + str(column_num_int) + ")")
        
        y = 0

        while y <= c:
            #wave heights
            wave_soup = data_soup.select("#forecast-table > div > table > tbody > tr:nth-child(5) > td:nth-child(" + str(column_num_int) + ") > div > svg > text")
            #forecast-table > div > table > tbody > tr:nth-child(5) > td:nth-child(2) > div > svg > text
            #forecast-table > div > table > tbody > tr:nth-child(5) > td:nth-child(4) > div > svg > text
            wave_height = float(((str(wave_soup)).split(">")[1]).split("<")[0])
            day_wave_height[y] = wave_height

            #wave power
            wave_soup = data_soup.select("#forecast-table > div > table > tbody > tr:nth-child(8) > td:nth-child(" + str(column_num_int) + ") > strong")
            
            #forecast-table > div > table > tbody > tr:nth-child(8) > td:nth-child(3) > strong
            wave_power = int(((str(wave_soup)).split(">")[1]).split("<")[0])
            day_wave_power[y] = wave_power
            
            #wave direction
            wave_soup = data_soup.select("#forecast-table > div > table > tbody > tr:nth-child(5) > td:nth-child(" + str(column_num_int) + ") > div > div")
            wave_dir = str(((str(wave_soup)).split(">")[1]).split("<")[0])
            day_wave_dir[y] = wave_dir

            #high tides
            wave_soup = data_soup.select("#forecast-table > div > table > tbody > tr:nth-child(11) > td:nth-child(" + str(column_num_int) + ") > div:nth-child(1)")
            high_tide = ((str(wave_soup).split(">")[1]).split("<")[0]).strip()
            day_high_tide[y] = high_tide

            #low tides
            wave_soup = data_soup.select("#forecast-table > div > table > tbody > tr:nth-child(12) > td:nth-child(" + str(column_num_int) + ") > div:nth-child(1)")
            low_tide = ((str(wave_soup).split(">")[1]).split("<")[0]).strip()
            day_low_tide[y] = low_tide
            
            #------------------------------------------------Basic weather related information------------------------------------------------------
            #_______________________________________________________________________________________________________________________________________
            
            # Wind speed
            wave_soup = data_soup.select("#forecast-table > div > table > tbody > tr:nth-child(9) > td:nth-child(" + str(column_num_int) + ") > div > svg > text")
            wind_speed = int((str(wave_soup).split(">")[1]).split("<")[0])
            day_wind_speed[y] = wind_speed          
            
            # Wind direction
            wave_soup = data_soup.select("#forecast-table > div > table > tbody > tr:nth-child(9) > td:nth-child(" + str(column_num_int) + ") > div > div")
            wind_dir = ((str(wave_soup).split(">")[1]).split("<")[0]).strip()
            day_wind_dir[y] = wind_dir
            
            # Rain in mm
            wave_soup = data_soup.select("#forecast-table > div > table > tfoot > tr.forecast-table__row.forecast-table-rain > td:nth-child(" + str(column_num_int) + ") > strong > span")
            rain = ((str(wave_soup).split(">")[1]).split("<")[0]).strip()            
            if rain != "-":
                rain = rain + " mm"                
            day_rain[y] = rain
            
            # Temp
            wave_soup = data_soup.select("#forecast-table > div > table > tfoot > tr:nth-child(6) > td:nth-child(" + str(column_num_int) + ") > span")
            temp = int((str(wave_soup).split(">")[1]).split("<")[0])                       
            day_temp[y] = temp
            
            # Chill factor
            wave_soup = data_soup.select("#forecast-table > div > table > tfoot > tr:nth-child(7) > td:nth-child(" + str(column_num_int) + ") > span")
            temp_chill = int((str(wave_soup).split(">")[1]).split("<")[0])                          
            day_temp_chill[y] = temp_chill
            
            # Sunrise
            wave_soup = data_soup.select("#forecast-table > div > table > tfoot > tr.forecast-table__row.forecast-table-sunrise > td:nth-child(" + str(column_num_int)+")")
            sunrise = ((str(wave_soup).split(">")[1]).split("<")[0]).strip()                           
            day_sunrise[y] = sunrise
            
            # Sunset
            wave_soup = data_soup.select("#forecast-table > div > table > tfoot > tr.forecast-table__row.forecast-table-sunset > td:nth-child(" + str(column_num_int) + ")")
            sunset = ((str(wave_soup).split(">")[1]).split("<")[0]).strip()                           
            day_sunset[y] = sunset
            
            y += 1
            column_num_int += 1
        
        # json creation here
        # First find out how many inputs there are
        
        times = ["0 AM", "3 AM", "6 AM", "9 AM", "12 PM", "3 PM", "6 PM", "9 PM"]
        timesLength = len(times)
        resultLength = len(day_wave_height)
        
        dif = timesLength - resultLength
        
        i=0
        
        if day == "next":
            times = ["Weather for " + now]
        else:        
            while i < dif:
                del times[0]
                i += 1
            
        i = 0        
        FSResults = dict()                
        while i < resultLength:
            FSResults[times[i]] = {"Wave Height":day_wave_height[i], "Wave Power":day_wave_power[i], "Wave Direction": day_wave_dir[i], "Low Tide":day_low_tide[i], "High Tide":day_high_tide[i], "Wind Speed":day_wind_speed[i], "Wind direction":day_wind_dir[i], "Rain":day_rain[i], "Temperature":day_temp[i], "Chill temp":day_temp_chill[i], "Sunrise":day_sunrise[i], "Sunset":day_sunset[i]}
            #FSResults[times[i]] = {"Wave Height":day_wave_height[i], "Wave Power":day_wave_power[i]} <--short results version for testing purposes
            i += 1
            
        return (FSResults)

    # Need to add the day to the start of the JSON
    dayResult = SF_info(day)
    return dayResult
        

#----------------------------------------END OF SURF-FORECAST website scraper--------------------------------------------------------------------------------

#---------------------------------------START OF METSERVICE website scraper---------------------------------------------
#---------INCOMPLETEINCOMPLETEINCOMPLETEINCOMPLETEINCOMPLETEINCOMPLETEINCOMPLETEINCOMPLETEINCOMPLETEINCOMPLETEINCOMPLETEINCOMPLETE

def metService(day, location):

    browser = mechanicalsoup.Browser()
    url = "http://www.metservice.com"
    page = browser.get(url)
    pagesoup = page.soup
    form = pagesoup.select("form")[0]
    form.select("input")[2]["value"] = location
    
    #document.querySelector("#root > div.App > div > div.SearchOverlay.is-open > div.SearchOverlay-bar > div > div > div > div > input")

    data_page = browser.submit(form,page.url)
    data_soup = data_page.soup
    
    def MS_info(day):        
        print("stuff to do added here")
    
    dayResult = MS_info(day)
    return dayResult

#---------------------------------------END OF METSERVICE website scraper---------------------------------------------


#---------------------------------------START OF TIDES4FISHING website scraper---------------------------------------------

# A major website for fishing information, will pull the following information off of it
# - Temperatures (water and air), swell, tides (side and time), pressure, moon, fishability





#---------------------------------------END OF TIDES4FISHING website scraper---------------------------------------------
