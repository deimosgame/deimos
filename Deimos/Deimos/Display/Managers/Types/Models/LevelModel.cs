using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    class LevelModel
    {
        public Vector3 Position
        {
            get;
            set;
        }
        public float Scale
        {
            get;
            set;
        }
        public Vector3 Rotation
        {
            get;
            set;
        }
        public CollisionType CollisionDetection
        {
            get;
            set;
        }
        public CollidableModel.CollidableModel CollisionModel
        {
            get;
            set;
        }
        public enum CollisionType
        {
            None,
            Accurate
        }

    }
}
