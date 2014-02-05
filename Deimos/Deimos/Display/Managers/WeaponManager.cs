using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    public class WeaponManager
    {
        DeimosGame Game;

        private Dictionary<string, Weapon> WeaponList =
            new Dictionary<string, Weapon>();

        // Constructor
        public WeaponManager(DeimosGame game)
        {
            Game = game;
        }
        
        // Methods
        public void PickupWeapon()
        {
            // We check if player already has the weapon, if not, we give it to him
            if (!WeaponList.ContainsValue(Game.ThisPlayer.PickupWeapon))
            {
               WeaponList.Add(Game.ThisPlayer.PickupWeapon.Name, Game.ThisPlayer.PickupWeapon);

                // if the picked up weapon has priority over current weapon, we equip it
                // Oldschool FTW!
               if (Game.ThisPlayer.CurrentWeapon.Importance < Game.ThisPlayer.PickupWeapon.Importance)
               {
                   SetCurrentWeapon(Game.ThisPlayer.PickupWeapon); // might be an overwriting problem, I trust Manu to tell me if there is
               }
            }
        }

        public void SetCurrentWeapon(Weapon weapon)
        {
            if (!(Game.ThisPlayer.CurrentWeapon == weapon) && (WeaponList.ContainsValue(weapon)))
            {
                Game.ThisPlayer.CurrentWeapon = weapon;
            }
        }

        private void QuickSwitch(Weapon firstWeapon, Weapon secondWeapon)
        {
            if (firstWeapon != secondWeapon)
            {
                Weapon temp = firstWeapon;
                firstWeapon = secondWeapon;
                secondWeapon = temp;
            }

            // Destroy temp maybe? or does visual studio do it by itself?
        }

        // Reloading, cartridges, ...
        public void PickupAmmo(Weapon weapon)
        {
            if (weapon.c_reservoirAmmo < weapon.m_reservoirAmmo)
            {
                if (weapon.ammoPickup + weapon.c_reservoirAmmo >= weapon.m_reservoirAmmo)
                {
                    weapon.c_reservoirAmmo = weapon.m_reservoirAmmo;
                }
                else
                {
                    weapon.c_reservoirAmmo += weapon.ammoPickup;
                }
            }
        }

        public void Reload()
        {
            if (Game.ThisPlayer.CurrentWeapon.c_chamberAmmo < Game.ThisPlayer.CurrentWeapon.m_chamberAmmo)
            {
                uint t = Game.ThisPlayer.CurrentWeapon.m_chamberAmmo - Game.ThisPlayer.CurrentWeapon.c_chamberAmmo;

                if (t > Game.ThisPlayer.CurrentWeapon.c_reservoirAmmo)
                {
                    t = Game.ThisPlayer.CurrentWeapon.c_reservoirAmmo;
                }

                Game.ThisPlayer.CurrentWeapon.c_chamberAmmo += t;
                Game.ThisPlayer.CurrentWeapon.c_reservoirAmmo -= t;
            }
        }

        // this method reloads the weapon automatically if the current chamber ammo is null
        public void UpdateAmmo()
        {
            if (Game.ThisPlayer.CurrentWeapon.c_chamberAmmo == 0)
            {
                Reload();
            }
        }

        // A forced 'add weapon' method for admin purposes
        public void ForceAdd(Weapon weapon)
        {
            WeaponList.Add(weapon.Name, weapon);
        }
    }
}
