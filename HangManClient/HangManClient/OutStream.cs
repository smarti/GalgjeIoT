using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Connectivity;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace HangManClient
{
    class OutStream
    {
        private string _thisIpAdress; //Je eigen IP Adres

        private string _serverIpAdress;
        private int _port;

        private StreamSocket _socket;
        private DataWriter _writer;

        public OutStream(string serverIpAddress, int port)
        {
            _serverIpAdress = serverIpAddress;
            _port = port;

            _thisIpAdress = GetIpAddress().ToString();
        }

        private IPAddress GetIpAddress()
        {
            IReadOnlyList<HostName> hosts = NetworkInformation.GetHostNames();
            foreach (HostName host in hosts)
            {
                
            }
        }
    }
}
