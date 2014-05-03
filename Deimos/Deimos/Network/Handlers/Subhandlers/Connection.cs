using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Deimos.Facades;

namespace Deimos
{
    public class Connection : Subhandler
    {
        // String storing the current mapname
        public string CurrentMap;

        // METHODS PROPER TO HANDLING CONNECTION DATAGRAMS

        // This method creates and return a new connection datagram
        public Packet Create(string email, string token)
        {
            Packet New = new Packet(Packet.PacketType.Connection);

            New.Packet_ID = 0x01;
            New.AddData(email);
            New.AddData(token);

            New.Encode();

            return New;
        }

        // This method interprets a received connection datagram
        // and if connection was succesfull, sets the map name
        // also returns a boolean for connection establishing
        public void Interpret(byte[] buf)
        {
            if (buf[4] == 0x01)
            {
                Packet Connect = new Packet(Packet.PacketType.Connection);
                Connect.Packet_ID = 0x01;
                Connect.Encoded_buffer = buf;

                // Succesful connection
                NetworkFacade.NetworkHandling.ServerConnected = true;
                // Extracting server map name
                CurrentMap = ExtractString(Connect.Encoded_buffer, 5);
            }
        }
    }
}
