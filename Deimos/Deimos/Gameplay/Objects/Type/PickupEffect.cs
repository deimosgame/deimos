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
        public float Intensity;

        public float E_Duration;
        public float t_effect = 0;

        // Constructor
        public PickupEffect(string name, string path, float scale, Effect effect)
        {
            Name = name;
            Model = path;
            Scale = scale;
            O_Effect = effect;
        }
    }
}
