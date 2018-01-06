using System;
using System.Diagnostics;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace HangManClient
{
    public class SocketClient
    {
        private readonly HostName _serverHostName;
        private readonly string _serverPortNumber;

        private StreamSocket _socket;

        public SocketClient(string serverHostName, int serverPort)
        {
            _serverHostName = new HostName(serverHostName);
            _serverPortNumber = serverPort.ToString();

            ConnectClient();
        }

        #region Private Methods

        private async void ConnectClient()
        {
            try
            {
                _socket = new StreamSocket();

                Debug.WriteLine("Client is trying to connect...");

                await _socket.ConnectAsync(_serverHostName, _serverPortNumber);

                Debug.WriteLine("Client connected!");

                //Send test Message
                SendMessage("ConnectionTest");
            }
            catch (Exception ex)
            {
                SocketErrorStatus webErrorStatus = SocketError.GetStatus(ex.GetBaseException().HResult);
                Debug.WriteLine(webErrorStatus.ToString() != "Unknown" ? webErrorStatus.ToString() : ex.Message);
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

                //TODO: retry Message
            }
        }
    }
}