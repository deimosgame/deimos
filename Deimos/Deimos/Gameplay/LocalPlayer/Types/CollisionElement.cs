using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    class CollisionElement
    {
        public enum CollisionType
        {
            Model,
            Sphere,
            Box
        }
        public CollisionType ElementType
        {
            get;
            set;
        }
        public LevelModel Model
        {
            get;
            set;
        }
        public BoundingBox Box
        {
            get;
            set;
        }
        public BoundingSphere Sphere
        {
            get;
            set;
        }

        public Action<CollisionElement, DeimosGame> Event
        {
            get;
            set;
        }

        public CollisionElement()
        {
            Event = delegate(CollisionElement element, DeimosGame game) {};
        }
    }
}
