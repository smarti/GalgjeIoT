using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace HangManClient
{
    class InStream
    {
        private int _port;

        private StreamSocketListener _listener;
        private OutStream _server;

        public delegate void DataOntvangenDelegate(string data);
        public DataOntvangenDelegate OndataOntvangen;

        public InStream(int port, OutStream server)
        {
            _port = port;
            _server = server;
            StartListener();
        }

        private async void StartListener()
        {
            _listener = new StreamSocketListener();
            _listener.ConnectionReceived += Listener_ConnectionReceived;

            await _listener.BindServiceNameAsync(_port.ToString());
        }

        private async void Listener_ConnectionReceived(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
        {
            DataReader reader = new DataReader(args.Socket.InputStream);

            try
            {
                while (true)
                {

                    uint sizeFieldCount = await reader.LoadAsync(sizeof(uint));
                    if (sizeFieldCount != sizeof(uint))
                        return; //Disconnect

                    uint stringlength = reader.ReadUInt32();
                    uint actualStringLength = await reader.LoadAsync(stringlength);
                    if (stringlength != actualStringLength)
                        return; //Disconnect

                    OndataOntvangen?.Invoke(reader.ReadString(actualStringLength));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        //TODO: OnDataOntvangen
    }
}
