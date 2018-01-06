using System.Collections.Generic;
using System.Diagnostics;
using Windows.Networking;

namespace HangManServer
{
    public class SocketServer
    {
        private readonly int _portNumber;

        private readonly List<SocketClient> _clients;
        public MessageReceivedDelegate MessageReceived;

        public SocketServer(int portNumber)
        {
            _portNumber = portNumber;

            _clients = new List<SocketClient>();

            SocketListener listener = new SocketListener(_portNumber); //TODO: check if this works
            listener.MessageReceived += Listener_MessageReceived;
        }

        public delegate void MessageReceivedDelegate(string message);

        #region Private Methods

        private bool IsExistingClient(HostName hostname)
        {
            foreach (SocketClient client in _clients)
                if (client.GetRemoteHostName == hostname)
                    return false;

            return true;
        }

        private void Listener_MessageReceived(HostName hostName, string message)
        {
            if (!IsExistingClient(hostName))
            {
                _clients.Add(new SocketClient(hostName, _portNumber));
                Debug.WriteLine("New client connected!");
            }

            MessageReceived(message);
        }

        #endregion
    }
}