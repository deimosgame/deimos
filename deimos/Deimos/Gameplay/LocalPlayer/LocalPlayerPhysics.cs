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
        LocalPlayer CurrentPhysicsPlayer;

        float InitialVelocity = 0;
        float CurrentTime = 0;

        float GravityCoef = 12f;

        // bool ApplyingGravity = false;

        private float bunnyhopCoeff = 1f;
        public float BunnyhopCoeff
        {
            get { return bunnyhopCoeff; }
            set {
                if (value > 3.5f)
                {
                    return;
                }
                bunnyhopCoeff = value; 
            }
        }


        public LocalPlayerPhysics(DeimosGame game, LocalPlayer player)
        {
            Game = game;
            CurrentPhysicsPlayer = player;
        }

        public void InitiateJump(float v)
        {
            // Setting initial state for applied gravity
            InitialVelocity = v;
            CurrentTime = 0;
            State = PhysicalState.Jumping;
        }

        public void StopGravity() // Changing states, to start descending or stop descending
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
                BunnyhopCoeff += 0.2f;
            }
        }


        public float ApplyGravity(float dt)
        {
            float y = (InitialVelocity * CurrentTime) - 0.8f * (GravityCoef * (float)Math.Pow(CurrentTime, 2));
            if (y <= 0 && CurrentTime != 0)
            {
                State = PhysicalState.Falling;
            }

            CurrentTime += dt;
            return y;
        }
    }
}
