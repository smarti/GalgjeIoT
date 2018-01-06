using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace HangManClient
{
    public class SocketClient
    {
        private readonly string _remotePortNumber;

        private StreamSocket _socket;

        public HostName GetRemoteHostName { get; }

        public SocketClient(HostName serverHostName, int serverPort)
        {
            GetRemoteHostName = serverHostName;
            _remotePortNumber = serverPort.ToString();

            ConnectClient();
        }

        #region Private Methods

        private async void ConnectClient()
        {
            try
            {
                _socket = new StreamSocket();

                Debug.WriteLine("Client is trying to connect...");

                await _socket.ConnectAsync(GetRemoteHostName, _remotePortNumber);

                Debug.WriteLine("Client connected!");

                //Send test Message
                SendMessage("ConnectionTest");
            }
            catch (Exception ex)
            {
                SocketErrorStatus webErrorStatus = SocketError.GetStatus(ex.GetBaseException().HResult);
                Debug.WriteLine(webErrorStatus.ToString() != "Unknown" ? webErrorStatus.ToString() : ex.Message);

                //Reconnect if connection timed out
                //if (webErrorStatus == SocketErrorStatus.ConnectionTimedOut)
                //{
                //    await Task.Delay(1000);
                //    ConnectClient();
                //}
            }
        }

        #endregion

        public async void SendMessage(string message)
        {
            try
            {
                using (DataWriter writer = new DataWriter(_socket.OutputStream))
                {
                    writer.WriteUInt32(writer.MeasureString(message));
                    writer.WriteString(message);

                    await writer.StoreAsync();
                    await writer.FlushAsync();
                }

                Debug.WriteLine($"Send message: {message}");
            }
            catch (Exception ex)
            {
                SocketErrorStatus webErrorStatus = SocketError.GetStatus(ex.GetBaseException().HResult);
                Debug.WriteLine(webErrorStatus.ToString() != "Unknown" ? webErrorStatus.ToString() : ex.Message);

                //reconnect if connection timed out
                //if (webErrorStatus == SocketErrorStatus.ConnectionTimedOut)
                //    ConnectClient();
            }
        }
    }
}