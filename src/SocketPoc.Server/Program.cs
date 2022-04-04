using System;

namespace SocketPoc.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Starting socket server...");

            //ServerFileTCP.StartServer();

            ServerTCP.StartServer();
           

        }
    }
}
