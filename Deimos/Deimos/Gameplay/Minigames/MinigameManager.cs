using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Deimos
{
    class MinigameManager
    {
        public Knife knife;

        public float Remaining;

        public MinigameManager()
        {
            knife = new Knife();
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
                        return knife.PlayerOneClass;
                    }

                    return knife.PlayerTwoClass;
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
                    loc = knife.Spawns[player].Location;
                    ang = knife.Spawns[player].Rotation;
                    break;
                default :
                    break;
            }

            return new SpawnLocation(loc, ang);
            
        }
    }
}
