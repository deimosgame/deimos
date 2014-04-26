using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Deimos
{
    public class PickupObject
    {
        // This is the base class for 
        // World objects that are pickup objects

        public enum State
        {
            Active,
            Inactive
        }

        public string Name;
        public string Model;

        public string Token;

        public Vector3 Position;
        public Vector3 Rotation;

        public State Status;
        public bool Show;

        public float T_Respawn;
        public float respawn_timer = 0;
    }
}
