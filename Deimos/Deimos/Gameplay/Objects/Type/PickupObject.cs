using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Deimos
{
    abstract public class PickupObject : CollisionElement
    {
        // This is the base class for 
        // World objects that are pickup objects

        public ObjectManager Manager;

        public enum State
        {
            Active,
            Inactive
        }

        public string Name;
        public string Model;

        public float Scale;

        public Vector3 Position;
        public Vector3 Rotation;

        public string Token;

        public State Status;
        public bool Show;

        public float T_Respawn;
        public float respawn_timer = 0;

        // Constructor

        public PickupObject(Vector2 bbox)
            : base(bbox)
        {

        }
    }
}
