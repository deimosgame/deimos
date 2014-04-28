using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Deimos
{
    class LocalPlayerCombat
    {
        float w_switch_timer;
        public string target_weapon;
        public bool firesprint = true;

        public LocalPlayerCombat()
        {
            w_switch_timer = 0f;
        }

        private void HandleTimers(float dt)
        {
            // let's update firing timer if necessary
            if (GeneralFacade.Game.ThisPlayer.CurrentWeapon.FireTimer <
                GeneralFacade.Game.ThisPlayer.CurrentWeapon.FiringRate)
            {
                GeneralFacade.Game.ThisPlayer.CurrentWeapon.FireTimer += dt;
            }

            // let's increment the reloading timer if reloading
            if (GeneralFacade.Game.ThisPlayer.CurrentWeapon.State ==
                WeaponState.Reloading)
            {
                if (GeneralFacade.Game.ThisPlayer.CurrentWeapon.ReloadTimer <
                    GeneralFacade.Game.ThisPlayer.CurrentWeapon.TimeToReload)
                {
                    GeneralFacade.Game.ThisPlayer.CurrentWeapon.ReloadTimer += dt;
                }
                else
                {
                    GeneralFacade.Game.ThisPlayer.Inventory.Reload();
                    GeneralFacade.Game.ThisPlayerDisplay.ShowWeapon();
                }
            }

              // let's increase weapon switch timer if switching
            if (GeneralFacade.Game.ThisPlayer.CurrentWeapon.State == 
                WeaponState.Switching)
            {
                if (w_switch_timer < GeneralFacade.Game.ThisPlayer.Inventory.GetSwitchTime(target_weapon))
                {
                    w_switch_timer += dt;
                }
                else
                {
                    GeneralFacade.Game.ThisPlayer.Inventory.Switch(target_weapon);
                    GeneralFacade.Game.ThisPlayerDisplay.ShowWeapon();

                    w_switch_timer = 0f;
                }
            }
        }

        private void CheckCombat()
        {
            GeneralFacade.Game.ThisPlayer.ks = Keyboard.GetState();
            GeneralFacade.Game.ThisPlayer.CurrentMouseState = Mouse.GetState();

            if (GeneralFacade.Game.ThisPlayer.CurrentMouseState.LeftButton ==
                ButtonState.Pressed &&
                CanFire())
            {
                if (GeneralFacade.Game.ThisPlayer.CurrentSpeedState ==
                    Player.SpeedState.Sprinting)
                {
                    GeneralFacade.Game.ThisPlayer.CurrentSpeedState = Player.SpeedState.Running;
                    firesprint = false;
                }
                GeneralFacade.Game.ThisPlayer.Inventory.Fire();

                // reloading if no bullet left after shot
                GeneralFacade.Game.ThisPlayer.Inventory.UpdateAmmo();
            }

            if (!firesprint && GeneralFacade.Game.ThisPlayer.ks.IsKeyUp(GeneralFacade.Game.Config.Sprint))
            {
                firesprint = true;
            }

            // mousewheeling for next/previous weapon
            if (GeneralFacade.Game.ThisPlayer.CurrentMouseState.ScrollWheelValue >
                GeneralFacade.Game.ThisPlayer.previousScrollValue &&
                CanSwitch(GeneralFacade.Game.ThisPlayer.Inventory.GetNext()))
            {
                GeneralFacade.Game.ThisPlayer.CurrentWeapon.State =
                    WeaponState.Switching;
                target_weapon = GeneralFacade.Game.ThisPlayer.Inventory.SetNext();
            }
            else if (GeneralFacade.Game.ThisPlayer.CurrentMouseState.ScrollWheelValue <
                GeneralFacade.Game.ThisPlayer.previousScrollValue &&
                CanSwitch(GeneralFacade.Game.ThisPlayer.Inventory.GetPrevious()))
            {
                GeneralFacade.Game.ThisPlayer.CurrentWeapon.State =
                    WeaponState.Switching;
                target_weapon = GeneralFacade.Game.ThisPlayer.Inventory.SetPrevious();
            }

            // switching weapons
            if (GeneralFacade.Game.ThisPlayer.ks.IsKeyDown(Keys.D1) &&
                CanSwitch("Pistol"))
            {
                if (GeneralFacade.Game.ThisPlayer.Inventory.Contains("Pistol"))
                {
                GeneralFacade.Game.ThisPlayer.CurrentWeapon.State =
                    WeaponState.Switching;
                target_weapon = "Pistol";
                }
            }
            if (GeneralFacade.Game.ThisPlayer.ks.IsKeyDown(Keys.D2) &&
                CanSwitch("Assault Rifle"))
            {
                if (GeneralFacade.Game.ThisPlayer.Inventory.Contains("Assault Rifle"))
                {
                    GeneralFacade.Game.ThisPlayer.CurrentWeapon.State =
                        WeaponState.Switching;
                    target_weapon = "Assault Rifle";
                }
            }
            if (GeneralFacade.Game.ThisPlayer.ks.IsKeyDown(Keys.D3) &&
                CanSwitch("Bazooka"))
            {
                if (GeneralFacade.Game.ThisPlayer.Inventory.Contains("Bazooka"))
                {
                    GeneralFacade.Game.ThisPlayer.CurrentWeapon.State =
                        WeaponState.Switching;
                    target_weapon = "Bazooka";
                }
            }
            if (GeneralFacade.Game.ThisPlayer.ks.IsKeyDown(Keys.D4) &&
                CanSwitch("Arbiter"))
            {
                if (GeneralFacade.Game.ThisPlayer.Inventory.Contains("Arbiter"))
                {
                    GeneralFacade.Game.ThisPlayer.CurrentWeapon.State =
                        WeaponState.Switching;
                    target_weapon = "Arbiter";
                }
            }
            if (GeneralFacade.Game.ThisPlayer.ks.IsKeyDown(Keys.D5) &&
                CanSwitch("Carver"))
            {
                if (GeneralFacade.Game.ThisPlayer.Inventory.Contains("Carver"))
                {
                    GeneralFacade.Game.ThisPlayer.CurrentWeapon.State =
                        WeaponState.Switching;
                    target_weapon = "Carver";
                }
            }
            if (GeneralFacade.Game.ThisPlayer.ks.IsKeyDown(Keys.T) &&
                CanSwitch("Essence of Phobia"))
            {
                if (GeneralFacade.Game.ThisPlayer.Inventory.Contains("Essence of Phobia"))
                {
                    GeneralFacade.Game.ThisPlayer.CurrentWeapon.State =
                        WeaponState.Switching;
                    target_weapon = "Essence of Phobia";
                }
            }

            if (GeneralFacade.Game.ThisPlayer.ks.IsKeyDown(GeneralFacade.Game.Config.Reload) &&
                CanReload())
            {
                GeneralFacade.Game.ThisPlayer.CurrentWeapon.State =
                    WeaponState.Reloading;
            }

            if (GeneralFacade.Game.ThisPlayer.ks.IsKeyDown(GeneralFacade.Game.Config.QuickSwitch) &&
                CanQuickSwitch())
            {
                GeneralFacade.Game.ThisPlayer.CurrentWeapon.State =
                    WeaponState.Switching;
                target_weapon = GeneralFacade.Game.ThisPlayer.PreviousWeapon.Name;
            }

            if (GeneralFacade.Game.ThisPlayer.CurrentMouseState.RightButton ==
                ButtonState.Pressed && CanAim())
            {
                GeneralFacade.Game.ThisPlayer.CurrentWeapon.AimState = AimState.True;
            }
            else
            { GeneralFacade.Game.ThisPlayer.CurrentWeapon.AimState = AimState.False; }
        }

        private bool CanAim()
        {
            return (
                //(GeneralFacade.Game.ThisPlayer.CurrentWeapon.AimState == AimState.False)
                 (GeneralFacade.Game.ThisPlayer.CurrentWeapon.State == WeaponState.AtEase)
                && (GeneralFacade.Game.ThisPlayer.CurrentSpeedState != Player.SpeedState.Sprinting)
                );
        }

        private bool CanFire()
        {
            return ((GeneralFacade.Game.ThisPlayer.CurrentWeapon.State ==
                WeaponState.AtEase) &&
                
                (GeneralFacade.Game.ThisPlayer.CurrentWeapon.FireTimer ==
                GeneralFacade.Game.ThisPlayer.CurrentWeapon.FiringRate));
        }

        private bool CanReload()
        {
            return (GeneralFacade.Game.ThisPlayer.CurrentWeapon.IsReloadable() &&

                (GeneralFacade.Game.ThisPlayer.CurrentWeapon.State ==
                WeaponState.AtEase) &&

                (GeneralFacade.Game.ThisPlayer.CurrentSpeedState !=
                LocalPlayer.SpeedState.Sprinting));
        }

        private bool CanSwitch(string s)
        {
            return (

                (GeneralFacade.Game.ThisPlayer.CurrentWeapon.Name != s) &&

                (GeneralFacade.Game.ThisPlayer.CurrentWeapon.State ==
                WeaponState.AtEase) &&

                (GeneralFacade.Game.ThisPlayer.CurrentSpeedState !=
                LocalPlayer.SpeedState.Sprinting));
        }

        private bool CanQuickSwitch()
        {
            return (GeneralFacade.Game.ThisPlayer.CurrentWeapon != null &&

                GeneralFacade.Game.ThisPlayer.PreviousWeapon != null &&

                CanSwitch(GeneralFacade.Game.ThisPlayer.PreviousWeapon.Name));
        }

        public void HandleCombat(float dt)
        {
            HandleTimers(dt);
            CheckCombat();
        }
    }
}
