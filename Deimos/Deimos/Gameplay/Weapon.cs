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

        private WeaponState state;
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

        private float firingRate;
        public float FiringRate
        {
            get { return firingRate; }
            set { firingRate = value; }
        }

        private int maxCartridgeAmmo;
        private int maxCartridges;
        private int currentCartridge;
        public int CurrentCartridge
        {
            get { return currentCartridge; }
            set
            {
                if (value <= maxCartridges)
                {
                    currentCartridge = value;
                }
            }
        }
        private int currentAmmo;
        public int CurrentAmmo
        {
            get { return currentAmmo; }
            set
            {
                if (value <= maxCartridgeAmmo)
                {
                    currentAmmo = value;
                }
            }
        }

        public float minDmg;
        public float maxDmg;

        private float projectileSpeed;
        public float ProjectileSpeed
        {
            get { return projectileSpeed; }
            set { projectileSpeed = value; }
        }

        private float range;
        public float Range
        {
            get { return range; }
            set { range = Range; }
        }

        public void Fire()
        {
            // Putting projectile in action through Bullet Manager
            bulletManager.Propagate(bulletManager.SpawnBullet(this), 5f); // These methods are not final at all, they WILL be changed.
        }

        // Constructor
        public Weapon(DeimosGame game)
        {
            Game = game;
            bulletManager = game.BulletManager;
        }
    }
}