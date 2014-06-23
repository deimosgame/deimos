using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    class PlayerList : Subhandler
    {
        public uint N_Players;

        private Dictionary<byte, Player> OldPList =
            new Dictionary<byte, Player>();

        public void Interpret(byte[] buf)
        {
            OldPList = new Dictionary<byte, Player>(NetworkFacade.Players.List);

            NetworkFacade.Players.List.Clear();

            int n = 0;
            int i = 4;

            while (buf[i+1] != 0x00)
            {
                byte uid = buf[i];
                string name = ExtractString(buf, i + 1);
                n++;
                i += name.Length + 2;

                if (name != Program.Username)
                {
                    Player playa = new Player();
                    playa.Name = name;

                    NetworkFacade.Players.List.Add(uid, playa);
                }
                else
                {
                    GameplayFacade.ThisPlayer.Playerbyte = uid;
                }
            }

            N_Players = (uint)n;

            HandleRenew();
            NetworkFacade.Players.LoadModels();
        }

        public void HandleRenew()
        {
            List<Player> ToBeRemoved = new List<Player>();

            foreach (KeyValuePair<byte, Player> p in OldPList)
            {
                if (!NetworkFacade.Players.List.ContainsKey(p.Key))
                {
                    ToBeRemoved.Add(p.Value);
                }
            }

            foreach (Player p in ToBeRemoved)
            {
                GeneralFacade.SceneManager.ModelManager.RemoveLevelModel
                    (p.Name);
            }
        }
    }
}
