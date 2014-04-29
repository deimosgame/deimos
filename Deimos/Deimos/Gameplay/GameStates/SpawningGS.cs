using Microsoft.Xna.Framework;
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
        public override GameStates GameState
        {
            get { return GameStates.Spawning; }
        }

        public override void PreSet()
        {
            GameplayFacade.Weapons = new WeaponsList();
            GameplayFacade.BulletManager = new BulletManager();
            GameplayFacade.Objects = new ObjectsList();

            GameplayFacade.Weapons.Initialise();
            GameplayFacade.Objects.Initialize();

            GameplayFacade.ThisPlayer = new LocalPlayer();
            GameplayFacade.ThisPlayerPhysics = new Physics();
            GameplayFacade.ThisPlayerDisplay = new Display();


            GeneralFacade.SceneManager.SetScene<SceneDeimos>();


            GameplayFacade.ThisPlayer.Inventory = new WeaponManager();
            GameplayFacade.ThisPlayer.InitializeInventory(GameplayFacade.ThisPlayer.Class);
            GameplayFacade.ThisPlayer.PlayerSpawn(new Vector3(-60f, 20f, -8f), Vector3.Zero);
        }

        public override void PostUnset()
        {
            //
        }
    }
}
