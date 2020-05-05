# A basic Socket Manager based on events and multithreading

Supported features:

1. Register events
2. Emit events

# Usage

## server
The server namespace is in SocketIO, to start a only create a instance of the SocketIO class and specified the host and port, 

To start the server then call to the function start

```c#
socketConnection.Start();
```

But before to start the server you can to call the onConnection method and set the callback to be called when a socket is connected, that method will receive a object of type SocketManager.

```c#
 socketManager.onConnection(someMethod);

 public void someMethod(object socketManager)
 {
     ...
 }
```
To register a event then call the method "On" of the SocketManager.

```c#
socketManager.On("someEvent", someMethod);

public void someMethod(object o)
{
    ...
}
```

To emit a event then call the Emit method
```c#
if(socketManager.Emit("someEvent", objectToSend))
{
    //Success
}
else
{
    //Error
}
```

When the conexion is lossed then the server try to call the event "disconnect", so if you want catch a disconnected event the register in with the "On" method

## Client
To execute a client then use the namespace SocketIO_Client, to create a SocketManager instance then call the static method createConnection of the SocketIO_Client class.

```c#
SocketManager socket = SocketIO_Client.createConnection(host, port);
socket.Start();
```

The emit and On are same that the server.

When the conexion is lossed then the client try to call the event "disconnect", so if you want catch a disconnected event the register in with the "On" method