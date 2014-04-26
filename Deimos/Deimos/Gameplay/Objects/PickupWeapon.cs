using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Deimos
{
    public class PickupWeapon : PickupObject
    {
        // Attributes
        public string Represents;
        public int Ammo;

        // Constructor
        public PickupWeapon(string name, string path, float scale,
            string weaponToken)
        {
            Name = name;
            Model = path;
            Scale = scale;
            Represents = weaponToken;
        }

        // Methods
        public override void Treat()
        {
            
        }
    }
}
