@echo off
setlocal enabledelayedexpansion
rem �A���q���ݭn���w��7zip
rem �]�w�ܼ�
rem �۰ʧ���ثe���
set currentDate=%date:~0,4%-%date:~5,2%-%date:~8,2%
rem �ؼи�Ƨ�,�Цۦ�]�w
set TargetDirPath="D:\DBBackup\*"

rem �������e��Ƨ�
cd /d "%~dp0"

rem �ϥ�7z����
"C:\Program Files\7-Zip\7z.exe" h -scrcSHA256 %TargetDirPath% > hashes_%currentDate%.sha256

echo �����ɮײ��ͧ���
