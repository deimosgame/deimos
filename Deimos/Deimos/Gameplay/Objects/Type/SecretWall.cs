using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Deimos
{
    class SecretWall : SecretObject
    {
        public SecretWall(Vector3 dimensions, string path, float scale)
            : base(dimensions)
        {
            Model = path;
            Scale = scale;

            Nature = ElementNature.SecretWall;
        }

        public override void CollisionEvent(CollisionElement element)
        {
            if (element.GetNature() == ElementNature.Bullet)
            {
                Manager.HandleWallDiscovery(Token);
            }
        }
    }
}
