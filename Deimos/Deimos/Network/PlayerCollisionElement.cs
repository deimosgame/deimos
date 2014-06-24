using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    class PlayerCollisionElement : CollisionElement
    {
        public PlayerCollisionElement(Vector2 vec)
            : base(vec)
        {
            Nature = ElementNature.Player;
        }

        public override bool FilterCollisionElement(CollisionElement element)
        {
            return (element.GetNature() != ElementNature.Bullet);
        }
    }
}
