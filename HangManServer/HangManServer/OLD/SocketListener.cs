using System;
using System.Diagnostics;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace HangManServer
{
    public class SocketListener
    {
        private readonly string _portNumber;
        public MessageReceivedDelegate MessageReceived;

        public SocketListener(int portNumber)
        {
            _portNumber = portNumber.ToString();

            StartListener();
        }

        public delegate void MessageReceivedDelegate(HostName hostName, string message);

        #region Private Methods

        private async void StartListener()
        {
            try
            {
                StreamSocketListener streamSocketListener = new StreamSocketListener();

                //When new connections are received this event is raised
                streamSocketListener.ConnectionReceived += StreamSocketListener_ConnectionReceived;

                //Start listening  for incoming connections on the specified port.
                await streamSocketListener.BindServiceNameAsync(_portNumber);

                Debug.WriteLine("Server is listening...");
            }
            catch (Exception ex)
            {
                SocketErrorStatus webErrorStatus = SocketError.GetStatus(ex.GetBaseException().HResult);
                Debug.WriteLine(webErrorStatus.ToString() != "Unknown" ? webErrorStatus.ToString() : ex.Message);
            }
        }

        private async void StreamSocketListener_ConnectionReceived(StreamSocketListener sender,
            StreamSocketListenerConnectionReceivedEventArgs args)
        {
            using (DataReader dataReader = new DataReader(args.Socket.InputStream))
            {
                uint sizeFieldCount = await dataReader.LoadAsync(sizeof(uint));

                if (sizeFieldCount == sizeof(uint))
                {
                    uint stringLength = dataReader.ReadUInt32();
                    uint actualStringLength = await dataReader.LoadAsync(stringLength);

                    if (stringLength == actualStringLength)
                    {
                        string message = dataReader.ReadString(actualStringLength);
                        HostName hostName = args.Socket.Information.RemoteHostName;

                        Debug.WriteLine($"Message received: {message}");
                        MessageReceived(hostName, message);
                    }
                }
            }
        }

        #endregion
    }
}