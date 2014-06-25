using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Deimos
{
    class RespawnGS : GameStateObj
    {
        string Instance;
        bool Returning;

        public RespawnGS(string instance, bool returning)
        {
            Instance = instance;
            Returning = returning;
        }

        public override GameStates GameState
        {
            get { return GameStates.Respawning; }
        }

        public override void PreSet()
        {
            if (Returning)
            {
                GameplayFacade.BulletManager = new BulletManager();
                GameplayFacade.ThisPlayer.Inventory = new WeaponManager();
            }

            GameplayFacade.ThisPlayer.InitializeInventory(GameplayFacade.ThisPlayer.Class);

            SpawnLocation spawn = GeneralFacade.SceneManager.CurrentScene.GetRandomSpawn();
            GameplayFacade.ThisPlayer.PlayerSpawn(spawn.Location, spawn.Rotation);
        }

        public override void PostUnset()
        {
            //
        }
    }
}
