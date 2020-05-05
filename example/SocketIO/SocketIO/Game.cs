using System;

namespace SocketIO
{
    class Game
    {
        SocketConnection socket;
        public void onConnection(object s)
        {
            socket = (SocketConnection)s;
            Console.WriteLine("Conexion recibida");
            socket.On("player", onPlayer);
            socket.On("disconnect", (object o) =>
            {
                Console.Write("Socket disconnected");
            });
        }

        public void onPlayer(object o)
        {
            if (o is Player)
            {
                Console.WriteLine("Player connected: " + ((Player)o).name);
                socket.Emit("ok", new object());
                
            }
            else
                Console.WriteLine("Unknown Player");
        }
    }
}
