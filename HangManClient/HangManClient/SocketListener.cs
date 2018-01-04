﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace HangManClient
{
    public class SocketListener
    {
        private string _portNumber;

        public SocketListener(int portNumber)
        {
            _portNumber = portNumber.ToString();

            StartListener();
        }

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

                //TODO: restart listener
            }
        }

        private async void StreamSocketListener_ConnectionReceived(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
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

                        //TODO: start method to handle message
                    }
                }
            }
        }
    }
}
