@echo off
setlocal enabledelayedexpansion

rem �]�w�ܼ�
set folderName="TPPDB"
set currentDate=%date:~0,4%-%date:~5,2%-%date:~8,2%
set zipFileName=(�w�g�[�K)��s��_TPPDB_%currentDate%.zip
set password="80725454"

rem �������e��Ƨ�
cd /d "%~dp0"

rem �ϥ�7z���Y�Ҧ����
"C:\Program Files\7-Zip\7z.exe" a -tzip -p!password! !zipFileName! %folderName%

echo ���Y�����C
pause
