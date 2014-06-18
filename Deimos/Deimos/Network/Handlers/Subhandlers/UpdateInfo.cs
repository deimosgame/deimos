using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    class UpdateInfo : Subhandler
    {
        public void Update()
        {
            byte[] buf = new byte[4];
            buf[0] = GameplayFacade.ThisPlayer.WeaponModel;
            buf[1] = GameplayFacade.ThisPlayer.Model;
            buf[2] = 0x00;
            buf[3] = GameplayFacade.ThisPlayer.Alive;

            Packet update = new Packet(Packet.PacketType.UpdateInfo);
            update.Packet_ID = 0x07;
            update.AddData(buf[0]);
            update.AddData(buf[1]);
            update.AddData(buf[2]);
            update.AddData(buf[3]);
            update.Encode();

            NetworkFacade.Sending.Enqueue(update);
        }
    }
}
