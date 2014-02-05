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
        public Weapon PreviousWeapon; // for quick-switch purposes

        private uint health;
        public uint Health
        {
            get { return health; }
            set { health = value; }
        }
        
        public float Speed = 70;

        public int Score = 0;
    }
}
