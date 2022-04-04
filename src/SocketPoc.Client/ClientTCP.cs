using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketPoc.Client
{
    public static class ClientTCP
    {
        private const int PORT_SERVER = 7123;
        private const string IP_SERVER = "127.0.0.1";

        public static void StartClient()
        {
            try
            {

                // TcpClient tcpClient = new TcpClient();
                // tcpClient.Connect(IP_SERVER, PORT_SERVER);
                // tcpClient.BeginConnect(IP_SERVER, PORT_SERVER,)
                // tcpClient.ConnectAsync()

                int bytesSent;
                string mensagem = string.Empty;

                // get local ip address for client
                //IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
                //IPAddress ipServerAddress = ipHost.AddressList[0];

                IPAddress ipServerAddress = IPAddress.Parse(IP_SERVER);
                IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Parse(IP_SERVER), PORT_SERVER);
                Socket sender;

                // Creation TCP/IP Socket using
                // Socket Class Constructor

                while (true)
                {

                    try
                    {
                        Console.WriteLine("Digite a mensagem: ");
                        mensagem = Console.ReadLine();

                        if (string.IsNullOrEmpty(mensagem.Trim()))
                            continue;

                        if (mensagem.ToLower() == "exit")
                            break;

                        sender = new Socket(ipServerAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                        sender.Connect(localEndPoint);
                            
                        if (!sender.Connected)
                        {
                            Console.WriteLine("Erro ao efeturar conexão.");
                            sender.Close();
                            sender.Dispose();
                            return;
                        }

                        Console.WriteLine("Conectado em: {0} ", sender.RemoteEndPoint.ToString());

                        // add End of file
                        mensagem += "<EOF>";


                        // Creation of message that
                        // we will send to Server
                        byte[] messageBytes = System.Text.Encoding.ASCII.GetBytes(mensagem);
                        bytesSent = sender.Send(messageBytes);

                        // Data buffer
                        byte[] messageReceived = new byte[1024];

                        // We receive the message using
                        // the method Receive(). This
                        // method returns number of bytes
                        // received, that we'll use to
                        // convert them to string
                        int byteRecv = sender.Receive(messageReceived);
                        Console.WriteLine("Message from Server -> {0}",
                              Encoding.ASCII.GetString(messageReceived, 0, byteRecv));

                        if(sender.Connected)
                        {
                            // Close Socket using
                            // the method Close()
                            sender.Shutdown(SocketShutdown.Both);
                            sender.Disconnect(true);
                            sender.Close();
                        }
                        

                    }
                    catch (ArgumentNullException ane)
                    {
                        Console.WriteLine("=======> ArgumentNullException");
                        Console.WriteLine("Erro: {0}", ane.ToString());
                    }
                    catch (SocketException se)
                    {
                        Console.WriteLine("=======> SocketException");
                        Console.WriteLine("Erro: {0}", se.ToString());
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("=======> Exception");
                        Console.WriteLine($"Erro: {ex.Message.ToString()}");
                        return;
                    }
                }

                Console.WriteLine("Desconectando...");
                

            }
            catch (Exception ex)
            {
                Console.WriteLine("=======> Exception");
                Console.WriteLine($"Erro: {ex.Message.ToString()}");
                return;
            }

        }

        
        
}
}
