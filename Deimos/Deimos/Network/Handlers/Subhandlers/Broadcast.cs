﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    public class Broadcast : Subhandler
    {

        // METHODS PROPER TO WORLD BROADCAST HANDLING

        // This method creates and sends a confirmation dgram
        // It is used to let the server know we have receives the identified
        // world matrix.
        public void ConfirmReceipt(int uuid)
        {
            Packet confirm = new Packet(Packet.PacketType.Broadcast);

            confirm.Packet_ID = 0x04;
            confirm.AddData(uuid);
            confirm.Encode();

            NetworkFacade.UDP_Sending.Enqueue(confirm);
        }

        // This is the method that interprets the whole packet
        public void Interpret(Packet p)
        {
            Data datapack = ExtractData(p.Encoded_buffer, 8);
            if (datapack != null)
            {
                NetworkFacade.DataHandling.Handle(datapack);
                int i = datapack.end_index;

                while (i < p.Encoded_buffer.Length && p.Encoded_buffer[i] != 0x00)
                {
                    Data new_data = ExtractData(p.Encoded_buffer, i);
                    if (new_data != null)
                    {
                        NetworkFacade.DataHandling.Handle(new_data);
                        i = new_data.end_index;
                    }
                    else
                    {
                        return;
                    }

                }

                if (p.Split && p.Index < p.Total_Packets)
                {
                    Interpret(p.Next);
                }
            }
        }

        // This method processes all ongoing packet information
        // and tries to assemble any split packets
        public void Process()   
        {
            for (int i = 0; i < Ongoing.Count; i++)
            {
                Packet p = Ongoing.ElementAt(i);
                Interpret(p);
                Ongoing.RemoveAt(i);
            }
        }
    }
}
