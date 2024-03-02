# Ebenezer

Utility tools for live AV systems

> ☠️☠️ 
> **NOT PRODUCTION READY**
> 
> **Use at your own risk**

## Run server

```cmd
Ebenezer.exe --urls http://0.0.0.0:5000
```

http://10.185.192.20:5000/swagger/index.html

## Install as service

Install
```cmd
@echo OFF
echo Installing service...
sc create "Ebenezer" binPath= %~dp0\Ebenzer.exe start= auto
sc start "Ebenezer"
echo Installing service complete
pause
```

Uninstall
```cmd
sc delete "Ebenezer"
```
See https://stackoverflow.com/a/11084834/