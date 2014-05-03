using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;

namespace Deimos.Facades
{
    static class NetworkFacade
    {
        // FOR DEVELOPMENT PURPOSES ONLY //
        static public bool IsMultiplayer = true;
        ////////         /////          //////

        static public string Username = "Vomuseind";

        static public MainHandler MainHandling = new MainHandler();
        static public DataHandler DataHandling = new DataHandler();
        static public NetworkHandler NetworkHandling = new NetworkHandler();

        static public Queue Sending = new Queue();
        static public Queue Receiving = new Queue();

        static public Dictionary<byte, Player> Players =
        new Dictionary<byte, Player>();

        static public Thread Outgoing = new Thread(new ThreadStart(HandleSend));
        static public Thread Incoming = new Thread(new ThreadStart(HandleReceive));
        static public Thread Interpret = new Thread(new ThreadStart(Process));
        static public Thread World = new Thread(new ThreadStart(UpdateWorld));
        static public Thread MovePacket = new Thread(new ThreadStart(SendMovePacket));

        static void HandleSend()
        {
            while (true)
            {
                if (NetworkFacade.Sending.Count != 0
                    && (Packet)NetworkFacade.Sending.Peek() != null)
                {
                    NetworkFacade.NetworkHandling.Send(
                        (Packet)NetworkFacade.Sending.Dequeue()
                    );
                }

                Thread.Sleep(25);
            }
        }

        static void HandleReceive()
        {
            while (true)
            {
                NetworkFacade.NetworkHandling.Receive();
            }
        }

        static void Process()
        {
            while (true)
            {
                if (NetworkFacade.Receiving.Count != 0
                    && (byte[])NetworkFacade.Receiving.Peek() != null)
                {
                    NetworkFacade.MainHandling.Distribute(
                        (byte[])NetworkFacade.Receiving.Dequeue()
                    );
                }

                NetworkFacade.MainHandling.Process();

                Thread.Sleep(25);
            }
        }


        static void UpdateWorld()
        {
            while (true)
            {
                NetworkFacade.DataHandling.Process();
            }
        }

        static void SendMovePacket()
        {
            Vector3 OldPos = Vector3.Zero;

            while (true)
            {
                if (OldPos != GameplayFacade.ThisPlayer.Position)
                {
                    Sending.Enqueue(MainHandling.Moves.Create());
                }

                OldPos = GameplayFacade.ThisPlayer.Position;

                Thread.Sleep(1000);
            }
        }
    }
}
