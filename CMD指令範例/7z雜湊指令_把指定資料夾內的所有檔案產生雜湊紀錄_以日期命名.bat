@echo off
setlocal enabledelayedexpansion
rem 你的電腦需要先安裝7zip
rem 設定變數
rem 自動抓取目前日期
set currentDate=%date:~0,4%-%date:~5,2%-%date:~8,2%
rem 目標資料夾,請自行設定
set TargetDirPath="D:\DBBackup\*"

rem 切換到當前資料夾
cd /d "%~dp0"

rem 使用7z雜湊
"C:\Program Files\7-Zip\7z.exe" h -scrcSHA256 %TargetDirPath% > hashes_%currentDate%.sha256

echo 雜湊檔案產生完成
