using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Sockets;

namespace HangManClient
{
    class ClientSocket : IDisposable
    {
        private HostName _hostName;
        private string _portNumber;

        private StreamSocket _socket;

        public ClientSocket(string hostname, string portNumber)
        {
            _hostName = new HostName(hostname);
            _portNumber = portNumber;

            StartClient();
        }

        private async void StartClient()
        {
            try
            {
                _socket = new StreamSocket();

                Debug.WriteLine("Client is trying to connect...");

                await _socket.ConnectAsync(_hostName, _portNumber);

                Debug.WriteLine("Client connected!");
            }
            catch (Exception ex)
            {
                SocketErrorStatus webErrorStatus = SocketError.GetStatus(ex.GetBaseException().HResult);
                Debug.WriteLine(webErrorStatus.ToString() != "Unknown" ? webErrorStatus.ToString() : ex.Message);

                //Retry connection
                StartClient();
            }

            //TODO: Make fitting request
            Request("RequestConnection");
        }

        public string Request(string request)
        {
            try
            {
                using (Stream outputStream = _socket.OutputStream.AsStreamForWrite())
                {
                    using (StreamWriter streamWriter = new StreamWriter(outputStream))
                    {
                        streamWriter.WriteLine(request);
                        streamWriter.Flush();
                    }
                }

                return Response();
            }
            catch (Exception ex)
            {
                SocketErrorStatus webErrorStatus = SocketError.GetStatus(ex.GetBaseException().HResult);
                Debug.WriteLine(webErrorStatus.ToString() != "Unknown" ? webErrorStatus.ToString() : ex.Message);

                return null; //TODO: Fix this
            }
        }

        private string Response()
        {
            string response;

            using (Stream inputStream = _socket.InputStream.AsStreamForRead())
            {
                using (StreamReader streamReader = new StreamReader(inputStream))
                {
                    response = streamReader.ReadLine();
                }
            }

            return response;
        }

        public void Dispose()
        {
            _socket.Dispose();
        }
    }
}
