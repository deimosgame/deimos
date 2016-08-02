﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    class SpawningGS : GameStateObj
    {
        string Instance;

        public SpawningGS(string instance)
        {
            Instance = instance;
        }

        public override GameStates GameState
        {
            get { return GameStates.Spawning; }
        }

        public override void PreSet()
        {
            GameplayFacade.BulletManager = new BulletManager();

            GameplayFacade.ThisPlayer = new LocalPlayer();
            GameplayFacade.ThisPlayerPhysics = new Physics();
            GameplayFacade.ThisPlayerDisplay = new Display();

            GameplayFacade.ThisPlayer.Inventory = new WeaponManager();
            GameplayFacade.ThisPlayer.InitializeInventory(GameplayFacade.ThisPlayer.Class);

            SpawnLocation spawn = GeneralFacade.SceneManager.CurrentScene.GetRandomSpawn();
            GameplayFacade.ThisPlayer.PlayerSpawn(spawn.Location, spawn.Rotation);

            if (NetworkFacade.IsMultiplayer && !NetworkFacade.ThreadStart2)
            {
                NetworkFacade.MovePacket.Start();

                NetworkFacade.ThreadStart2 = true;

                NetworkFacade.MainHandling.Moves.Send(NetworkFacade.MainHandling.Moves.Create());
                GameplayFacade.ScoresInterface.ChangeMap(NetworkFacade.MainHandling.Connections.CurrentMap);
            }
        }

        public override void PostUnset()
        {
            //
        }
    }
}
