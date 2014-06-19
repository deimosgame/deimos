using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Deimos
{
    class SecretObject : CollisionElement
    {
        public SecretsManager Manager;

        public enum State
        {
            Inactive,
            Undiscovered,
            Discovered
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
        public SecretObject(Vector2 dimensions)
            : base(dimensions)
        {

        }
    }
}
