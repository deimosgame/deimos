using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    class MinigameTrigger : Subhandler
    {
        public void SendBegin(byte enemy)
        {
            Packet p = new Packet(Packet.PacketType.Minigame);

            p.Packet_ID = 0x09;
            p.Write(GetNextMG());
            p.Write(0x01);
            p.Write(enemy);
            p.Encode();

            NetworkFacade.TCP_Sending.Enqueue(p);
        }

        public void SendEnd(byte mg, byte enemy)
        {
            Packet p = new Packet(Packet.PacketType.Minigame);

            p.Packet_ID = 0x09;
            p.Write(mg);
            p.Write(0x00);
            p.Write(enemy);
            p.Encode();

            NetworkFacade.TCP_Sending.Enqueue(p);
        }

        public void Interpret(byte[] buf)
        {
            if (buf[5] == 0x01)
            {
                GameplayFacade.ThisPlayer.IsMG = true;
                GameplayFacade.ThisPlayer.MGNumber = buf[6];
                NetworkFacade.Players.List[buf[7]].CurrentInstance = GetInstanceName(buf[4]);

                switch (buf[4])
                {
                    case 0x00:
                        GameplayFacade.Minigames.knife.Load();
                        break;
                    default:
                        return;
                }
            }
            else
            {
                NetworkFacade.Players.List[buf[7]].CurrentInstance = "main";

                switch (buf[4])
                {
                    case 0x00:
                        GameplayFacade.Minigames.knife.Terminate();
                        break;
                    default :
                        return;
                }
            }
        }

        public string GetInstanceName(byte b)
        {
            switch (b)
            {
                case 0x00:
                    return "knife";
                default :
                    return "main";
            }
        }

        public byte GetNextMG()
        {
            Random rng = new Random();
            byte b = (byte)rng.Next(0, 0);

            return b;
        }
    }
}
