using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketPoc.Server
{
    public static class ServerTCP
    {
        private const int PORT_SERVER = 7123;
        private const string IP_SERVER = "127.0.0.1";

        public static void StartServer()
        {

            // get local ip address for client
            //IPHostEntry ipHost = Dns.GetHostEntry( Dns.GetHostName());
            //IPAddress ipServerAddress = ipHost.AddressList[0];

            IPAddress ipServerAddress = IPAddress.Parse(IP_SERVER);
            IPEndPoint serverIPEndPoint = new IPEndPoint(ipServerAddress, PORT_SERVER);
            Socket _socketServer;

            try
            {

                // Creation TCP/IP Socket using
                // Socket Class Constructor
                _socketServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);

                // Using Bind() method we associate a
                // network address to the Server Socket
                // All client that will connect to this
                // Server Socket must know this network
                // Address
                _socketServer.Bind(serverIPEndPoint);

                // Using Listen() method we create
                // the Client list that will want
                // to connect to Server
                _socketServer.Listen(10);

                Console.WriteLine($"Listening on port: {PORT_SERVER}");

                while (true)
                {

                    // Suspend while waiting for
                    // incoming connection Using
                    // Accept() method the server
                    // will accept connection of client
                    Socket clientSocket = _socketServer.Accept();

                    // Data buffer
                    byte[] bytes = new Byte[1024];
                    string messageReceived = null;

                    while (true)
                    {
                        int numByte = clientSocket.Receive(bytes);

                        messageReceived += Encoding.ASCII.GetString(bytes, 0, numByte);

                        if (string.IsNullOrEmpty(messageReceived) || messageReceived.IndexOf("<EOF>") > -1)
                            break;
                    }

                    Console.WriteLine("Mensagem recebida: {0} ", messageReceived);

                    byte[] message = Encoding.ASCII.GetBytes("Message Ok");

                    // Send a message to Client
                    // using Send() method
                    clientSocket.Send(message);

                    // Close client Socket using the
                    // Close() method. After closing,
                    // we can use the closed Socket
                    // for a new Client Connection
                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("==================");
                Console.WriteLine($"Ocorreu o Erro: {ex.Message.ToString()}");
                return;
            }
        }
    }
}
