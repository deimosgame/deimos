using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Deimos
{
    class LocalPlayerCombat
    {
        DeimosGame Game;
        float w_switch_timer;

        public LocalPlayerCombat(DeimosGame game)
        {
            Game = game;
            w_switch_timer = 0f;
        }

        private void HandleTimers(float dt)
        {
            // let's update firing timer if necessary
            if (Game.ThisPlayer.CurrentWeapon.FireTimer <
                Game.ThisPlayer.CurrentWeapon.FiringRate)
            {
                Game.ThisPlayer.CurrentWeapon.FireTimer += dt;
            }

            // let's increment the reloading timer if reloading
            if (Game.ThisPlayer.CurrentWeapon.State ==
                Weapon.WeaponState.Reloading)
            {
                if (Game.ThisPlayer.CurrentWeapon.ReloadTimer <
                    Game.ThisPlayer.CurrentWeapon.TimeToReload)
                {
                    Game.ThisPlayer.CurrentWeapon.ReloadTimer += dt;
                }
                else
                {
                    Game.ThisPlayer.Inventory.Reload();
                }
            }

              // let's increase weapon switch timer if switching
            if (Game.ThisPlayer.CurrentWeapon.State == 
                Weapon.WeaponState.Switching)
            {
                if (w_switch_timer < Game.ThisPlayer.Inventory.GetSwitchTime(Game.ThisPlayer.PreviousWeapon))
                {
                    w_switch_timer += dt;
                }
                else
                {
                    Game.ThisPlayer.Inventory.QuickSwitch(
                        Game.ThisPlayer.CurrentWeapon,
                        Game.ThisPlayer.PreviousWeapon);
                    w_switch_timer = 0f;
                }
            }
        }

        private void CheckCombat()
        {
            Game.ThisPlayer.ks = Keyboard.GetState();
            Game.ThisPlayer.CurrentMouseState = Mouse.GetState();

            if (Game.ThisPlayer.CurrentMouseState.LeftButton ==
                ButtonState.Pressed &&
                CanFire())
            {
                Game.ThisPlayer.CurrentWeapon.Fire();

                // reloading if no bullet left after shot
                Game.ThisPlayer.Inventory.UpdateAmmo();
            }

            if (Game.ThisPlayer.ks.IsKeyDown(Game.Config.Reload) &&
                CanReload())
            {
                Game.ThisPlayer.CurrentWeapon.State =
                    Weapon.WeaponState.Reloading;
            }

            if (Game.ThisPlayer.ks.IsKeyDown(Game.Config.QuickSwitch) &&
                CanQuickSwitch())
            {
                Game.ThisPlayer.CurrentWeapon.State =
                    Weapon.WeaponState.Switching;
            }
        }

        private bool CanFire()
        {
            return ((Game.ThisPlayer.CurrentWeapon.State ==
                Weapon.WeaponState.AtEase) &&

                (Game.ThisPlayer.CurrentSpeedState !=
                LocalPlayer.SpeedState.Sprinting) &&
                
                (Game.ThisPlayer.CurrentWeapon.FireTimer ==
                Game.ThisPlayer.CurrentWeapon.FiringRate));
        }

        private bool CanReload()
        {
            return (Game.ThisPlayer.CurrentWeapon.IsReloadable() &&

                (Game.ThisPlayer.CurrentWeapon.State ==
                Weapon.WeaponState.AtEase) &&

                (Game.ThisPlayer.CurrentSpeedState !=
                LocalPlayer.SpeedState.Sprinting));
        }

        private bool CanQuickSwitch()
        {
            return (Game.ThisPlayer.CurrentWeapon != null &&

                Game.ThisPlayer.PreviousWeapon != null &&

                (Game.ThisPlayer.CurrentWeapon.State ==
                Weapon.WeaponState.AtEase) &&

                (Game.ThisPlayer.CurrentSpeedState !=
                LocalPlayer.SpeedState.Sprinting));
        }

        public void HandleCombat(float dt)
        {
            HandleTimers(dt);
            CheckCombat();
        }
    }
}
