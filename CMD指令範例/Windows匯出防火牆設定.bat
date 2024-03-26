rem 切換到當前資料夾 EX: D:\GIT\Tools\CMD指令範例\
cd /d "%~dp0"
rem 設定年月, EX: 2024/03
set currentDate=%date:~0,4%\%date:~5,2%\
rem 建立年月資料夾
mkdir %currentDate%
rem 匯出防火牆設定 EX: D:\GIT\Tools\CMD指令範例\2024\03\Firewall_Setting.wfw
netsh advfirewall export "%~dp0%currentDate%Firewall_Setting.wfw"
