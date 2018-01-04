using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace HangManClient
{
    public class SocketClient
    {
        private HostName _serverHostName;
        private string _serverPortNumber;

        private StreamSocket _socket;

        public SocketClient(string serverHostName, int serverPort)
        {
            _serverHostName = new HostName(serverHostName);
            _serverPortNumber = serverPort.ToString();

            ConnectClient();
        }

        private async void ConnectClient()
        {
            try
            {
                _socket = new StreamSocket();

                Debug.WriteLine("Client is trying to connect...");

                await _socket.ConnectAsync(_serverHostName, _serverPortNumber);

                Debug.WriteLine("Client connected!");

                //Send Message
                SendMessage("Hello");
            }
            catch (Exception ex)
            {
                SocketErrorStatus webErrorStatus = SocketError.GetStatus(ex.GetBaseException().HResult);
                Debug.WriteLine(webErrorStatus.ToString() != "Unknown" ? webErrorStatus.ToString() : ex.Message);

                //TODO: restart connection
            }
        }

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
