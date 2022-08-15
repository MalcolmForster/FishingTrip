import FSConnections
import pyodbc
import os
import subprocess
import sys

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

mssqlDB = connMSSql()
cursor = mssqlDB.cursor()
query = "SELECT * FROM forecastRequests"
cursor.execute(query)

result = cursor.fetchall()
if len(result) > 0:
    spot = result[0][1]
    jobFor = result[0][3]
    reqType = result[0][4]

    if reqType == 's':
        reqType = 0         #added this test to see if it will allow the select query in addRequest() method to work correctly, still no change
    elif reqType == 'f':
        reqType == 1

    if reqType == 0:
        fullReq = "request"
    elif reqType == 1:
        fullReq = "update"


    print("Found " + fullReq + " job for fishing spot \'"+spot+"\' for user "+jobFor)
    subprocess.call(['python3', '/var/www/scripts/FSServer.py',fullReq,spot,'FTW'])
    query = "DELETE FROM forecastRequests WHERE spot='%s' AND UserName='%s'" %(spot,jobFor)
    cursor = mssqlDB.cursor()
    cursor.execute(query)
else:
    print("No job to do")

mssqlDB.commit()
mssqlDB.close()