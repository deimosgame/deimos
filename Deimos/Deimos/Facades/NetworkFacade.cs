using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;

namespace Deimos
{
    static class NetworkFacade
    {
        // FOR DEVELOPMENT PURPOSES ONLY //
        static public bool IsMultiplayer = true;
        static public bool ServerIsLocal = true;
        ////////         /////          //////

        static public bool Local = true;

        static public bool ThreadStart1 = false;
        static public bool ThreadStart2 = false;

        static public NetworkManager Network = new NetworkManager();

        static public MainHandler MainHandling = new MainHandler();
        static public DataHandler DataHandling = new DataHandler();
        static public NetworkHandler NetworkHandling = new NetworkHandler();

        static public Queue TCP_Sending = new Queue();
        static public Queue TCP_Receiving = new Queue();
        static public Queue UDP_Sending = new Queue();
        static public Queue UDP_Receiving = new Queue();

        static public PlayerManager Players = new PlayerManager();

        static public Thread Outgoing = new Thread(NetworkFacade.Network.HandleSend);
        static public Thread TCPIncoming = new Thread(NetworkFacade.Network.ReceiveTCP);
        static public Thread UDPIncoming = new Thread(NetworkFacade.Network.ReceiveUDP);
        static public Thread Interpret = new Thread(NetworkFacade.Network.Process);
        static public Thread MovePacket = new Thread(NetworkFacade.Network.SendMovePacket);
    }
}
