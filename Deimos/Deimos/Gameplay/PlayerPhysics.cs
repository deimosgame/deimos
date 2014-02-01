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
            Jumping,
            Falling,
            Walking
        }

        PhysicalState state = PhysicalState.Jumping;
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
            State = PhysicalState.Jumping;
        }

        public void StopGravity()
        {
            if (State == PhysicalState.Jumping)
            {
                InitialVelocity = 0;
                CurrentTime = 0;
                State = PhysicalState.Falling;
            }
            else
            {
                State = PhysicalState.Walking;
            }
        }


        public float ApplyGravity(float dt)
        {
            float y = (InitialVelocity * CurrentTime) - (GravityCoef * (CurrentTime * CurrentTime) * 0.5f);
            if (y < 0)
            {
                State = PhysicalState.Falling;
            }
            CurrentTime += dt;
            return y;
        }
    }
}
