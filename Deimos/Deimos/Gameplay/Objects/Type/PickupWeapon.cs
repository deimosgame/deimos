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
            : base(new Vector2(10, 20))
        {
            Name = name;
            Model = path;
            Scale = scale;
            Represents = weaponToken;

            Nature = ElementNature.Object;
        }

        public override bool FilterCollisionElement(CollisionElement element)
        {
            return (element.GetNature() != ElementNature.Player);
        }

        public override void CollisionEvent(CollisionElement element)
        {
            Manager.TreatWeapon(this, Token);
        }
    }
}
