@echo off
rem 設定變數
set currentDate=%date:~0,4%-%date:~5,2%-%date:~8,2%
set DirName=%currentDate%

rem 切換到當前資料夾
cd /d "%~dp0"

rem 建立資料夾
mkdir %currentDate%
rem pause
