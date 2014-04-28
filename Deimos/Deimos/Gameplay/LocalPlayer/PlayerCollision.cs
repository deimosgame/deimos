using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CollidableModel;


namespace Deimos
{
    class PlayerCollision : Collidable
    {
        // Constructor
        public PlayerCollision(float playerHeight, float playerWidth, float playerDepth)
            : base(new Vector3(playerWidth, playerHeight, playerDepth))
        {
            //
        }

        private BoundingBox GenerateBox(Vector3 position, Vector3 dimension)
        {
            Vector3 bbTop = new Vector3(
                position.X - (dimension.X / 2),
                position.Y - (dimension.Y / 5) * 3,
                position.Z - (dimension.Z / 2)
            );
            Vector3 bbBottom = new Vector3(
                position.X + (dimension.X / 2),
                position.Y + (dimension.Y / 5) * 2,
                position.Z + (dimension.Z / 2)
            );
            return new BoundingBox(
                bbTop,
                bbBottom
            );
        }

    }
}
