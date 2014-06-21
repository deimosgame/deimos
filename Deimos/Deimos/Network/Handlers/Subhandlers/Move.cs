using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    public class Move : Subhandler
    {

        // METHODS FOR PROPER MOVE DGRAM HANDLING

            // This method creates a float array of size 10
            // containing current player information
            // and will return the array for it to be easily sent
        public float[] Create()
        {
            float[] data = new float[10];

            data[0] = GameplayFacade.ThisPlayer.Position.X;
            data[1] = GameplayFacade.ThisPlayer.Position.Y;
            data[2] = GameplayFacade.ThisPlayer.Position.Z;
            data[3] = GameplayFacade.ThisPlayer.Rotation.X;
            data[4] = GameplayFacade.ThisPlayer.Rotation.Y;
            data[5] = GameplayFacade.ThisPlayer.Velocity.X;
            data[6] = GameplayFacade.ThisPlayer.Velocity.Y;
            data[7] = GameplayFacade.ThisPlayer.Velocity.Z;
            data[8] = 0;
            data[9] = 0;

            return data;
        }

        // This method creates and sends an updated move dgram
        public void Send(float[] player_data)
        {
            if (player_data.Length == 10)
            {
                // clean packet

                // Let us create the packet
                Packet move = new Packet(Packet.PacketType.Move);

                move.Packet_ID = 0x05;

                // Because this datagram has a fixed size,
                // we can let ourselves use a foreach loop
                foreach (float f in player_data)
                {
                    move.AddData(f);
                }

                // Encoding the packet
                move.Encode();

                // Let us now add the packet to the sending queue
                NetworkFacade.UDP_Sending.Enqueue(move);
            }
            else
            {
                // Corrupt packet
            }
        }
    }
}
