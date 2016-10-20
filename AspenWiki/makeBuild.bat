rmdir /S /Q Release
mkdir Release

xcopy bin Release\bin\ /E
xcopy Content Release\Content\ /E
xcopy Scripts Release\Scripts\ /E
xcopy Views Release\Views\ /E

xcopy favicon.ico Release\
xcopy Global.asax Release\
xcopy Web.config Release\

rem copy Web.Release.config Release\Web.config

rem robocopy Release \\shfmbfile.dev.aspentech.com\C$\inetpub\wwwroot /E

pause

