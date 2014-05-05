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

        static public NetworkManager Network;

        static public MainHandler MainHandling;
        static public DataHandler DataHandling;
        static public NetworkHandler NetworkHandling;

        static public Queue Sending;
        static public Queue Receiving;

        static public Dictionary<byte, Player> Players;

        static public Thread Outgoing;
        static public Thread Incoming;
        static public Thread Interpret;
        static public Thread World;
        static public Thread MovePacket;
    }
}
