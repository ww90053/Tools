@echo off
rem �]�w�ܼ�
set currentDate=%date:~0,4%-%date:~5,2%-%date:~8,2%
set DirName=%currentDate%

rem �������e��Ƨ�
cd /d "%~dp0"

rem �إ߸�Ƨ�
mkdir %currentDate%
rem pause
