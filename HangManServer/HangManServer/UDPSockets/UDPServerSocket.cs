using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Windows.UI.Xaml;

namespace UDPSockets
{
    class UDPServerSocket
    {
        private string _listeningPortNumber;

        public delegate void MessageReceivedDelegate(HostName hostName, string message);
        public MessageReceivedDelegate MessageReceived;

        public UDPServerSocket(int listeningPortNumber)
        {
            _listeningPortNumber = listeningPortNumber.ToString();

            StartServer();
        }

        private async void StartServer()
        {
            try
            {
                DatagramSocket dataGramSocket = new DatagramSocket();

                dataGramSocket.MessageReceived += DataGramSocket_MessageReceived;

                Debug.WriteLine("Server is about to bind...");

                await dataGramSocket.BindServiceNameAsync(_listeningPortNumber);

                Debug.WriteLine($"Server is bound to {_listeningPortNumber}");
            }
            catch (Exception ex)
            {
                SocketErrorStatus webErrorStatus = SocketError.GetStatus(ex.GetBaseException().HResult);
                Debug.WriteLine(webErrorStatus.ToString() != "Unknown" ? webErrorStatus.ToString() : ex.Message);
            }
        }

        private void DataGramSocket_MessageReceived(DatagramSocket sender, DatagramSocketMessageReceivedEventArgs args)
        {
            string message;
            using (DataReader dataReader = args.GetDataReader())
            {
                message = dataReader.ReadString(dataReader.UnconsumedBufferLength).Trim();
            }

            MessageReceived(args.RemoteAddress, message);

            Debug.WriteLine($"Server received the message: \"{message}\"");
        }
    }
}
