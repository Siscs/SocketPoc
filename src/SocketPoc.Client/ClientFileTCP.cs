using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketPoc.Client
{
    public class ClientFileTCP
    {
        private const int PORT_SERVER = 7123;
        private const string IP_SERVER = "127.0.0.1";

        public static string mensagemCliente = "em espera";

        public static void EnviarArquivo(string nomeArquivo)
        {
            try
            {
                IPEndPoint iPEndPointClient = new IPEndPoint(IPAddress.Parse(IP_SERVER), PORT_SERVER);
                Socket socketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                
                string caminhoArquivo = @"C:\dados\temp\";

                nomeArquivo = nomeArquivo.Replace("\\", "/");
                while (nomeArquivo.IndexOf("/") > -1)
                {
                    caminhoArquivo += nomeArquivo.Substring(0, nomeArquivo.IndexOf("/") + 1);
                    nomeArquivo = nomeArquivo.Substring(nomeArquivo.IndexOf("/") + 1);
                }

                byte[] nomeArquivoByte = Encoding.UTF8.GetBytes(nomeArquivo);

                if (nomeArquivoByte.Length > 5000 * 1024)
                {
                    mensagemCliente = "O tamanho do arquivo é maior que 5Mb, tente um arquivo menor.";
                    return;
                }

                string caminhoCompleto = caminhoArquivo + nomeArquivo;
                byte[] fileData = File.ReadAllBytes(caminhoCompleto);
                byte[] clientData = new byte[4 + nomeArquivoByte.Length + fileData.Length];
                byte[] nomeArquivoLen = BitConverter.GetBytes(nomeArquivoByte.Length);

                nomeArquivoLen.CopyTo(clientData, 0);
                nomeArquivoByte.CopyTo(clientData, 4);

                fileData.CopyTo(clientData, 4 + nomeArquivoByte.Length);

                socketClient.Connect(iPEndPointClient);

                if (!socketClient.Connected) { 
                    Console.WriteLine("Not connected");
                    socketClient.Close();
                    return;
                }

                socketClient.Send(clientData, 0, clientData.Length, 0);
                socketClient.Close();
                mensagemCliente = "Arquivo [" + caminhoCompleto + "] transferido.";
            }
            catch (Exception ex)
            {
                mensagemCliente = ex.Message + " " + "\nFalha, pois o Servidor não esta atendendo....";
            }
        }

    }
}
