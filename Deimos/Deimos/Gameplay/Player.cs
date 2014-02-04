using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    public class Player
    {
        public Vector3 Position;
        

        public Vector3 Rotation;
        

        public Vector3 LookAt;
        

        public enum Teams
        {
            Humans,
            Aliens
        }
        public Teams Team;

        public Weapon CurrentWeapon;

        private int health;
        public int Health
        {
            get { return health; }
            set { health = value; }
        }
        
        public float Speed = 30;
    }
}
