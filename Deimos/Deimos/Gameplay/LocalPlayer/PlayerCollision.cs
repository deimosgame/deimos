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

    }
}
