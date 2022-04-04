using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketPoc.Server
{
    public static class ServerFileTCP
    {
        private const int PORT_SERVER = 7123;
        private const string IP_SERVER = "127.0.0.1";

        static IPEndPoint _iPEndPoint;
        static Socket _socketServer;

        public static string caminhoRecepcaoArquivos = @"C:\TEMP\";
        public static string mensagemServidor = "Serviço encerrado !!";

        public static void StartServer()
        {
            try
            {
                _iPEndPoint = new IPEndPoint(IPAddress.Parse(IP_SERVER), PORT_SERVER);

                _socketServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                _socketServer.Bind(_iPEndPoint);

                Console.WriteLine($"Listening on port: {PORT_SERVER}");

            }
            catch (Exception ex)
            {
                Console.WriteLine("==================");
                Console.WriteLine($"Ocorreu o Erro: {ex.Message.ToString()}");
                return;
            }


            try
            {
                _socketServer.Listen(100);

                Socket clienteSock = _socketServer.Accept();

                clienteSock.ReceiveBufferSize = 16384;

                byte[] dadosCliente = new byte[1024 * 50000];

                int tamanhoBytesRecebidos = clienteSock.Receive(dadosCliente, dadosCliente.Length, 0);
                int tamnhoNomeArquivo = BitConverter.ToInt32(dadosCliente, 0);
                string nomeArquivo = Encoding.UTF8.GetString(dadosCliente, 4, tamnhoNomeArquivo);

                BinaryWriter bWrite = new BinaryWriter(File.Open(caminhoRecepcaoArquivos + nomeArquivo, FileMode.Append));
                bWrite.Write(dadosCliente, 4 + tamnhoNomeArquivo, tamanhoBytesRecebidos - 4 - tamnhoNomeArquivo);

                while (tamanhoBytesRecebidos > 0)
                {
                    tamanhoBytesRecebidos = clienteSock.Receive(dadosCliente, dadosCliente.Length, 0);
                    if (tamanhoBytesRecebidos == 0)
                    {
                        bWrite.Close();
                    }
                    else
                    {
                        bWrite.Write(dadosCliente, 0, tamanhoBytesRecebidos);
                    }
                }
                bWrite.Close();

                clienteSock.Close();

                mensagemServidor = "Arquivo recebido e arquivado [" + nomeArquivo + "] (" + (tamanhoBytesRecebidos - 4 - tamnhoNomeArquivo) +
                        " bytes recebido); Servidor Parado";

                Console.WriteLine($"{mensagemServidor}");
            }
            catch (SocketException ex)
            {
                Console.WriteLine($"{ex.Message} - Erro ao receber arquivo.");
            }

            

        }

    }
}
