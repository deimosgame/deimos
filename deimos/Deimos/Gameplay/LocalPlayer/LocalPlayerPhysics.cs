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
        public void Accelerate(int direction)
        {
            switch (Accelerestate)
            {
                case AccelerationState.Still:
                    Accelerestate = AccelerationState.On;
                    break;

                case AccelerationState.On:
                    ProcessAcceleration(direction);
                    break;

                case AccelerationState.Off:
                    Accelerestate = AccelerationState.On;
                    break;

                case AccelerationState.Constant:
                    break;

                case AccelerationState.Maxed:
                    break;
            }
        }

        public void Decelerate(int direction)
        {
            switch (Accelerestate)
            {
                case AccelerationState.Still:
                    break;

                case AccelerationState.On:
                    Accelerestate = AccelerationState.Off;
                    break;

                case AccelerationState.Constant:
                    Accelerestate = AccelerationState.Off;
                    break;

                case AccelerationState.Off:
                    ProcessDeceleration(direction);
                    break;

                case AccelerationState.Maxed:
                    Accelerestate = AccelerationState.Off;
                    break;
            }
        }

        public void ProcessAcceleration(int direction)
        {
            switch (direction)
            {
                case 0:
                    {
                        if (Game.ThisPlayer.Acceleration.Z < 1)
                        { Game.ThisPlayer.Acceleration.Z += dv; }
                        else
                        { Game.ThisPlayer.Acceleration.Z = 1; }
                    }
                    break;

                case 1:
                    {
                        if (Game.ThisPlayer.Acceleration.Z > -1)
                        { Game.ThisPlayer.Acceleration.Z -= dv; }
                        else
                        { Game.ThisPlayer.Acceleration.Z = -1; }
                    }
                    break;

                case 2:
                    {
                        if (Game.ThisPlayer.Acceleration.X < 1)
                        { Game.ThisPlayer.Acceleration.X += dv; }
                        else
                        { Game.ThisPlayer.Acceleration.X = 1; }
                    }
                    break;

                case 3:
                    {
                        if (Game.ThisPlayer.Acceleration.X > -1)
                        { Game.ThisPlayer.Acceleration.X -= dv; }
                        else
                        { Game.ThisPlayer.Acceleration.X = -1; }
                    }
                    break;
            }
        }

        public void ProcessDeceleration(int direction)
        {
            switch (direction)
            {
                case 0:
                    {
                        if (Game.ThisPlayer.Acceleration.Z > 0)
                        {
                            Game.ThisPlayer.Acceleration.Z -= dv;
                            if (Game.ThisPlayer.Acceleration.Z <= 0)
                            {
                                Game.ThisPlayer.Acceleration.Z = 0;
                            }
                        }
                        else
                        {
                            Game.ThisPlayer.Acceleration.Z += dv;
                            if (Game.ThisPlayer.Acceleration.Z >= 0)
                            {
                                Game.ThisPlayer.Acceleration.Z = 0;
                            }
                        }

                    }
                    break;

                case 1:
                    {
                        if (Game.ThisPlayer.Acceleration.X > 0)
                        {
                            Game.ThisPlayer.Acceleration.X -= dv;
                            if (Game.ThisPlayer.Acceleration.X <= 0)
                            {
                                Game.ThisPlayer.Acceleration.X = 0;
                            }
                        }
                        else
                        {
                            Game.ThisPlayer.Acceleration.X += dv;
                            if (Game.ThisPlayer.Acceleration.X >= 0)
                            {
                                Game.ThisPlayer.Acceleration.X = 0;
                            }
                        }

                    }
                    break;
            }
        }
    }
}
