serverIP = '192.168.86.24'

#-----This one is for linux

fishrDBMysqlLinux = {
    'host':serverIP,
    'user':'fishrDB',
    'password':'fishrDB123',
    'database':'fishrData'
    }

FishingTripSQLLinux = ('''DRIVER={ODBC Driver 18 for SQL Server};
                        SERVER=%s;
                        PORT=1433;
                        DATABASE=FishingTrip;
                        UID=FishingTrip;
                        PWD=FishingTrip123;
                        Encrypt=no;
                        TrustServerCertificate=yes''' % serverIP)

#-----This one is for windows pc

FishingTripSQLWindows = ('''DRIVER={SQL Server};
                        SERVER=%s;
                        PORT=1433;
                        DATABASE=FishingTrip;
                        UID=FishingTrip;
                        PWD=FishingTrip123''' % serverIP)


fishrMysqlLinux = {
    'host':serverIP,
    'user':'fishr',
    'password':'fishr123',
    'database':'fishr'''
    }
