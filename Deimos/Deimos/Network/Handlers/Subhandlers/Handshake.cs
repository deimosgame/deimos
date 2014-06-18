using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    public class Handshake : Subhandler
    {

        // METHODS PROPER TO HANDSHAKE PACKET HANDLING

        // This method creates and returns a handshaking packet
        public Packet Create()
        {
            Packet New = new Packet(Packet.PacketType.Handshake);

            New.Packet_ID = 0x00;
            New.Write(0x01);
            New.Encode();

            return New;
        }

        // This method interprets a received handshaking packet
        public void Interpret(byte[] buf)
        {
            if (buf[4] == 0x01)
            {
                NetworkFacade.NetworkHandling.Handshook = true;
            }
            else
            {
                
            }
        }
    }
}
