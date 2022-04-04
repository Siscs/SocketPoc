using System;

namespace SocketPoc.Client
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Starting socket client...");

            //ClientFileTCP.EnviarArquivo("teste.txt");
            ClientTCP.StartClient();
        }
    }
}
