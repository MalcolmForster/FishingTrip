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
    if reqType == 0:
        fullReq = "request"
    elif reqType == 1:
        fullReq = "update"
    print("Found " + reqType + " job for fishing spot \'"+spot+"\' for user "+jobFor)
    subprocess.call(['python3', 'FSServer.py',fullReq,spot,'FTW'])
    query = "DELETE FROM forecastRequests WHERE spot='%s' AND UserName='%s'" %(spot,jobFor)
    cursor = mssqlDB.cursor()
    cursor.execute(query)
else:
    print("No job to do")

mssqlDB.commit()
mssqlDB.close()