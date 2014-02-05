using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    public class Weapon
    {
        DeimosGame Game;
        BulletManager bulletManager;

        public enum WeaponState
        {
            AtEase,
            Firing
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

        public uint c_chamberAmmo; // current ammo in the chamber
        public uint m_chamberAmmo; // maximum chamber ammo
        public uint c_reservoirAmmo; // current ammo in the reservoir
        public uint m_reservoirAmmo; // maximum reservoir ammo
        public uint ammoPickup; // amount of ammo that is potentially picked up

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
        public Weapon(DeimosGame game, string w_name, int w_priority, 
            float w_firingRate, uint w_mca, uint w_mra, 
            uint w_initialReservoirAmmo,float w_minDmg, float w_maxDmg, 
            float b_speed, float w_range)
        {
            Game = game;
            bulletManager = game.BulletManager;

            name = w_name;
            Importance = w_priority;
            FiringRate = w_firingRate;
            m_chamberAmmo = w_mca;
            // to start out with a full chamber when weapon spawns
            c_chamberAmmo = m_chamberAmmo; 
            m_reservoirAmmo = w_mra;
            // if we want to give extra ammo to the player right from the pickup, we can
            ammoPickup = w_initialReservoirAmmo; 
            minDmg = w_minDmg;
            maxDmg = w_maxDmg;
            ProjectileSpeed = b_speed;
            Range = w_range;

            // to receive the additional ammo:
            Game.ThisPlayer.Inventory.PickupAmmo(this);
        }

        public void Fire()
        {
            // checking if we have enough ammo
            if (c_chamberAmmo != 0)
            {
                // Putting projectile in action through Bullet Manager
                // These methods are not final at all, they WILL be changed.
                //bulletManager.SpawnBullet();
                c_chamberAmmo--;
            }
            else Game.ThisPlayer.Inventory.Reload();
        }
    }
}