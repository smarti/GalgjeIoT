using System;
using System.Diagnostics;
using System.IO;
using Windows.Networking;
using Windows.Networking.Sockets;

namespace UDPSockets
{
    public class UDPClientSocket
    {
        public HostName RemoteHostName { get; }
        private readonly string _remotePortNumber;

        public UDPClientSocket(HostName remoteHostName, int remotePortNumber)
        {
            _remotePortNumber = remotePortNumber.ToString();
            RemoteHostName = remoteHostName;
        }

        public async void SendMessage(string message)
        {
            try
            {
                using (DatagramSocket dataGramSocket = new DatagramSocket())
                {
                    using (Stream outputStream =
                        (await dataGramSocket.GetOutputStreamAsync(RemoteHostName, _remotePortNumber))
                        .AsStreamForWrite())
                    {
                        using (StreamWriter streamWriter = new StreamWriter(outputStream))
                        {
                            await streamWriter.WriteLineAsync(message);
                            await streamWriter.FlushAsync();
                        }
                    }
                }

                Debug.WriteLine($"Message sent: \"{message}\"");
            }
            catch (Exception ex)
            {
                SocketErrorStatus webErrorStatus = SocketError.GetStatus(ex.GetBaseException().HResult);
                Debug.WriteLine(webErrorStatus.ToString() != "Unknown" ? webErrorStatus.ToString() : ex.Message);
            }
        }
    }
}