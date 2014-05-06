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
        static public bool ServerIsLocal = false;
        ////////         /////          //////

        static public bool ThreadStart1 = false;
        static public bool ThreadStart2 = false;

        static public NetworkManager Network = new NetworkManager();

        static public MainHandler MainHandling = new MainHandler();
        static public DataHandler DataHandling = new DataHandler();
        static public NetworkHandler NetworkHandling = new NetworkHandler();

        static public Queue Sending = new Queue();
        static public Queue Receiving = new Queue();

        static public Dictionary<byte, Player> Players = new Dictionary<byte, Player>();

        static public Thread Outgoing = new Thread(NetworkFacade.Network.HandleSend);
        static public Thread Incoming = new Thread(NetworkFacade.Network.HandleReceive);
        //static public Thread Interpret = new Thread(NetworkFacade.Network.Process);
        //static public Thread World = new Thread(NetworkFacade.Network.UpdateWorld);
        static public Thread MovePacket = new Thread(NetworkFacade.Network.SendMovePacket);
    }
}
