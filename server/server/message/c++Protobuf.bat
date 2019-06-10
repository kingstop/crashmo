@echo off
setlocal enabledelayedexpansion
set tool=.\
set common_path=.\msg\
set pbfile_path=.\
set csharp_path=..\..\..\client\pushmole\Assets\mole_net\message\
for /f %%i in ('dir /a-d /b *.proto') do ( 
echo %%i
%tool%protoc.exe --proto_path=%pbfile_path%  --cpp_out=%common_path%  %%i
)
pause
