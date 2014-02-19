using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Deimos
{
    class LocalPlayerPhysics
    {
        DeimosGame Game;

        public enum PhysicalState
        {
            Jumping,
            Falling,
            Walking
        }

        public enum AccelerationState
        {
            Still,
            On,
            Constant,
            Off,
            Maxed
        }

        public PhysicalState GravityState = PhysicalState.Walking;
        public AccelerationState Accelerestate = AccelerationState.Still;

        public float dv = 0.05f;

        // Constructor
        public LocalPlayerPhysics(DeimosGame game)
        {
            Game = game;
        }

        // Methods
        public void Accelerate()
        {
            if (Accelerestate == AccelerationState.Still
                || Accelerestate == AccelerationState.Off)
            {
                Accelerestate = AccelerationState.On;
            }
        }

        public void Decelerate()
        {
            if (Accelerestate == AccelerationState.On
                || Accelerestate == AccelerationState.Constant
                || Accelerestate == AccelerationState.Maxed)
            {
                Accelerestate = AccelerationState.Off;
            }
        }

        public float GetDV()
        {
            switch (Accelerestate)
            {
                case AccelerationState.On:
                    return dv;
                case AccelerationState.Off:
                    return -dv;
                default:
                    return 0;
            }
        }
    }
}
