

call 1.Environment.bat

@echo ZipOutputPath : "%ZipOutputPath%"
@echo SourcePath : "%SourcePath%"

@echo 1. Make zip

@"7zip\7za.exe" d "%ZipOutputPath%\Client.Shared.zip"
@"7zip\7za.exe" a "%ZipOutputPath%\Client.Shared.zip" "%SourcePath%\Client.Shared\*.cs"

@echo @"7zip\7za.exe" a "%ZipOutputPath%\GameClient.Shared.zip" "%SourcePath%\GameClient.Shared\**\**\*.cs"
@echo @"7zip\7za.exe" a "%ZipOutputPath%\GameClient.Shared.zip" "%SourcePath%\GameClient.Shared\**\*.cs"
#echo @"7zip\7za.exe" a "%ZipOutputPath%\GameClient.Shared.zip" "%SourcePath%\CMCode\*.cs"



pause