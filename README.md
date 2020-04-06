# CSharpGame

This is a simple 'game' written in csharp. It is based off the official [asynchronous-server-socket-example](https://docs.microsoft.com/en-us/dotnet/framework/network-programming/asynchronous-server-socket-example) and [asynchronous-client-socket-example](https://docs.microsoft.com/en-us/dotnet/framework/network-programming/asynchronous-client-socket-example) from Microsoft.


## Usage
To run the server just run the executable.
```
.\CSharpGameServer.exe
```

Find the server IP address (printed to stdout on startup) and run the client.
```
.\CSharpGameClient.exe -i <game-server-ip> -n <player-name>
```
