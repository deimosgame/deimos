using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Deimos.Facades;

namespace Deimos
{
    class NetworkHandler
    {
        // Connection handling
        public Socket socket;

        public IPAddress server_address;

        public int server_port;

        public IPEndPoint server_endpoint;
        public IPEndPoint ip_endpoint;
        public EndPoint end_point;

        public byte[] receive_buffer;

        public bool Handshook = false;
        public bool ServerConnected = false;

        // CONSTRUCTOR
        public NetworkHandler()
        {
            receive_buffer = new byte[576];

            socket = new Socket(AddressFamily.InterNetwork,
                SocketType.Dgram, ProtocolType.Udp);

            server_address = IPAddress.Parse("127.0.0.1");

            server_port = 1518;

            server_endpoint = new IPEndPoint(server_address, server_port);
            ip_endpoint = new IPEndPoint(server_address, 8461);
            end_point = (EndPoint)ip_endpoint;

            // Binding the port
            socket.Bind(end_point);
        }

        // METHODS FOR HANDHSAKING AND SERVER CONNECTION
        public void ShakeHands()
        {
            Packet Handshake = NetworkFacade.MainHandling.Handshakes.Create();
            NetworkFacade.Sending.Enqueue(Handshake);
        }

        public void Connect()
        {
            Packet Connection = NetworkFacade.MainHandling.Connections.Create(
                Program.PlayerEmail, Program.PlayerToken);
            NetworkFacade.Sending.Enqueue(Connection);
        }

        // METHODS FOR DGRAMS

        // Sending our datagrams to the server
        public void Send(Packet pack)
        {
            // sending packet to ip end point
            socket.SendTo(pack.Encoded_buffer, server_endpoint);
        }

        // Receiving datagrams
        // this function returns true if packet can be identified
        // and false if datagram is corrupted, and will leave it
        public void Receive()
        {
            // clearing the byte buffer
            receive_buffer = new byte[576];

            // waiting for receipt
            socket.ReceiveFrom(receive_buffer, ref end_point);

            // once the packet received, handing it over to the
            // interpretation network thread
            NetworkFacade.Receiving.Enqueue(receive_buffer);
        }
    }
}
