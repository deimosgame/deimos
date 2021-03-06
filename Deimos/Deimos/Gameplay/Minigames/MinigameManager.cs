﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Deimos
{
    class MinigameManager
    {
        public Knife KnifeMG;
        public Labyrinth LabyrinthMG;

        public float Remaining;

        public MinigameManager()
        {
            KnifeMG = new Knife();
            LabyrinthMG = new Labyrinth();
        }

        public float GetRemaining()
        {
            if (GameplayFacade.ThisPlayer.IsMG)
            {
                return Remaining;
            }

            else
            {
                return 0;
            }
        }

        public Player.Spec GetClass(byte player, string name)
        {
            switch (name)
            {
                case "knife":
                    if (player == 0x00)
                    {
                        return KnifeMG.PlayerOneClass;
                    }

                    return KnifeMG.PlayerTwoClass;

                case "labyrinth":
                    if (player == 0x00)
                    {
                        return LabyrinthMG.PlayerOneClass;
                    }

                    return LabyrinthMG.PlayerTwoClass;

                default:
                    return Player.Spec.Soldier;
            }
        }

        public SpawnLocation GetSpawn(byte player, string name)
        {
            Vector3 loc = default(Vector3);
            Vector3 ang = default(Vector3);

            switch (name)
            {
                case "knife":
                    loc = KnifeMG.Spawns[player].Location;
                    ang = KnifeMG.Spawns[player].Rotation;
                    break;
                case "labyrinth":
                    loc = LabyrinthMG.Spawns[player].Location;
                    ang = LabyrinthMG.Spawns[player].Rotation;
                    break;
                default :
                    break;
            }

            return new SpawnLocation(loc, ang);
            
        }
    }
}
