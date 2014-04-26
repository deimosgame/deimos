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
        public PickupWeapon(string name, string path,
            Vector3 position, string token,
            string weaponToken, int initial_ammo,
            State state, float respawn,
            Vector3 rotation = default(Vector3))
        {
            Name = name;
            Model = path;
            Position = position;
            Rotation = rotation;

            Token = token;
            Represents = weaponToken;
            Ammo = initial_ammo;

            Status = state;

            T_Respawn = respawn;
        }
    }
}
