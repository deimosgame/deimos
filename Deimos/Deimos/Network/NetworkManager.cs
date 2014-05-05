using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    class NetworkManager
    {
        public void HandleSend()
        {
            while (true)
            {
                if (NetworkFacade.Sending.Count != 0)
                {
                    Packet p = (Packet)NetworkFacade.Sending.Dequeue();

                    if (p != null)
                    {
                        NetworkFacade.NetworkHandling.Send(p);
                    }
                }
            }
        }

        public void HandleReceive()
        {
            while (true)
            {
                NetworkFacade.NetworkHandling.Receive();
            }
        }

        public void Process()
        {
            while (true)
            {
                if (NetworkFacade.Receiving.Count != 0)
                {
                    byte[] b = (byte[])NetworkFacade.Receiving.Dequeue();

                    if (b != null)
                    {
                        NetworkFacade.MainHandling.Distribute(b);
                    }
                }

                NetworkFacade.MainHandling.Process();
            }
        }


        public void UpdateWorld()
        {
            while (true)
            {
                NetworkFacade.DataHandling.Process();

                if (NetworkFacade.IsMultiplayer)
                {
                    foreach (KeyValuePair<byte, Player> p in NetworkFacade.Players)
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

        public void SendMovePacket()
        {
            Vector3 OldPos = Vector3.Zero;
            Vector3 OldRot = Vector3.Zero;

            while (true)
            {
                if (OldPos != GameplayFacade.ThisPlayer.Position
                    || OldRot != GameplayFacade.ThisPlayer.Rotation)
                {
                    NetworkFacade.MainHandling.Moves.Send(
                      NetworkFacade.MainHandling.Moves.Create()
                      );
                }

                OldPos = GameplayFacade.ThisPlayer.Position;
                OldRot = GameplayFacade.ThisPlayer.Rotation;

                System.Threading.Thread.Sleep(20);
            }
        }
    }
}
