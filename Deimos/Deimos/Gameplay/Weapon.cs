using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    class Weapon
    {
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

        private Bullet bullet;

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

        private int maxAmmo;
        private int minAmmo = 0;

        private int currentAmmo;
        public int CurrentAmmo
        {
            get { return currentAmmo; }
            set { currentAmmo = value; }
        }

        private void SetBullet()
        {
            bullet = new Bullet(); // Constructing the projectile, function called by Fire()
        }

        private void Fire()
        {
            // Putting projectile in action
        }
    }
}
