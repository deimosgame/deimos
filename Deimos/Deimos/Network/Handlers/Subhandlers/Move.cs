using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Deimos.Facades;

namespace Deimos
{
    public class Move : Subhandler
    {

        // METHODS FOR PROPER MOVE DGRAM HANDLING

        // This method creates and sends an updated move dgram
        public void SendMV(float[] player_data)
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
                NetworkFacade.Sending.Enqueue(move);
            }
            else
            {
                // Corrupt packet
            }
        }
    }
}
