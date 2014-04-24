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
        Switching
    }

    public enum AimState
    {
        False,
        True
    }

    public class WeaponManager
    {
        DeimosGame Game;

        // This is the player's dynamic Weapon Inventory
        private Dictionary<string, Weapon> PlayerInventory =
            new Dictionary<string, Weapon>();
        private string[] Order = new string[5];
        uint b_weapons = 0;

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

                Order[b_weapons] = pickupWeapon.Name;
                b_weapons++;
       
                // if the picked up weapon has priority over current weapon, 
                // we equip it
                // Oldschool FTW!
                // The current weapon may not be set at the beginning.
               if (Game.ThisPlayer.CurrentWeapon == null)
               {
                   SetCurrentWeapon(pickupWeapon.Name);
               }
               else if (Game.ThisPlayer.CurrentWeapon != null && 
                   Game.ThisPlayer.CurrentWeapon.Importance 
                   < pickupWeapon.Importance)
               {
                   // might be an overwriting problem, I trust Manu to 
                   // tell me if there is
                   SetPreviousWeapon(Game.ThisPlayer.CurrentWeapon.Name);
                   SetCurrentWeapon(pickupWeapon.Name);
               }
               Sort();
            }
        }

        public void Sort()
        {
            
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
            if ((PlayerInventory.ContainsKey(name))
                 && !(Game.ThisPlayer.PreviousWeapon == 
                PlayerInventory[name]))
            {
                Game.ThisPlayer.PreviousWeapon = PlayerInventory[name];
            }
        }

        public float GetSwitchTime(string w_name)
        {
            if (Game.ThisPlayer.Inventory.Contains(w_name))
            {
                return PlayerInventory[w_name].TimeToReload / 5f;
            }
            else
            {
                return 0;
            }
        }

        public void QuickSwitch(string firstWeapon, string secondWeapon)
        {
            if (firstWeapon != secondWeapon)
            {
                string temp = firstWeapon;
                firstWeapon = secondWeapon;
                secondWeapon = temp;

                SetCurrentWeapon(firstWeapon);
                SetPreviousWeapon(secondWeapon);
                Game.ThisPlayerDisplay.SetCurrentWeaponModel();
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
            Order[b_weapons] = weapon.Name;
            b_weapons++;

            Sort();
        }

        // Weapon-dropping
        public void DropWeapon()
        {
            if (PlayerInventory.ContainsKey(Game.ThisPlayer.CurrentWeapon.Name))
            {
                PlayerInventory.Remove(Game.ThisPlayer.CurrentWeapon.Name);
                RemoveFromOrder(Game.ThisPlayer.CurrentWeapon.Name);
            }

            Sort();
        }

        public void RemoveFromOrder(string name)
        {
            if (Order.First<string>((x) => x == name) != null)
            {
                int i = 0;

                while (Order[i] != name)
                {
                    i++; ;
                }

                for (int j = i; j < Order.Length - 1; j++)
                {
                    Order[j] = Order[j + 1];
                    i = j;
                }

                Order[i + 1] = null;
                b_weapons--;
            }
        }

        public void Switch(string w_name)
        {
            string n_weapon = w_name;

            //switch (w_name)
            //{
            //    case "Pistol" :
            //        n_weapon = Order[0];
            //        break;

            //    case "Assault Rifle" :
            //        n_weapon = Order[1];
            //        break;
                
            //    case "Bazooka" :
            //        n_weapon = Order[2];
            //        break;

            //    default:
            //        break;
            //}

            string n_oldweapon = Game.ThisPlayer.CurrentWeapon.Name;

            if (n_weapon != n_oldweapon && PlayerInventory.ContainsKey(n_weapon))
            {
                SetCurrentWeapon(n_weapon);
                SetPreviousWeapon(n_oldweapon);
                Game.ThisPlayerDisplay.SetCurrentWeaponModel();
            }

            Game.ThisPlayer.CurrentWeapon.State = WeaponState.AtEase;
        }

        public bool Contains(string name)
        {
            return PlayerInventory.ContainsKey(name);
        }

        public void Flush()
        {
            PlayerInventory = new Dictionary<string, Weapon>();
            Order = new string[5];
            b_weapons = 0;
        }
    }
}
