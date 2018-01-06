using System;
using System.Diagnostics;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace HangManServer
{
    public class SocketClient
    {
        private readonly HostName _remoteHostName;
        private readonly string _remotePortNumber;

        private StreamSocket _socket;

        public SocketClient(HostName serverHostName, int serverPort)
        {
            _remoteHostName = serverHostName;
            _remotePortNumber = serverPort.ToString();

            ConnectClient();
        }

        public HostName GetRemoteHostName
            => _remoteHostName;

        #region Private Methods

        private async void ConnectClient()
        {
            try
            {
                _socket = new StreamSocket();

                Debug.WriteLine("Client is trying to connect...");

                await _socket.ConnectAsync(_remoteHostName, _remotePortNumber);

                Debug.WriteLine("Client connected!");

                //Send test Message
                SendMessage("ConnectionTest");
            }
            catch (Exception ex)
            {
                SocketErrorStatus webErrorStatus = SocketError.GetStatus(ex.GetBaseException().HResult);
                Debug.WriteLine(webErrorStatus.ToString() != "Unknown" ? webErrorStatus.ToString() : ex.Message);

                //Reconnect if connection timed out
                if (webErrorStatus == SocketErrorStatus.ConnectionTimedOut)
                    ConnectClient();
            }
        }

        #endregion

        public async void SendMessage(string message)
        {
            try
            {
                DataWriter writer = new DataWriter(_socket.OutputStream);

                writer.WriteUInt32(writer.MeasureString(message));
                writer.WriteString(message);

                await writer.StoreAsync();
                await writer.FlushAsync();
            }
            catch (Exception ex)
            {
                SocketErrorStatus webErrorStatus = SocketError.GetStatus(ex.GetBaseException().HResult);
                Debug.WriteLine(webErrorStatus.ToString() != "Unknown" ? webErrorStatus.ToString() : ex.Message);

                //reconnect if connection timed out
                if (webErrorStatus == SocketErrorStatus.ConnectionTimedOut)
                    ConnectClient();

                //Retry
                SendMessage(message);
            }
        }
    }
}
