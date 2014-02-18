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

        public LocalPlayer(DeimosGame game)
        {
            Game = game;

            PlayerMovement = new LocalPlayerMovement(game);
            PlayerActions = new LocalPlayerActions(game);
            PlayerCombat = new LocalPlayerCombat(game);
        }

        public void InitializeInventory()
        {
            Inventory = new WeaponManager(Game);

            Inventory.PickupWeapon(Game.Weapons.GetWeapon("Assault Rifle"));
            Inventory.SetCurrentWeapon("Assault Rifle");

            ammoPickup = CurrentWeapon.m_reservoirAmmo;
            Inventory.PickupAmmo(CurrentWeapon);

            Game.ThisPlayerDisplay.LoadCurrentWeaponModel();
        }

        private void Stuff(float dt)
        {
            // Jump
            //if (ks.IsKeyDown(Game.Config.Jump))
            //{
            //    if (Game.CurrentPlayingState == DeimosGame.PlayingStates.NoClip)
            //    {
            //        moveVector.Y = 1;
            //    }

            //    else
            //    {
            //        if (Game.ThisPlayerPhysics.State ==
            //        LocalPlayerPhysics.PhysicalState.Walking)
            //        {
            //            if (Game.ThisPlayer.CurrentSpeedState == SpeedState.Sprinting)
            //            {
            //                Game.ThisPlayer.CurrentSpeedState = SpeedState.Running;
            //            }
            //            Game.ThisPlayerPhysics.InitiateJump(4.3f);
            //        }
            //    }
            //}

            

            // Testing purposes: picking up ammo
            if (ks.IsKeyDown(Keys.O))
            {
                Game.ThisPlayer.ammoPickup = 10;
                Game.ThisPlayer.Inventory.PickupAmmo(Game.ThisPlayer.CurrentWeapon);

                Game.ThisPlayer.Inventory.UpdateAmmo();
            }

            //Game.Config.DebugScreen = true;

            if (ks.IsKeyDown(Game.Config.ShowDebug))
            {
                Game.Config.DebugScreen = true;
            }

            //if (Game.CurrentPlayingState != DeimosGame.PlayingStates.NoClip)
            //{
            //    moveVector.Y = Game.ThisPlayerPhysics.ApplyGravity(dt);
            //}

        }

        
        
        public void HandleInput(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f;

            PlayerMovement.HandleMovement(dt);
            PlayerActions.HandleActions(dt);
            PlayerCombat.HandleCombat(dt);
            Stuff(dt);
            Game.ThisPlayerDisplay.DisplayCurrentWeapon(Weapon.WeaponState.AtEase);
       }
   }
}
