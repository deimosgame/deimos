using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    public class Chat : Subhandler
    {

        // METHODS PROPER FOR CHAT DATAGRAM HANDLING

        // This method is to send a chat datagram
        public void Send(string message)
        {
            Packet M = new Packet(Packet.PacketType.Chat);

            M.Packet_ID = 0x03;
            M.AddData(message);
            M.Encode();

            NetworkFacade.TCP_Sending.Enqueue(M);
        }

        // This method is to interpret a received chat datagran
        public string Interpret(Packet m)
        {
            if (m.Encoded_buffer[4] != 0x00)
            {
                Console.WriteLine(ExtractString(m.Encoded_buffer, 4));
                return ExtractString(m.Encoded_buffer, 4);
            }

            return null;
        }

        // This method process all ongoing chat datagrams and interprets them
        public void Process()
        {
            foreach (Packet p in Ongoing)
            {
                Interpret(p);
            }

            Ongoing.Clear();
        }
    }
}
