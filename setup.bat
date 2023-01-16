@echo off

set /p spt_path="Input SPTarkov absolute filepath: "

if not exist %spt_path% (
	echo Invalid filepath!
	exit /b 1
)

echo copy "%%1" "%spt_path%\BepInEx\plugins\" > LegDay\copy_build.bat
echo Generated "LegDay\copy_build.bat" with target "%spt_path%\BepInEx\plugins\"