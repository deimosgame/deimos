using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Deimos
{
    public class PickupEffect : PickupObject
    {
        // Attributes
        public enum Effect
        {
            Health,
            Speed,
            Gravity
        }

        public Effect O_Effect;

        // Constructor
        public PickupEffect(string name, string path,
            Vector3 position, Effect effect, string token,
            State state, float respawn,
            Vector3 rotation = default(Vector3))
        {
            Name = name;
            Model = path;
            Position = position;
            Rotation = rotation;
            Token = token;

            O_Effect = effect;

            Status = state;

            T_Respawn = respawn;
        }
    }
}
