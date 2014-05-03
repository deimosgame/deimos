using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Deimos.Facades
{
    static class NetworkFacade
    {
        // FOR DEVELOPMENT PURPOSES ONLY //
        static public bool IsMultiplayer = false;
        ////////         /////          //////

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

        static void HandleSend()
        {
            while (true)
            {
                if (NetworkFacade.Sending.Count != 0)
                {
                    NetworkFacade.NetworkHandling.Send(
                        (Packet)NetworkFacade.Sending.Dequeue()
                    );
                }

                Thread.Sleep(10);
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
                if (NetworkFacade.Receiving.Count != 0)
                {
                    NetworkFacade.MainHandling.Distribute(
                        (byte[])NetworkFacade.Receiving.Dequeue()
                    );
                }

                Thread.Sleep(5);

                NetworkFacade.MainHandling.Process();
            }
        }


        static void UpdateWorld()
        {
            while (true)
            {

            }
        }
    }
}
