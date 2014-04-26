using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Deimos
{
    public enum Type
    {
        Firearm,
        Melee,
        Grenade
    }

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

    public enum ActionOnHit
    {
        Damage,
        Event,
        Heal
    }

    public class WeaponManager
    {
        DeimosGame Game;

        // This is the player's dynamic Weapon Inventory
        private Dictionary<string, Weapon> PlayerInventory =
            new Dictionary<string, Weapon>();
        private char[] Order;
        int max_n_weapons = 5;
        uint n_weapons = 0;
        int c_weapon = 0;

        // Constructor
        public WeaponManager(DeimosGame game)
        {
            Game = game;

            Order = new char[max_n_weapons];
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

                Order[n_weapons] = pickupWeapon.representative;
                n_weapons++;
                Sort();
       
                // if the picked up weapon has priority over current weapon, 
                // we equip it
                // Oldschool FTW!
                // The current weapon may not be set at the beginning.
               if (Game.ThisPlayer.CurrentWeapon == null)
               {
                   SetCurrentWeapon(pickupWeapon.Name);
                   c_weapon = Array.IndexOf(Order, GetRep(pickupWeapon.Name));
               }
               else if (Game.ThisPlayer.CurrentWeapon != null && 
                   Game.ThisPlayer.CurrentWeapon.Importance 
                   < pickupWeapon.Importance)
               {
                   // might be an overwriting problem, I trust Manu to 
                   // tell me if there is
                   SetPreviousWeapon(Game.ThisPlayer.CurrentWeapon.Name);
                   SetCurrentWeapon(pickupWeapon.Name);
                   c_weapon = Array.IndexOf(Order, GetRep(pickupWeapon.Name));
               }
            }
        }

        private string GetName(char c)
        {
            switch (c)
            {
                case 'A':
                    return "Carver";
                case 'B':
                    return "Pistol";
                case 'C':
                    return "Assault Rifle";
                case 'D':
                    return "Arbiter";
                case 'E':
                    return "Bazooka";
                default:
                    return "hands";
            }
        }

        private char GetRep(string name)
        {
            switch (name)
            {
                case "Carver":
                    return 'A';
                case "Pistol":
                    return 'B';
                case "Assault Rifle":
                    return 'C';
                case "Arbiter":
                    return 'D';
                case "Bazooka":
                    return 'E';
                default:
                    return '0';
            }
        }

        private int FirstIndex()
        {
            int i = 0;

            while (i < Order.Length && Order[i] == '\0')
            {
                i++;
            }

            return i;
        }

        public string GetNext()
        {
            if (c_weapon + 1 != max_n_weapons)
            {
                c_weapon++;
                return GetName(Order[c_weapon]);
            }
            else
            {
                c_weapon = FirstIndex();
                return GetName(Order[c_weapon]);
            }
        }

        public string GetPrevious()
        {
            if (c_weapon - 1 != -1 && Order[c_weapon -1] != '\0')
            {
                c_weapon--;
                return GetName(Order[c_weapon]);
            }
            else
            {
                c_weapon = max_n_weapons-  1;
                return GetName(Order[c_weapon]);
            }
        }

        public void Sort()
        {
            Array.Sort(Order);
        }

        public void SetCurrentWeapon(string name)
        {
            if (PlayerInventory.ContainsKey(name))
            {
                Game.ThisPlayer.CurrentWeapon = PlayerInventory[name];
                c_weapon = Array.IndexOf(Order, GetRep(name));
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

            switch (currentWeapon.Type)
            {
                case Type.Firearm:
                    {
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
                    break;

                case Type.Melee:
                    break;

                case Type.Grenade:
                    break;
            }
        
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
            Order[n_weapons] = weapon.representative;
            n_weapons++;

            Sort();
        }

        // Weapon-dropping
        public void DropWeapon()
        {
            if (PlayerInventory.ContainsKey(Game.ThisPlayer.CurrentWeapon.Name))
            {
                PlayerInventory.Remove(Game.ThisPlayer.CurrentWeapon.Name);
                RemoveFromOrder(Game.ThisPlayer.CurrentWeapon.representative);
            }

            Sort();
        }

        public void RemoveFromOrder(char rep)
        {
            if (Order.First<char>((x) => x == rep) != null)
            {
                int i = 0;

                while (Order[i] != rep)
                {
                    i++; ;
                }

                for (int j = i; j < Order.Length - 1; j++)
                {
                    Order[j] = Order[j + 1];
                    i = j;
                }

                Order[i + 1] = '\0';
                n_weapons--;
            }
        }

        public void Switch(string w_name)
        {
            string n_oldweapon = Game.ThisPlayer.CurrentWeapon.Name;

            if (w_name != n_oldweapon && PlayerInventory.ContainsKey(w_name))
            {
                SetCurrentWeapon(w_name);
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
            Order = new char[max_n_weapons];
            n_weapons = 0;
            c_weapon = 0;
        }
    }
}
