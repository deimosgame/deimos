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
            GameplayFacade.ThisPlayer.PlayerRespawn(new Vector3(2f, 0f, -20f),
                        Vector3.Zero, "main");
        }

        public override void PostUnset()
        {
            //
        }
    }
}
