using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    class MGSpawningGS : GameStateObj
    {
        string Minigame;

        public MGSpawningGS(string mg_name)
        {
            Minigame = mg_name;
        }

        public override GameStates GameState
        {
            get { return GameStates.MinigameSpawning; }
        }

        public override void PreSet()
        {
            GameplayFacade.BulletManager = new BulletManager();

            GameplayFacade.ThisPlayer.Inventory = new WeaponManager();

            GameplayFacade.ThisPlayer.InitializeInventory(GameplayFacade.Minigames.GetClass(
                GameplayFacade.ThisPlayer.MGNumber, Minigame)
                );

            GameplayFacade.ThisPlayer.PlayerSpawn(GameplayFacade.Minigames.GetSpawn(
                GameplayFacade.ThisPlayer.MGNumber, Minigame).Location,
                GameplayFacade.Minigames.GetSpawn(
                GameplayFacade.ThisPlayer.MGNumber, Minigame).Rotation
                );
        }

        public override void PostUnset()
        {
            //
        }
    }
}
