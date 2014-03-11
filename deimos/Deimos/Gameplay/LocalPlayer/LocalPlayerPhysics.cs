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

        public PhysicalState GravityState = PhysicalState.Falling;
        public float t = 0f;
        public float c_gravity = 9.8f;
        public float vi;

        public AccelerationState Accelerestate = AccelerationState.Still;
        public Vector3 dv = new Vector3(0.3f, 0.1f, 0.1f);
        private Vector3 acceleration;
        public Vector3 Acceleration
        {
            get { return acceleration; }
            set
            {
                acceleration.X = Math.Max(-GetMaxHorizAcceleration(), Math.Min(value.X, GetMaxHorizAcceleration()));
                acceleration.Y = Math.Max(-GetMaxVertAcceleration(), Math.Min(value.Y, GetMaxVertAcceleration()));
                acceleration.Z = Math.Max(-GetMaxHorizAcceleration(), Math.Min(value.Z, GetMaxHorizAcceleration()));
            }
        }

        // Constructor
        public LocalPlayerPhysics(DeimosGame game)
        {
            Game = game;
        }

        // Methods
        public Vector3 GetAcceleration()
        {
            return Acceleration;
        }

        // MOVEMENT RELATED
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
                    Acceleration += new Vector3(dv.X, 0, 0);
                    break;
                case AccelerationDirection.Y:
                    Acceleration += new Vector3(0, 1f, 0);
                    break;
                case AccelerationDirection.Z:
                    if (acceleration.Z < GetMaxHorizAcceleration())
                    {
                        Acceleration += new Vector3(0, 0, dv.Z);

                        if (acceleration.Z > GetMaxHorizAcceleration())
                        {
                            acceleration.Z = 1f;
                        }
                    }
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
                    Acceleration -= new Vector3(dv.X, 0, 0);
                    break;
                case AccelerationDirection.Y:
                    Acceleration -= new Vector3(0, dv.Y, 0);
                    break;
                case AccelerationDirection.Z:
                    Acceleration -= new Vector3(0, 0, dv.Z);
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

        private float GetMaxHorizAcceleration()
        {
            float primal_speed;

            switch (Game.ThisPlayer.CurrentSpeedState)
            {
                case Player.SpeedState.Running:
                    primal_speed = 1f;
                    break;

                case Player.SpeedState.Sprinting:
                    primal_speed = 1.5f;
                    break;

                case Player.SpeedState.Walking:
                    primal_speed = 0.5f;
                    break;

                case Player.SpeedState.Crouching:
                    primal_speed = 0.3f;
                    break;

                default:
                    primal_speed = 1f;
                    break;
            }

            return primal_speed;
        }

        // GRAVITY RELATED
        private float GetMaxVertAcceleration()
        {
            switch (GravityState)
            {
                case PhysicalState.Walking:
                    return 0f;

                case PhysicalState.Jumping:
                    return 1f;

                case PhysicalState.Falling:
                    return 1f;

                default:
                    return 0f;
            }
        }

        public void ApplyGravity(float dt)
        {
            float vy = (vi * t) - ((float)Math.Pow(t, 2) * c_gravity);
            acceleration.Y = vy;
            
            if (vy < 0f)
            { 
                GravityState = PhysicalState.Falling;
            }

            t += dt;
        }

        public void StabilizeGravity()
        {
            if (GravityState == PhysicalState.Falling)
            {
                t = 0;
                vi = 0;
                GravityState = PhysicalState.Walking;
                acceleration.Y = 0f;
                Reset(AccelerationDirection.Z);
            }

            if (GravityState == PhysicalState.Jumping)
            {
                GravityState = PhysicalState.Falling;
            }
        }

        public void Jump()
        {
            vi = 8f;
            t = 0f;
            GravityState = PhysicalState.Jumping;


            if (acceleration.Z > 0)
            {
                Accelerestate = AccelerationState.Maxed;
                acceleration.Z += dv.Z;
                
            }
        }
    }
}
