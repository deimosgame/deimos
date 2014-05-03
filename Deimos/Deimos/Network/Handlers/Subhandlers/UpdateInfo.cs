using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Deimos.Facades;

namespace Deimos
{
    class UpdateInfo : Subhandler
    {
        public void Update()
        {
            byte[] buf = new byte[3];
            buf[0] = GameplayFacade.ThisPlayer.WeaponModel;
            buf[1] = GameplayFacade.ThisPlayer.Model;
            buf[2] = 0x00;

            Packet update = new Packet(Packet.PacketType.UpdateInfo);
            update.Packet_ID = 0x07;
            update.AddData(buf[0]);
            update.AddData(buf[1]);
            update.AddData(buf[2]);
            update.Encode();

            NetworkFacade.Sending.Enqueue(update);
        }
    }
}
