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
        static public Thread MovePacket = new Thread(new ThreadStart(SendMovePacket));

        static void HandleSend()
        {
            while (true)
            {
                if (NetworkFacade.Sending.Count != 0)
                {
                    Packet p = (Packet)Sending.Dequeue();

                    if (p != null)
                    {
                        NetworkHandling.Send(p);
                    }
                }
            }
        }

        static void HandleReceive()
        {
            while (true)
            {
                NetworkHandling.Receive();
            }
        }

        static void Process()
        {
            while (true)
            {
                if (NetworkFacade.Receiving.Count != 0)
                {
                    byte[] b = (byte[])Receiving.Dequeue();

                    if (b != null)
                    {
                        MainHandling.Distribute(b);
                    }
                }

                NetworkFacade.MainHandling.Process();
            }
        }


        static void UpdateWorld()
        {
            while (true)
            {
                NetworkFacade.DataHandling.Process();

                if (IsMultiplayer)
                {
                    foreach (KeyValuePair<byte, Player> p in Players)
                    {
                        if (GeneralFacade.SceneManager.ModelManager.LevelModelExists(p.Value.Name)
                            && p.Value.IsAlive())
                        {
                            GeneralFacade.SceneManager.ModelManager.GetLevelModel(p.Value.Name).show = true;

                            GeneralFacade.SceneManager.ModelManager.GetLevelModel(p.Value.Name).Position =
                                p.Value.Position;

                            GeneralFacade.SceneManager.ModelManager.GetLevelModel(p.Value.Name).Rotation =
                                p.Value.Rotation;
                        }
                    }
                }
            }
        }

        static void SendMovePacket()
        {
            Vector3 OldPos = Vector3.Zero;
            Vector3 OldRot = Vector3.Zero;

            while (true)
            {
                if (OldPos != GameplayFacade.ThisPlayer.Position
                    || OldRot != GameplayFacade.ThisPlayer.Rotation)
                {
                    MainHandling.Moves.Send(
                        MainHandling.Moves.Create()
                        );
                }

                OldPos = GameplayFacade.ThisPlayer.Position;
                OldRot = GameplayFacade.ThisPlayer.Rotation;

                Thread.Sleep(15);
            }
        }
    }
}
