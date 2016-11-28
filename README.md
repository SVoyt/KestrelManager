# KestrelManager
Application for managing (start/stop/deploy) kestrel apps.
It'is ultra-alpha-early version =))). 
Welcome to contributing. There is to much work: tests, security etc.

## Sample builds for Windows
[Server](https://github.com/SVoyt/svoyt.github.io/raw/master/kestrelmanager/0.0.1/samplebuilds/kestrelmanager.zip)
[Client](https://github.com/SVoyt/svoyt.github.io/raw/master/kestrelmanager/0.0.1/samplebuilds/kestrelmanager.client.zip)

## How to use
Setup server: 
Sample of appsettings.json for Windows app (.exe)
```javascript
    {
      "Name": "YourAppName",
      "Path": "C:\\App",
      "AutoStart": true,
      "RunCommand": "C:\\App\\App.exe"
    }
```
Sample of appsettings.json for Windows NetCore app (.dll)
```javascript
    {
      "Name": "YourAppName",
      "Path": "C:\\App",
      "AutoStart": true,
      "RunCommand": "dotnet C:\\App\\App.dll"
    }
```
Then you should run KestrelManager. 
As default it use 8000 port. 
* to see all apps http://localhost:8000/api/v1/app
* to start first app http://localhost:8000/api/v1/start/1
* to stop first app http://localhost:8000/api/v1/stop/1

## How to deploy
Run kestrel.manager.client with params.
For example: 
```
dotnet KestrelManager.Client.dll -h http://yourServerWithKestrelManager:8000 -a YourAppName -path C:\dataToDeploy\
```

