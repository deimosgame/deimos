using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Deimos.Facades;

namespace Deimos
{
    class PlayerList : Subhandler
    {
        public uint N_Players;

        public void Interpret(byte[] buf)
        {
            N_Players = buf[4];

            int n = 0;
            int i = 5;

            while (n < N_Players)
            {
                byte uid = buf[i];
                string name = ExtractString(buf, i + 1);
                n++;
                i += name.Length + 2;

                Player playa = new Player();
                playa.Name = name;

                NetworkFacade.Players.Add(uid, playa);
            }
        }
    }
}
