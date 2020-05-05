using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketIO_Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Player p = new Player("Some player name");
            //SocketStatus clientStatus = null;
            //SocketIO_Client client = new SocketIO_Client("localhost", 4404);
            SocketConnection socket = SocketIO_Client.createConnection("localhost", 4404);
            socket.Start();
            
            if(socket != null)
            {
                if(socket.Emit("player", p))
                {
                    Console.WriteLine("Player sended to the server");
                    socket.On("ok", (object o) =>
                    {
                        Console.WriteLine("Player accepted");
                    });

                    socket.On("disconnect", (object o) =>
                    {
                        Console.WriteLine("Socket desconectado");
                    });
                }
                else
                {
                    Console.WriteLine("Failed to send the player");
                }
            }
            else
            {
                Console.WriteLine("Failed to start the connection");
            }

            Console.ReadKey();
        }
    }
}
