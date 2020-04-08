# CSharpGame

This is a simple 'game' written in csharp. It is based off the official [asynchronous-server-socket-example](https://docs.microsoft.com/en-us/dotnet/framework/network-programming/asynchronous-server-socket-example) and [asynchronous-client-socket-example](https://docs.microsoft.com/en-us/dotnet/framework/network-programming/asynchronous-client-socket-example) from Microsoft.

## Build

### Windows
Use Visual Studio to build the executable in your desired config.

### Linux
Use the following command to build a release version of the game for Linux.
```
dotnet publish -c release -r linux-x64 --self-contained
```

## Usage

To run the server just run the executable.
```
# Windows
.\CSharpGameServer.exe --port=<game_server_port>

# Linux
.\CSharpGameServer --port=<game_server_port>
```

Find the server IP address (printed to stdout on startup) and run the client.
```
# Windows
.\CSharpGameClient.exe -i <game_server_ip> -p <game_server_port> -n <player_name>

# Linux
.\CSharpGameClient -i <game_server_ip> -p <game_server_port> -n <player_name>
```
