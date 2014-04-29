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
        public char Represents;
        public int Ammo;

        // Constructor
        public PickupWeapon(string name, string path, float scale,
            char weaponToken)
            : base(new Vector3(10, 10f, 10f))
        {
            Name = name;
            Model = path;
            Scale = scale;
            Represents = weaponToken;

            Nature = ElementNature.Object;
        }

        public override void CollisionEvent(CollisionElement element)
        {
            if (element.GetNature() == ElementNature.Player)
            {
                Manager.TreatWeapon(this, Token);
            }
        }
    }
}
