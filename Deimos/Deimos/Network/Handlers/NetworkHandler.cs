using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Deimos
{
    class NetworkHandler
    {
        // Connection handling
        public Socket UDP_Socket;
        public Socket TCP_Socket;

        public NetworkStream Writer;
        public NetworkStream Reader;

        public IPAddress server_address;
        public IPAddress local_address;

        public int server_port;

        public IPEndPoint server_endpoint;
        public IPEndPoint ip_endpoint;
        public EndPoint end_point;

        public byte[] TCP_RBuf;
        public byte[] UDP_RBuf;

        public bool Handshook = false;
        public bool ServerConnected = false;

        // CONSTRUCTOR
        public NetworkHandler()
        {
            TCP_RBuf = new byte[576];
            UDP_RBuf = new byte[576];

            TCP_Socket = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);
            UDP_Socket = new Socket(AddressFamily.InterNetwork,
                SocketType.Dgram, ProtocolType.Udp);
        }

        public void SetConnectivity(string server, int serverPort, string local, int localPort)
        {
            server_address = IPAddress.Parse(server);
            local_address = IPAddress.Parse(local);
            server_port = serverPort;

            server_endpoint = new IPEndPoint(server_address, serverPort);
            ip_endpoint = new IPEndPoint(local_address, localPort);
            end_point = (EndPoint)ip_endpoint;

            try
            {
                // establishing connection
                TCP_Socket.Connect(server_address, server_port);
            }
            catch
            {
                GeneralFacade.GameStateManager.Set(new ErrorScreenGS("TCP Connection could not be established"));
            }

            Writer = new NetworkStream(TCP_Socket);
            Reader = new NetworkStream(TCP_Socket);

            try
            {
                // Binding the port
                UDP_Socket.Bind(end_point);
            }
            catch
            {
                GeneralFacade.GameStateManager.Set(new ErrorScreenGS("Socket bind error"));
            }
        }

        // METHODS FOR HANDHSAKING AND SERVER CONNECTION
        public void ShakeHands()
        {
            Packet Handshake = NetworkFacade.MainHandling.Handshakes.Create();
            NetworkFacade.TCP_Sending.Enqueue(Handshake);
        }

        public void Connect()
        {
            Packet Connection = NetworkFacade.MainHandling.Connections.Create(
                Program.PlayerEmail, Program.PlayerToken);
            NetworkFacade.TCP_Sending.Enqueue(Connection);
        }

        // METHODS FOR DGRAMS

        public void TCP_Send(Packet pack)
        {
            if (Writer.CanWrite)
            {
                Writer.Write(pack.Encoded_buffer, 0, pack.Encoded_buffer.Length);
                Writer.Flush();
            }
        }

        public void TCP_Receive()
        {
            // clearing the byte buffer
            TCP_RBuf = new byte[576];

            // waiting for receipt
            if (Reader.CanRead)
            {
                Reader.Read(TCP_RBuf, 0, 576);
                Reader.Flush();
            }

            // once the packet received, handing it over to the
            // interpretation network thread

            while (!NetworkFacade.Network.TCPGuard)
            {
                System.Threading.Thread.Sleep(1);
            }

            NetworkFacade.TCP_Receiving.Enqueue(UDP_RBuf);
        }

        // Sending our datagrams to the server
        public void UDP_Send(Packet pack)
        {
            // sending packet to ip end point
            UDP_Socket.SendTo(pack.Encoded_buffer, server_endpoint);
        }

        // Receiving datagrams
        // this function returns true if packet can be identified
        // and false if datagram is corrupted, and will leave it
        public void UDP_Receive()
        {
            // clearing the byte buffer
            UDP_RBuf = new byte[576];

            // waiting for receipt
            UDP_Socket.ReceiveFrom(UDP_RBuf, ref end_point);

            // once the packet received, handing it over to the
            // interpretation network thread

            while (!NetworkFacade.Network.UDPGuard)
            {
                System.Threading.Thread.Sleep(1);
            }

            NetworkFacade.UDP_Receiving.Enqueue(UDP_RBuf);

        }
    }
}
