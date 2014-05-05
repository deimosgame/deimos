using Microsoft.Xna.Framework;
using System;
using Tranquillity;

namespace Deimos
{
    class ExplosionParticleEmitter : IParticleEmitter
    {
        protected Vector3 Position;

        public DynamicParticleSystem ParticleSystem { get; set; }

        public ExplosionParticleEmitter(Vector3 pos)
        {
            Position = pos;
        }

        public void Emit(int n)
        {
            for (int i = 0; i < n; i++)
            {
                Vector3 velocity;
                velocity.X = (float)(HelperFacade.Helpers.Random.NextDouble() - 0.5) * 60;
                velocity.Y = (float)(HelperFacade.Helpers.Random.NextDouble() + 0.5) * 40;
                velocity.Z = (float)(HelperFacade.Helpers.Random.NextDouble() - 0.5) * 60;
                ParticleSystem.AddParticle(
                        Position,
                        HelperFacade.Helpers.ColorBetween(Color.DarkGray, Color.Gray),
                        velocity * 0.01f + new Vector3(HelperFacade.Helpers.FloatBetween(-30, 30), HelperFacade.Helpers.FloatBetween(30, -10), HelperFacade.Helpers.FloatBetween(-30, 30)) * 0.05f,
                        HelperFacade.Helpers.FloatBetween(-0.01f, 0.01f),
                        TimeSpan.FromSeconds(HelperFacade.Helpers.IntBetween(1, 2)),
                        true,
                        HelperFacade.Helpers.FloatBetween(0.0f, MathHelper.Pi),
                        0.1f);
            }
        }

        public void Update(GameTime gameTime)
        {
            //
        }
    }
}
