using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Deimos
{
    public class Weapon
    {
        DeimosGame Game;
        BulletManager bulletManager;

        // Attributes for proper display
        public Vector3 W_Offset;
        public float W_Scaling;

        public string Path;

        public enum WeaponState
        {
            AtEase,
            Firing,
            Reloading,
            Switching,
            Aiming
        }

        private WeaponState state = WeaponState.AtEase;
        public WeaponState State
        {
            get { return state; }
            set { state = value; }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set { name = Name; }
        }

        private int importance;
        public int Importance
        {
            get { return importance; }
            set { importance = value; }
        }

        private float firingRate;
        public float FiringRate
        {
            get { return firingRate; }
            set { firingRate = value; }
        }

        public float fireTimer;

        public uint c_chamberAmmo; // current ammo in the chamber
        public uint m_chamberAmmo; // maximum chamber ammo
        public uint c_reservoirAmmo; // current ammo in the reservoir
        public uint m_reservoirAmmo; // maximum reservoir ammo
        //public uint ammoPickup; // amount of ammo that is potentially picked up

        public float reloadTimer = 0; // timer used for reload
        public float timeToReload; // time needed to reload

        public float timeToSwitch = 0f;

        public bool reloadable = false;

        public float minDmg;
        public float maxDmg;    

        private float projectileSpeed;
        public float ProjectileSpeed
        {
            get { return projectileSpeed; }
            set { projectileSpeed = value; }
        }

        public float Range
        {
            get;
            private set;
        }

        // Constructor
        public Weapon(DeimosGame game, Vector3 w_offset, float w_scaling,
            string path, string w_name, int w_priority, 
            float w_firingRate, uint w_mca, uint w_mra, 
            uint w_initialReservoirAmmo, float w_reloadt, float w_minDmg, float w_maxDmg, 
            float b_speed, float w_range)
        {
            Game = game;
            bulletManager = game.BulletManager;

            W_Offset = w_offset;
            W_Scaling = w_scaling;

            Path = path;

            name = w_name;
            Importance = w_priority;
            FiringRate = w_firingRate;
            fireTimer = FiringRate;
            m_chamberAmmo = w_mca;
            // to start out with a full chamber when weapon spawns
            c_chamberAmmo = m_chamberAmmo; 
            m_reservoirAmmo = w_mra;
            timeToReload = w_reloadt;
            //// if we want to give extra ammo to the player right from the pickup, we can
            //Game.ThisPlayer.ammoPickup = w_initialReservoirAmmo; 
            //minDmg = w_minDmg;
            maxDmg = w_maxDmg;
            ProjectileSpeed = b_speed;
            Range = w_range;

            //// to receive the additional ammo:
            //Game.ThisPlayer.Inventory.PickupAmmo(this);
        }

        public void Fire()
        {
            // checking if we have enough ammo
            if (c_chamberAmmo != 0)
            {
                State = WeaponState.Firing;
                // Putting projectile in action through Bullet Manager
                // These methods are not final at all, they WILL be changed.
                //bulletManager.SpawnBullet();
                c_chamberAmmo--;
                fireTimer = 0; // reset the fire timer
                State = WeaponState.AtEase;
            }
            else if (IsReloadable())
            {
                State = WeaponState.Reloading;
            }
        }

        public bool IsReloadable()
        {
            return (c_chamberAmmo < m_chamberAmmo &&
                c_reservoirAmmo > 0);
        }

        public bool HasAmmo()
        {
            return (c_chamberAmmo > 0);
        }
    }
}