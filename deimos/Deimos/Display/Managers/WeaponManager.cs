using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Deimos
{
    public enum WeaponState
    {
        AtEase,
        Firing,
        Reloading,
        Switching,
        Aiming
    }

    public class WeaponManager
    {
        DeimosGame Game;

        // This is the player's dynamic Weapon Inventory
        private Dictionary<string, Weapon> PlayerInventory =
            new Dictionary<string, Weapon>();

        // Constructor
        public WeaponManager(DeimosGame game)
        {
            Game = game;
        }
        
        // Methods for the Player Inventory
        public void PickupWeapon(Weapon pickupWeapon)
        {
            // We check if player already has the weapon, if not, we give 
            // it to him
            if (!PlayerInventory.ContainsValue(pickupWeapon))
            {
               PlayerInventory.Add(
                   pickupWeapon.Name, 
                   pickupWeapon
               );
       
                // if the picked up weapon has priority over current weapon, 
                // we equip it
                // Oldschool FTW!
                // The current weapon may not be set at the beginning.
               if (Game.ThisPlayer.CurrentWeapon != null && 
                   Game.ThisPlayer.CurrentWeapon.Importance 
                   < pickupWeapon.Importance)
               {
                   // might be an overwriting problem, I trust Manu to 
                   // tell me if there is
                   SetCurrentWeapon(pickupWeapon.Name); 
               }
            }
        }

        public void SetCurrentWeapon(string name)
        {
            if (PlayerInventory.ContainsKey(name))
            {
                Game.ThisPlayer.CurrentWeapon = PlayerInventory[name];
            }
        }

        public void SetPreviousWeapon(string name)
        {
            if (!(Game.ThisPlayer.PreviousWeapon == 
                PlayerInventory[name])
                && (PlayerInventory.ContainsKey(name)))
            {
                Game.ThisPlayer.PreviousWeapon = PlayerInventory[name];
            }
        }

        public float GetSwitchTime(Weapon weapon)
        {
            float time = weapon.TimeToReload / 5f;

            return time;
        }

        public void QuickSwitch(Weapon firstWeapon, Weapon secondWeapon)
        {
            if (firstWeapon != secondWeapon)
            {
                Weapon temp = firstWeapon;
                firstWeapon = secondWeapon;
                secondWeapon = temp;

                SetCurrentWeapon(firstWeapon.Name);
                SetPreviousWeapon(secondWeapon.Name);
            }

            Game.ThisPlayer.CurrentWeapon.State = WeaponState.AtEase;

            // Destroy temp maybe? or does visual studio do it by itself?
        }

        // Reloading, cartridges, ...
        public void PickupAmmo(Weapon weapon)
        {
            if (weapon.c_reservoirAmmo < weapon.m_reservoirAmmo)
            {
                if (Game.ThisPlayer.ammoPickup + weapon.c_reservoirAmmo 
                    >= weapon.m_reservoirAmmo)
                {
                    weapon.c_reservoirAmmo = weapon.m_reservoirAmmo;
                }
                else
                {
                    weapon.c_reservoirAmmo += Game.ThisPlayer.ammoPickup;
                }
            }
        }

        public void Fire()
        {
            Weapon currentWeapon = Game.ThisPlayer.CurrentWeapon;
            if (!currentWeapon.IsFirable())
            {
                return;
            }
            currentWeapon.State = WeaponState.Firing;
            // Putting projectile in action through Bullet Manager
            // These methods are not final at all, they WILL be changed.
            Game.BulletManager.SpawnBullet();
            Game.SceneManager.SoundManager.Play("weaponFire");
            currentWeapon.c_chamberAmmo--;
            currentWeapon.FireTimer = 0; // reset the fire timer
            currentWeapon.State = WeaponState.AtEase;
        }

        public void Reload()
        {
            uint t = Game.ThisPlayer.CurrentWeapon.m_chamberAmmo -
                Game.ThisPlayer.CurrentWeapon.c_chamberAmmo;

            if (t > Game.ThisPlayer.CurrentWeapon.c_reservoirAmmo)
            {
                t = Game.ThisPlayer.CurrentWeapon.c_reservoirAmmo;
            }

            Game.ThisPlayer.CurrentWeapon.c_chamberAmmo += t;
            Game.ThisPlayer.CurrentWeapon.c_reservoirAmmo -= t;
                

            Game.ThisPlayer.CurrentWeapon.ReloadTimer = 0;
            Game.ThisPlayer.CurrentWeapon.State = WeaponState.AtEase;
        }

        // this method reloads the weapon automatically if the current chamber 
        // ammo is null
        public void UpdateAmmo()
        {
            if (!Game.ThisPlayer.CurrentWeapon.HasAmmo() &&
                            Game.ThisPlayer.CurrentWeapon.IsReloadable())
            {
                Game.ThisPlayer.CurrentWeapon.State =
                    WeaponState.Reloading;
            }
        }

        // A forced 'add weapon' method for admin purposes
        public void ForceAdd(Weapon weapon)
        {
            PlayerInventory.Add(weapon.Name, weapon);
        }
    }
}
