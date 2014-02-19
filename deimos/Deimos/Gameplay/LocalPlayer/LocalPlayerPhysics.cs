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

        public Vector3 lastMovementVector = Vector3.Zero;
        public Vector3 LastMovementVector
        {
            get;
            set;
        }
        public float dv = 0.05f;
        private float tempDv = 0;
        private float TempDv
        {
            get
            {
                return tempDv;
            }
            set
            {
                tempDv = Math.Max(0, Math.Min(value, 1));
            }
        }

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
            TempDv += dv;
        }

        public void Decelerate()
        {
            if (Accelerestate == AccelerationState.On
                || Accelerestate == AccelerationState.Constant
                || Accelerestate == AccelerationState.Maxed)
            {
                Accelerestate = AccelerationState.Off;
            }
            TempDv -= dv;
        }

        public float GetDV()
        {
            return TempDv;
        }
    }
}
