﻿using System;
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
            OldPList = new Dictionary<byte, Player>(NetworkFacade.Players);

            NetworkFacade.Players.Clear();

            int n = 0;
            int i = 4;

            while (buf[i+1] != 0x00)
            {
                byte uid = buf[i];
                string name = ExtractString(buf, i + 1);
                n++;
                i += name.Length + 2;

                DisplayFacade.DebugScreen.Debug("Tried: " + name);

                if (name != Program.Username)
                {
                    Player playa = new Player();
                    playa.Name = name;

                    NetworkFacade.Players.Add(uid, playa);
                    DisplayFacade.DebugScreen.Debug(uid.ToString() + " " + playa.Name);
                }
                else
                {
                    DisplayFacade.DebugScreen.Debug("Tried to add self");
                }
            }

            N_Players = (uint)n;

            HandleRenew();
            HandleModels();
        }

        public void HandleRenew()
        {
            List<Player> ToBeRemoved = new List<Player>();

            foreach (KeyValuePair<byte, Player> p in OldPList)
            {
                if (!NetworkFacade.Players.ContainsKey(p.Key))
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

        public void HandleModels()
        {
                foreach (KeyValuePair<byte, Player> p in NetworkFacade.Players)
                {
                    if (!GeneralFacade.SceneManager.ModelManager.LevelModelExists(p.Value.Name))
                    {
                    GeneralFacade.SceneManager.ModelManager.LoadModel(
                        p.Value.Name,
                        p.Value.GetModelName(),
                        p.Value.Position,
                        p.Value.Rotation,
                        5,
                        LevelModel.CollisionType.None
                        );
                    }
                }

        }
        
    }
}
