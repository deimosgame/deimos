using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Deimos
{
    class RespawnGS : GameStateObj
    {
        public override GameStates GameState
        {
            get { return GameStates.Respawning; }
        }

        public override void PreSet()
        {
            GameplayFacade.ThisPlayer.InitializeInventory(GameplayFacade.ThisPlayer.Class);
            if (GameplayFacade.ThisPlayer.Class != Player.Spec.Agent)
            {
                GameplayFacade.ThisPlayer.PlayerRespawn(new Vector3(15f, 0f, -38f),
                            Vector3.Zero, "main");
            }
            else
            {
                GameplayFacade.ThisPlayer.PlayerRespawn(new Vector3(17, 0, 88),
                    Vector3.Zero, "main");
            }
        }

        public override void PostUnset()
        {
            //
        }
    }
}
