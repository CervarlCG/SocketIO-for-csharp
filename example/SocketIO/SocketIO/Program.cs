using System;

namespace SocketIO
{
    class Program
    {
        static void Main(string[] args)
        {
            SocketIO s = new SocketIO("localhost", 4404);
            Game g = new Game();
            s.onConnection(g.onConnection);
            s.Start();
            Console.WriteLine("Server started");
            Console.ReadKey();
        }

        private void onConnection(SocketConnection socket)
        {
            Console.WriteLine("Conexion recibida.");
        }
    }
}
