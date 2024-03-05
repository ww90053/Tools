@echo off
rem 來源資料夾
set sourceSubfolder=TPPDB
set currentDate=%date:~0,4%-%date:~5,2%-%date:~8,2%
rem 新資料夾
set destinationSubfolder=更新檔_TPPDB_%currentDate%

rem 使用 %~dp0 獲取當前資料夾的路徑
set currentFolder=%~dp0

rem 組合源資料夾和目標資料夾的完整路徑
set sourcePath=%currentFolder%%sourceSubfolder%
set destinationPath=%currentFolder%%destinationSubfolder%

robocopy "%sourcePath%" "%destinationPath%" /S /MAXAGE:7
echo 完成。
pause