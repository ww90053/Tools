@echo off
setlocal enabledelayedexpansion

rem �]�w�ܼ�
set currentDate=%date:~0,4%-%date:~5,2%-%date:~8,2%
set zipFileName=BACKUP_%currentDate%.zip

rem �������e��Ƨ�
cd /d "%~dp0"

rem �ϥ�7z���Y�Ҧ����
"C:\Program Files\7-Zip\7z.exe" a -tzip !zipFileName! *

echo ���Y�����C
pause
