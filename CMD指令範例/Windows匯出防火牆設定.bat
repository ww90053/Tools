rem �������e��Ƨ� EX: D:\GIT\Tools\CMD���O�d��\
cd /d "%~dp0"
rem �]�w�~��, EX: 2024/03
set currentDate=%date:~0,4%\%date:~5,2%\
rem �إߦ~���Ƨ�
mkdir %currentDate%
rem �ץX������]�w EX: D:\GIT\Tools\CMD���O�d��\2024\03\Firewall_Setting.wfw
netsh advfirewall export "%~dp0%currentDate%Firewall_Setting.wfw"
rem �ץX������]�w��reg�ɮ� �D�n�O��K��M�ק鷺�e,���פJ�����٬O��wfw�Y�i
reg export HKEY_LOCAL_MACHINE\SYSTEM\ControlSet001\Services\SharedAccess "%~dp0%currentDate%Firewall_Setting.reg" /y