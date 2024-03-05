@echo off
setlocal enabledelayedexpansion

rem 設定變數
set folderName="TPPDB"
set currentDate=%date:~0,4%-%date:~5,2%-%date:~8,2%
set zipFileName=(已經加密)更新檔_TPPDB_%currentDate%.zip
set password="80725454"

rem 切換到當前資料夾
cd /d "%~dp0"

rem 使用7z壓縮所有文件
"C:\Program Files\7-Zip\7z.exe" a -tzip -p!password! !zipFileName! %folderName%

echo 壓縮完成。
pause
