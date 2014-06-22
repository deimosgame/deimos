using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    class NetworkManager
    {
        public bool TCPGuard = true;
        public bool UDPGuard = true;

        public void HandleSend()
        {
            while (true)
            {
                if (NetworkFacade.TCP_Sending.Count != 0)
                {
                    Packet a = (Packet)NetworkFacade.TCP_Sending.Dequeue();

                    if (a != null)
                    {
                        NetworkFacade.NetworkHandling.TCP_Send(a);
                    }
                }

                if (NetworkFacade.UDP_Sending.Count != 0)
                {
                    Packet p = (Packet)NetworkFacade.UDP_Sending.Dequeue();

                    if (p != null)
                    {
                        NetworkFacade.NetworkHandling.UDP_Send(p);
                    }
                }
            }
        }

        public void ReceiveTCP()
        {
            while (true)
            {
                NetworkFacade.NetworkHandling.TCP_Receive();
            }
        }

        public void ReceiveUDP()
        {
            while (true)
            {
                NetworkFacade.NetworkHandling.UDP_Receive();
            }
        }

        public void Process()
        {
            while (true)
            {
                if (NetworkFacade.TCP_Receiving.Count != 0)
                {
                    TCPGuard = false;
                    byte[] a = (byte[])NetworkFacade.TCP_Receiving.Dequeue();
                    TCPGuard = true;
                    if (a != null)
                    {
                        NetworkFacade.MainHandling.Distribute(a);
                    }
                }

                if (NetworkFacade.UDP_Receiving.Count != 0)
                {
                    UDPGuard = false;
                    byte[] b = (byte[])NetworkFacade.UDP_Receiving.Dequeue();
                    UDPGuard = true;
                    if (b != null)
                    {
                        NetworkFacade.MainHandling.Distribute(b);
                    }
                }

                NetworkFacade.MainHandling.Process();
                NetworkFacade.DataHandling.Process();

                System.Threading.Thread.Sleep(1);
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

                System.Threading.Thread.Sleep(15);
            }
        }
    }
}
