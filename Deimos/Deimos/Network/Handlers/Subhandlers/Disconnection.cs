using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    public class Disconnection : Subhandler
    {
        // String storing the latest disconnection reason
        public string Reason;

        // METHODS PROPER FOR HANDLING DISCONNECTION DATAGRAMS

        // This method is a player disconnect method
        public void Disco()
        {
            Packet Disco = new Packet(Packet.PacketType.Disconnection);

            Disco.Packet_ID = 0x02;
            Disco.Encode();

            NetworkFacade.UDP_Sending.Enqueue(Disco);
        }

        // This method interprets disconnect datagrams
        public void Interpret(byte[] buf)
        {
            Packet Disco = new Packet(Packet.PacketType.Disconnection);
            Disco.Encoded_buffer = buf;

            Reason = ExtractString(Disco.Encoded_buffer, 4);

            // Player disconnection
            NetworkFacade.NetworkHandling.ServerConnected = false;
            GeneralFacade.GameStateManager.Set(new StartMenuGS());
            GeneralFacade.GameStateManager.Set(new ErrorScreenGS("Disconnected: " + Reason));
        }
    }
}
