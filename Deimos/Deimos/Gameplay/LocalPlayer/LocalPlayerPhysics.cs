using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Deimos
{
    class LocalPlayerPhysics
    {
        public enum PhysicalState
        {
            Jumping,
            Falling,
            Walking
        }

        PhysicalState state = PhysicalState.Walking;
        public PhysicalState State
        {
            get { return state; }
            set { state = value; }
        }

        DeimosGame Game;
        float InitialVelocity = 0;
        float CurrentTime = 0;
        float GravityCoef = 12f;
        bool ApplyingGravity = false;

        public LocalPlayerPhysics(DeimosGame game)
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
            else if (State == PhysicalState.Falling)
            {
                InitialVelocity = 0;
                CurrentTime = 0;
                State = PhysicalState.Walking;
            }
        }


        public float ApplyGravity(float dt)
        {
            float y = (InitialVelocity * CurrentTime) - 0.5f * (GravityCoef * (float)Math.Pow(CurrentTime, 2));
            if (y <= 0 && CurrentTime != 0)
            {
                State = PhysicalState.Falling;
            }
            CurrentTime += dt;
            return y;
        }
    }
}
