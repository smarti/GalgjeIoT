using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Windows.Networking;
using UDPSockets;

namespace HangManServer
{
    internal class Server
    {
        private readonly int _listeningPortNumber;

        private readonly UDPServerSocket _serverSocket;

        private readonly List<UDPClientSocket> _clients;
        public MessageReceivedDelegate MessageReceived;

        public Server(int listeningPortNumber)
        {
            _listeningPortNumber = listeningPortNumber;

            _clients = new List<UDPClientSocket>();

            _serverSocket = new UDPServerSocket(_listeningPortNumber);
            _serverSocket.MessageReceived += ServerSocket_MessageReceived;
        }

        public delegate void MessageReceivedDelegate(string message);

        #region Private Methods

        private bool IsExistingClient(HostName hostname)
        {
            return _clients.All(client => client.RemoteHostName == hostname);
        }

        private void ServerSocket_MessageReceived(HostName hostname, string message)
        {
            if (!IsExistingClient(hostname))
            {
                _clients.Add(new UDPClientSocket(hostname, _listeningPortNumber));
                Debug.WriteLine("A new client has connected!");
            }

            MessageReceived(message);
        }

        #endregion
    }
}