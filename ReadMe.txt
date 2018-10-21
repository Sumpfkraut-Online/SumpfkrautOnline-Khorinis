Everything you need will be compiled & copied into the "bin" folder.

Server files can be found in "bin/Server/*".
Client files can be found in "bin/localhost-9054/*".
The Launcher can be found in "bin/*".
The Filepacker can be found in "bin/FilePacker/*".

How to start and connect to a local server:
1. Start "bin/Server/GUC_Server.exe" to start the server.
2. Start "bin/GUCLauncher.exe" to start the launcher.
(3. On the first start, search and select your Gothic 2 folder in the opened folder browser.)
4. Add the server "localhost:9054" to the server list.
5. Choose the localhost-server from the server list and press "Connect".
6. Press "Start" to start the GUC-Gothic-Client.


DBPatterns - example Sqlite database patterns to be used in the server scripts
FilePacker - To compile data for the launcher to load
Gothic - Objects and information directly related to the Gothic-process
GUCClient - Basic GUC client process
GUCLauncher - Program to search and store servers and join with the GUC client
GUCServer - Basic GUC server process
GUCShared - Basic GUC functionality shared between client and server
ScriptsClient - Server-specific scripts loaded by GUCClient
ScriptsServer - Scripts that define the actual game server of basis of GUCServer
ScriptsShared - Shared functionality between client and server scripts
WinApi - Process injection, hooks and manipulation (often, falsely identified as malware)
WebInterface - A basic web-interface to i.e. send control commands to runnign server scripts