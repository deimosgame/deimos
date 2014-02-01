using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Deimos
{
    class PlayerPhysics
    {
        public enum PhysicalState
        {
            MidAir,
            Walking
        }

        PhysicalState state = PhysicalState.MidAir;
        public PhysicalState State
        {
            get { return state; }
            set { state = value; }
        }

        DeimosGame Game;
        float InitialVelocity = 0;
        float CurrentTime = 0;
        float GravityCoef = 9.8f;

        public PlayerPhysics(DeimosGame game)
        {
            Game = game;
        }

        public void GetInitGravity(float v)
        {
            InitialVelocity = v;
            CurrentTime = 0;
            State = PhysicalState.MidAir;
        }

        public void StopGravity()
        {
            State = PhysicalState.Walking;

        }

        private void ApplyGravity(GameTime gameTime)
        {
            float y = (InitialVelocity * CurrentTime) - (GravityCoef * (CurrentTime * CurrentTime) * 0.5f);
            float dt = (float) gameTime.ElapsedGameTime.TotalSeconds;
            Game.ThisPlayer.Move(new Vector3(0, y, 0), dt);
        }

        public void Update(GameTime gameTime)
        {
            if (State == PhysicalState.MidAir)
            {
                ApplyGravity(gameTime);
            }
        }
    }
}
