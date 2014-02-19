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

        public enum AccelerationDirection
        {
            X,
            Y,
            Z
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
        public float dv = 0.15f;
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
        private Vector3 acceleration;
        private Vector3 Acceleration
        {
            get { return acceleration; }
            set
            {
                acceleration.X = Math.Max(-1, Math.Min(value.X, 1));
                acceleration.Y = Math.Max(-1, Math.Min(value.Y, 1));
                acceleration.Z = Math.Max(-1, Math.Min(value.Z, 1));
            }
        }

        // Constructor
        public LocalPlayerPhysics(DeimosGame game)
        {
            Game = game;
        }

        // Methods
        public void Accelerate(AccelerationDirection dir)
        {
            if (Accelerestate == AccelerationState.Still
                || Accelerestate == AccelerationState.Off)
            {
                Accelerestate = AccelerationState.On;
            }
            switch(dir)
            {
                case AccelerationDirection.X:
                    Acceleration += new Vector3(dv, 0, 0);
                    break;
                case AccelerationDirection.Y:
                    Acceleration += new Vector3(0, dv, 0);
                    break;
                case AccelerationDirection.Z:
                    Acceleration += new Vector3(0, 0, dv);
                    break;
            }
        }

        public void Decelerate(AccelerationDirection dir)
        {
            if (Accelerestate == AccelerationState.On
                || Accelerestate == AccelerationState.Constant
                || Accelerestate == AccelerationState.Maxed)
            {
                Accelerestate = AccelerationState.Off;
            }
            switch (dir)
            {
                case AccelerationDirection.X:
                    Acceleration -= new Vector3(dv, 0, 0);
                    break;
                case AccelerationDirection.Y:
                    Acceleration -= new Vector3(0, dv, 0);
                    break;
                case AccelerationDirection.Z:
                    Acceleration -= new Vector3(0, 0, dv);
                    break;
            }
        }

        public void Reset(AccelerationDirection dir)
        {
            switch(dir)
            {
                case AccelerationDirection.X:
                    if (Acceleration.X == 0) return;
                    if (Acceleration.X > 0)
                    {
                        Decelerate(dir);
                        if (Acceleration.X < 0)
                        {
                            Acceleration = new Vector3(0, Acceleration.Y, Acceleration.Z);
                        }
                    }
                    else
                    {
                        Accelerate(dir);
                        if (Acceleration.X > 0)
                        {
                            Acceleration = new Vector3(0, Acceleration.Y, Acceleration.Z);
                        }
                    }
                    break;
                case AccelerationDirection.Y:
                    if (Acceleration.Y == 0) return;
                    if (Acceleration.Y > 0)
                    {
                        Decelerate(dir);
                        if (Acceleration.Y < 0)
                        {
                            Acceleration = new Vector3(Acceleration.X, 0, Acceleration.Z);
                        }
                    }
                    else
                    {
                        Accelerate(dir);
                        if (Acceleration.Y > 0)
                        {
                            Acceleration = new Vector3(Acceleration.X, 0, Acceleration.Z);
                        }
                    }
                    break;
                case AccelerationDirection.Z:
                    if (Acceleration.Z == 0) return;
                    if (Acceleration.Z > 0)
                    {
                        Decelerate(dir);
                        if (Acceleration.Z < 0)
                        {
                            Acceleration = new Vector3(Acceleration.X, Acceleration.Y, 0);
                        }
                    }
                    else
                    {
                        Accelerate(dir);
                        if (Acceleration.Z > 0)
                        {
                            Acceleration = new Vector3(Acceleration.X, Acceleration.Y, 0);
                        }
                    }
                    break;
            }
        }

        public Vector3 GetAcceleration()
        {
            return Acceleration;
        }
    }
}
