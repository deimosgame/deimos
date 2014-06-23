using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Deimos
{
    class Combat
    {
        float w_switch_timer;
        public string target_weapon;
        public bool firesprint = true;

        public Combat()
        {
            w_switch_timer = 0f;
        }

        private void HandleTimers(float dt)
        {
            // let's update firing timer if necessary
            if (GameplayFacade.ThisPlayer.CurrentWeapon.FireTimer <
                GameplayFacade.ThisPlayer.CurrentWeapon.FiringRate)
            {
                GameplayFacade.ThisPlayer.CurrentWeapon.FireTimer += dt;
            }

            // let's increment the reloading timer if reloading
            if (GameplayFacade.ThisPlayer.CurrentWeapon.State ==
                WeaponState.Reloading)
            {
                if (GameplayFacade.ThisPlayer.CurrentWeapon.ReloadTimer <
                    GameplayFacade.ThisPlayer.CurrentWeapon.TimeToReload)
                {
                    GameplayFacade.ThisPlayer.CurrentWeapon.ReloadTimer += dt;
                }
                else
                {
                    GameplayFacade.ThisPlayer.Inventory.Reload();
                    GameplayFacade.ThisPlayerDisplay.ShowWeapon();
                }
            }

              // let's increase weapon switch timer if switching
            if (GameplayFacade.ThisPlayer.CurrentWeapon.State == 
                WeaponState.Switching)
            {
                if (w_switch_timer < GameplayFacade.ThisPlayer.Inventory.GetSwitchTime(target_weapon))
                {
                    w_switch_timer += dt;
                }
                else
                {
                    GameplayFacade.ThisPlayer.Inventory.Switch(target_weapon);
                    GameplayFacade.ThisPlayerDisplay.ShowWeapon();

                    w_switch_timer = 0f;
                }
            }
        }

        private void CheckCombat()
        {
            GameplayFacade.ThisPlayer.ks = Keyboard.GetState();
            GameplayFacade.ThisPlayer.CurrentMouseState = Mouse.GetState();

            if (GameplayFacade.ThisPlayer.CurrentMouseState.LeftButton ==
                ButtonState.Pressed &&
                CanFire())
            {
                if (GameplayFacade.ThisPlayer.CurrentSpeedState ==
                    Player.SpeedState.Sprinting)
                {
                    GameplayFacade.ThisPlayer.CurrentSpeedState = Player.SpeedState.Running;
                    firesprint = false;
                }
                GameplayFacade.ThisPlayer.Inventory.Fire();

                // reloading if no bullet left after shot
                GameplayFacade.ThisPlayer.Inventory.UpdateAmmo();
            }

            if (!firesprint && GameplayFacade.ThisPlayer.ks.IsKeyUp(GeneralFacade.Config.Sprint))
            {
                firesprint = true;
            }

            // mousewheeling for next/previous weapon
            if (GameplayFacade.ThisPlayer.CurrentMouseState.ScrollWheelValue >
                GameplayFacade.ThisPlayer.previousScrollValue &&
                CanSwitch(GameplayFacade.ThisPlayer.Inventory.GetNext()))
            {
                GameplayFacade.ThisPlayer.CurrentWeapon.State =
                    WeaponState.Switching;
                target_weapon = GameplayFacade.ThisPlayer.Inventory.SetNext();
            }
            else if (GameplayFacade.ThisPlayer.CurrentMouseState.ScrollWheelValue <
                GameplayFacade.ThisPlayer.previousScrollValue &&
                CanSwitch(GameplayFacade.ThisPlayer.Inventory.GetPrevious()))
            {
                GameplayFacade.ThisPlayer.CurrentWeapon.State =
                    WeaponState.Switching;
                target_weapon = GameplayFacade.ThisPlayer.Inventory.SetPrevious();
            }

            // switching weapons
            if (GameplayFacade.ThisPlayer.ks.IsKeyDown(Keys.D1) &&
                CanSwitch("Pistol")
                && (!GameplayFacade.ChatInterface.InputChat))
            {
                if (GameplayFacade.ThisPlayer.Inventory.Contains("Pistol"))
                {
                GameplayFacade.ThisPlayer.CurrentWeapon.State =
                    WeaponState.Switching;
                target_weapon = "Pistol";
                }
            }
            if (GameplayFacade.ThisPlayer.ks.IsKeyDown(Keys.D2) &&
                CanSwitch("Assault Rifle")
                && (!GameplayFacade.ChatInterface.InputChat))
            {
                if (GameplayFacade.ThisPlayer.Inventory.Contains("Assault Rifle"))
                {
                    GameplayFacade.ThisPlayer.CurrentWeapon.State =
                        WeaponState.Switching;
                    target_weapon = "Assault Rifle";
                }
            }
            if (GameplayFacade.ThisPlayer.ks.IsKeyDown(Keys.D3) &&
                CanSwitch("Bazooka")
                && (!GameplayFacade.ChatInterface.InputChat))
            {
                if (GameplayFacade.ThisPlayer.Inventory.Contains("Bazooka"))
                {
                    GameplayFacade.ThisPlayer.CurrentWeapon.State =
                        WeaponState.Switching;
                    target_weapon = "Bazooka";
                }
            }
            if (GameplayFacade.ThisPlayer.ks.IsKeyDown(Keys.D4) &&
                CanSwitch("Arbiter")
                && (!GameplayFacade.ChatInterface.InputChat))
            {
                if (GameplayFacade.ThisPlayer.Inventory.Contains("Arbiter"))
                {
                    GameplayFacade.ThisPlayer.CurrentWeapon.State =
                        WeaponState.Switching;
                    target_weapon = "Arbiter";
                }
            }
            if (GameplayFacade.ThisPlayer.ks.IsKeyDown(Keys.D5) &&
                CanSwitch("Carver")
                && (!GameplayFacade.ChatInterface.InputChat))
            {
                if (GameplayFacade.ThisPlayer.Inventory.Contains("Carver"))
                {
                    GameplayFacade.ThisPlayer.CurrentWeapon.State =
                        WeaponState.Switching;
                    target_weapon = "Carver";
                }
            }
            if (GameplayFacade.ThisPlayer.ks.IsKeyDown(Keys.T) &&
                CanSwitch("Essence of Phobia")
                && (!GameplayFacade.ChatInterface.InputChat))
            {
                if (GameplayFacade.ThisPlayer.Inventory.Contains("Essence of Phobia"))
                {
                    GameplayFacade.ThisPlayer.CurrentWeapon.State =
                        WeaponState.Switching;
                    target_weapon = "Essence of Phobia";
                }
            }

            if (GameplayFacade.ThisPlayer.ks.IsKeyDown(GeneralFacade.Config.Reload) &&
                CanReload()
                && (!GameplayFacade.ChatInterface.InputChat))
            {
                GameplayFacade.ThisPlayer.CurrentWeapon.State =
                    WeaponState.Reloading;

                GeneralFacade.SceneManager.SoundManager.Play("w_c");
                if (!NetworkFacade.Local)
                {
                    NetworkFacade.MainHandling.Sounds.SendWithPos(GeneralFacade.SceneManager.SoundManager.GetSoundByte("w_c"),
                        GameplayFacade.ThisPlayer.Position);
                }
            }

            if (GameplayFacade.ThisPlayer.ks.IsKeyDown(GeneralFacade.Config.QuickSwitch) &&
                CanQuickSwitch()
                && (!GameplayFacade.ChatInterface.InputChat))
            {
                GameplayFacade.ThisPlayer.CurrentWeapon.State =
                    WeaponState.Switching;
                target_weapon = GameplayFacade.ThisPlayer.PreviousWeapon.Name;
            }

            if (GameplayFacade.ThisPlayer.CurrentMouseState.RightButton ==
                ButtonState.Pressed && CanAim())
            {
                GameplayFacade.ThisPlayer.CurrentWeapon.AimState = AimState.True;
            }
            else
            { GameplayFacade.ThisPlayer.CurrentWeapon.AimState = AimState.False; }
        }

        private bool CanAim()
        {
            return (
                //(GameplayFacade.ThisPlayer.CurrentWeapon.AimState == AimState.False)
                 (GameplayFacade.ThisPlayer.CurrentWeapon.State == WeaponState.AtEase)
                && (GameplayFacade.ThisPlayer.CurrentSpeedState != Player.SpeedState.Sprinting)
                );
        }

        private bool CanFire()
        {
            return ((GameplayFacade.ThisPlayer.CurrentWeapon.State ==
                WeaponState.AtEase) &&
                
                (GameplayFacade.ThisPlayer.CurrentWeapon.FireTimer ==
                GameplayFacade.ThisPlayer.CurrentWeapon.FiringRate));
        }

        private bool CanReload()
        {
            return (GameplayFacade.ThisPlayer.CurrentWeapon.IsReloadable() &&

                (GameplayFacade.ThisPlayer.CurrentWeapon.State ==
                WeaponState.AtEase) &&

                (GameplayFacade.ThisPlayer.CurrentSpeedState !=
                LocalPlayer.SpeedState.Sprinting));
        }

        private bool CanSwitch(string s)
        {
            return (

                (GameplayFacade.ThisPlayer.CurrentWeapon.Name != s) &&

                (GameplayFacade.ThisPlayer.CurrentWeapon.State ==
                WeaponState.AtEase) &&

                (GameplayFacade.ThisPlayer.CurrentSpeedState !=
                LocalPlayer.SpeedState.Sprinting));
        }

        private bool CanQuickSwitch()
        {
            return (GameplayFacade.ThisPlayer.CurrentWeapon != null &&

                GameplayFacade.ThisPlayer.PreviousWeapon != null &&

                CanSwitch(GameplayFacade.ThisPlayer.PreviousWeapon.Name));
        }

        public void HandleCombat(float dt)
        {
            HandleTimers(dt);
            CheckCombat();
        }
    }
}
