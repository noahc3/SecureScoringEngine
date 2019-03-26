mkdir rawbin
mkdir bin\ubuntu-16.04\

dotnet build -o ./rawbin -r Debug SSEFrontend.csproj

CALL mkbundle rawbin/SSEFrontend.exe --simple -o bin/ubuntu-16.04/run-script-x86 --cross mono-5.16.0-ubuntu-16.04-x86
CALL mkbundle rawbin/SSEFrontend.exe --simple -o bin/ubuntu-16.04/run-script-x64 --cross mono-5.16.0-ubuntu-16.04-x64