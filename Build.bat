@echo off

rem Needed to update variable in loop
setlocal enabledelayedexpansion

for %%i in (.\bin\EmailPassword\*.dll) do (set files=!files!%%~i )

echo Merging EmailPassword

tools\ILMerge\ILMerge.exe /out:build\CHAOS.Portal.Authentication.EmailPasswordModule.dll /lib:C:\Windows\Microsoft.NET\Framework64\v4.0.30319 /targetplatform:v4,C:\Windows\Microsoft.NET\Framework64\v4.0.30319 /lib:lib\ %files%

echo Done