using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tranquillity;

namespace Deimos
{
    class SmokeParticleEmitter : IParticleEmitter
    {

        protected Vector3 Position;

        public DynamicParticleSystem ParticleSystem
        {
            get;
            set;
        }

        public SmokeParticleEmitter(Vector3 position)
        {
            Position = position;
        }


        public void Emit(int particlesToEmit)
        {
            //throw new NotImplementedException();
            for (int i = 0; i < particlesToEmit; i++)
            {
                ParticleSystem.AddParticle(Position, Color.White, Vector3.Up, null, new TimeSpan(0, 0, 5));
            }
        }

        public void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            //
        }
    }
}
