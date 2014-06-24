using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    class Damage
    {
        public void Send(byte player, int dmg)
        {
            Packet p = new Packet(Packet.PacketType.Damage);

            p.Packet_ID = 0x0C;

            p.Write(player);
            p.AddData(dmg);

            p.Encode();

            NetworkFacade.TCP_Sending.Enqueue(p);
        }

        public void Interpret(byte[] buf)
        {
            GameplayFacade.ThisPlayer.Hurt(buf[4]);
        }
    }
}
