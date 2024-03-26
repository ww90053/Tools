rem 切換到當前資料夾 EX: D:\GIT\Tools\CMD指令範例\
cd /d "%~dp0"
rem 設定年月, EX: 2024/03
set currentDate=%date:~0,4%\%date:~5,2%\
rem 建立年月資料夾
mkdir %currentDate%
rem 匯出防火牆設定 EX: D:\GIT\Tools\CMD指令範例\2024\03\Firewall_Setting.wfw
netsh advfirewall export "%~dp0%currentDate%Firewall_Setting.wfw"
rem 匯出防火牆設定的reg檔案 主要是方便釐清修改內容,但匯入應該還是用wfw即可
reg export HKEY_LOCAL_MACHINE\SYSTEM\ControlSet001\Services\SharedAccess "%~dp0%currentDate%Firewall_Setting.reg" /y