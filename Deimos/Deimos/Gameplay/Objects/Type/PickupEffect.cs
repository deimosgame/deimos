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
            : base(new Vector2(10, 10))
        {
            Name = name;
            ModelPath = path;
            Scale = scale;
            O_Effect = effect;

            Nature = ElementNature.Object;
        }

        public override bool FilterCollisionElement(CollisionElement element)
        {
            return (element.GetNature() != ElementNature.Player);
        }

        public override void CollisionEvent(CollisionElement element)
        {
            Manager.TreatEffect(this, Token);
        }
    }
}
