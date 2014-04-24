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
        string target_weapon;

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
                WeaponState.Reloading)
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
                WeaponState.Switching)
            {
                if (w_switch_timer < Game.ThisPlayer.Inventory.GetSwitchTime(target_weapon))
                {
                    w_switch_timer += dt;
                }
                else
                {
                    Game.ThisPlayer.Inventory.Switch(target_weapon);

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
                Game.ThisPlayer.Inventory.Fire();

                // reloading if no bullet left after shot
                Game.ThisPlayer.Inventory.UpdateAmmo();
            }

            // switching weapons
            if (Game.ThisPlayer.ks.IsKeyDown(Keys.D1) &&
                CanSwitch("Pistol"))
            {
                if (Game.ThisPlayer.Inventory.Contains("Pistol"))
                {
                Game.ThisPlayer.CurrentWeapon.State =
                    WeaponState.Switching;
                target_weapon = "Pistol";
                }
            }
            if (Game.ThisPlayer.ks.IsKeyDown(Keys.D2) &&
                CanSwitch("Assault Rifle"))
            {
                if (Game.ThisPlayer.Inventory.Contains("Assault Rifle"))
                {
                    Game.ThisPlayer.CurrentWeapon.State =
                        WeaponState.Switching;
                    target_weapon = "Assault Rifle";
                }
            }
            if (Game.ThisPlayer.ks.IsKeyDown(Keys.D3) &&
                CanSwitch("Bazooka"))
            {
                if (Game.ThisPlayer.Inventory.Contains("Bazooka"))
                {
                    Game.ThisPlayer.CurrentWeapon.State =
                        WeaponState.Switching;
                    target_weapon = "Bazooka";
                }
            }

            if (Game.ThisPlayer.ks.IsKeyDown(Game.Config.Reload) &&
                CanReload())
            {
                Game.ThisPlayer.CurrentWeapon.State =
                    WeaponState.Reloading;
            }

            if (Game.ThisPlayer.ks.IsKeyDown(Game.Config.QuickSwitch) &&
                CanQuickSwitch())
            {
                Game.ThisPlayer.CurrentWeapon.State =
                    WeaponState.Switching;
                target_weapon = Game.ThisPlayer.PreviousWeapon.Name;
            }

            if (Game.ThisPlayer.CurrentMouseState.RightButton ==
                ButtonState.Pressed && CanAim())
            {
                Game.ThisPlayer.CurrentWeapon.AimState = AimState.True;
            }
            else
            { Game.ThisPlayer.CurrentWeapon.AimState = AimState.False; }
        }

        private bool CanAim()
        {
            return (
                //(Game.ThisPlayer.CurrentWeapon.AimState == AimState.False)
                 (Game.ThisPlayer.CurrentWeapon.State == WeaponState.AtEase)
                && (Game.ThisPlayer.CurrentSpeedState != Player.SpeedState.Sprinting)
                );
        }

        private bool CanFire()
        {
            return ((Game.ThisPlayer.CurrentWeapon.State ==
                WeaponState.AtEase) &&

                (Game.ThisPlayer.CurrentSpeedState !=
                LocalPlayer.SpeedState.Sprinting) &&
                
                (Game.ThisPlayer.CurrentWeapon.FireTimer ==
                Game.ThisPlayer.CurrentWeapon.FiringRate));
        }

        private bool CanReload()
        {
            return (Game.ThisPlayer.CurrentWeapon.IsReloadable() &&

                (Game.ThisPlayer.CurrentWeapon.State ==
                WeaponState.AtEase) &&

                (Game.ThisPlayer.CurrentSpeedState !=
                LocalPlayer.SpeedState.Sprinting));
        }

        private bool CanSwitch(string s)
        {
            return (

                (Game.ThisPlayer.CurrentWeapon.Name != s) &&

                (Game.ThisPlayer.CurrentWeapon.State ==
                WeaponState.AtEase) &&

                (Game.ThisPlayer.CurrentSpeedState !=
                LocalPlayer.SpeedState.Sprinting));
        }

        private bool CanQuickSwitch()
        {
            return (Game.ThisPlayer.CurrentWeapon != null &&

                Game.ThisPlayer.PreviousWeapon != null &&

                CanSwitch(Game.ThisPlayer.PreviousWeapon.Name));
        }

        public void HandleCombat(float dt)
        {
            HandleTimers(dt);
            CheckCombat();
        }
    }
}
