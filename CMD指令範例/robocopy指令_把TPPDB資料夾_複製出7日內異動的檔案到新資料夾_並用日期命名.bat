@echo off
rem �ӷ���Ƨ�
set sourceSubfolder=TPPDB
set currentDate=%date:~0,4%-%date:~5,2%-%date:~8,2%
rem �s��Ƨ�
set destinationSubfolder=��s��_TPPDB_%currentDate%

rem �ϥ� %~dp0 �����e��Ƨ������|
set currentFolder=%~dp0

rem �զX����Ƨ��M�ؼи�Ƨ���������|
set sourcePath=%currentFolder%%sourceSubfolder%
set destinationPath=%currentFolder%%destinationSubfolder%

robocopy "%sourcePath%" "%destinationPath%" /S /MAXAGE:7
echo �����C
pause