using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Deimos
{
    public class LocalPlayer : Player
    {
        DeimosGame Game;

        public WeaponManager Inventory;

        public MouseState CurrentMouseState;
        public MouseState PreviousMouseState;
        public Vector3 MouseRotationBuffer;

        public Vector3 CameraOldPosition;

        public KeyboardState ks;

        LocalPlayerActions PlayerActions;
        LocalPlayerMovement PlayerMovement;
        LocalPlayerCombat PlayerCombat;

        public float dt;

        public LocalPlayer(DeimosGame game)
        {
            Game = game;

            PlayerMovement = new LocalPlayerMovement(game);
            PlayerActions = new LocalPlayerActions(game);
            PlayerCombat = new LocalPlayerCombat(game);

            Name = Game.Config.PlayerName;
            Instance = "main";
        }

        public void InitializeInventory(Player.Spec spec)
        {
            Inventory = new WeaponManager(Game);
            Game.ThisPlayer.CurrentWeapon = null;
            Game.ThisPlayer.PreviousWeapon = null;

            switch (spec)
            {
                case Spec.Soldier:
                    {
                        Inventory.PickupWeapon(Game.Weapons.GetWeapon("Pistol"));
                        Inventory.PickupWeapon(Game.Weapons.GetWeapon("Assault Rifle"));
                        Inventory.PickupWeapon(Game.Weapons.GetWeapon("Bazooka"));
                    }
                    break;

                case Spec.Agent:
                    {
                        Inventory.PickupWeapon(Game.Weapons.GetWeapon("Pistol"));
                        Inventory.PickupWeapon(Game.Weapons.GetWeapon("Assault Rifle"));
                    }
                    break;

                case Spec.Overwatch:
                    {
                        Inventory.PickupWeapon(Game.Weapons.GetWeapon("Assault Rifle"));
                        Inventory.PickupWeapon(Game.Weapons.GetWeapon("Bazooka"));
                    }
                    break;

                default:
                    break;
            }

            ammoPickup = CurrentWeapon.m_reservoirAmmo;
            Inventory.PickupAmmo(CurrentWeapon);

            Game.ThisPlayerDisplay.LoadCurrentWeaponModel();
        }

        private void Stuff(float dt)
        {
            // Testing purposes: picking up ammo
            if (ks.IsKeyDown(Keys.O))
            {
                Game.ThisPlayer.ammoPickup = 10;
                Game.ThisPlayer.Inventory.PickupAmmo(Game.ThisPlayer.CurrentWeapon);

                Game.ThisPlayer.Inventory.UpdateAmmo();
            }

            if (ks.IsKeyDown(Game.Config.ShowDebug))
            {
                Game.Config.DebugScreen = !Game.Config.DebugScreen;
            }

        }

        
        
        public void HandleInput(GameTime gameTime)
        {
            dt = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f;

            PlayerMovement.HandleMovement(dt);
            PlayerActions.HandleActions(dt);
            PlayerCombat.HandleCombat(dt);
            Stuff(dt);
            Game.ThisPlayerDisplay.DisplayCurrentWeapon(Game.ThisPlayer.CurrentWeapon);
       }
   }
}
