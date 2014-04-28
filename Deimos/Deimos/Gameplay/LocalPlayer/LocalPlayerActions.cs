using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Deimos
{
    public class LocalPlayerActions
    {
        public LocalPlayerActions()
        {
            //
        }

        private void HandleSpeed()
        {
            switch (GeneralFacade.Game.ThisPlayer.CurrentSpeedState)
            {
                case LocalPlayer.SpeedState.Running:
                    //Game.ThisPlayer.Speed = Game.ThisPlayer.RunSpeed;
                    break;

                case LocalPlayer.SpeedState.Sprinting:
                    if (GeneralFacade.Game.ThisPlayer.ks.IsKeyDown(GeneralFacade.Game.Config.Forward))
                    {
                        if (GeneralFacade.Game.ThisPlayer.SprintTimer < GeneralFacade.Game.ThisPlayer.MaxSprintTime)
                        {
                            //Game.ThisPlayer.Speed = Game.ThisPlayer.SprintSpeed;
                        }
                        else
                        {
                            GeneralFacade.Game.ThisPlayer.CurrentSpeedState = LocalPlayer.SpeedState.Running;
                            //Game.ThisPlayer.Speed = Game.ThisPlayer.RunSpeed;
                            GeneralFacade.Game.ThisPlayer.SprintTimer = 0f;
                            GeneralFacade.Game.ThisPlayer.CooldownTimer = 0f;
                        }
                    }
                    else
                    {
                        GeneralFacade.Game.ThisPlayer.CurrentSpeedState = LocalPlayer.SpeedState.Running;
                        //Game.ThisPlayer.Speed = Game.ThisPlayer.RunSpeed;
                    }
                    break;

                case LocalPlayer.SpeedState.Walking:
                    //Game.ThisPlayer.Speed = Game.ThisPlayer.WalkSpeed;
                    break;
            }
        }

        private void HandleTimers(float dt)
        {
            // increasing sprint timer if sprinting
            if (GeneralFacade.Game.ThisPlayer.CurrentSpeedState ==
                LocalPlayer.SpeedState.Sprinting &&
                GeneralFacade.Game.ThisPlayer.ks.IsKeyDown(GeneralFacade.Game.Config.Forward))
            {
                GeneralFacade.Game.ThisPlayer.SprintTimer += dt;
            }

            else
            {
                // cooling down sprint if not already
                if (GeneralFacade.Game.ThisPlayer.CooldownTimer < 
                    GeneralFacade.Game.ThisPlayer.SprintCooldown)
                {
                    GeneralFacade.Game.ThisPlayer.CooldownTimer += dt;
                }

                // setting the sprint timer
                if (GeneralFacade.Game.ThisPlayer.SprintTimer > 0f)
                {
                    float cd = 0f;

                    if (GeneralFacade.Game.ThisPlayer.CurrentSpeedState == LocalPlayer.SpeedState.Running)
                    {
                        cd = dt / 2f;
                    }
                    else if (GeneralFacade.Game.ThisPlayer.CurrentSpeedState == LocalPlayer.SpeedState.Walking)
                    {
                        cd = dt;
                    }

                    GeneralFacade.Game.ThisPlayer.SprintTimer -= cd;
                }
            }
        }

        private bool CanSprint()
        {
            return (GeneralFacade.Game.ThisPlayer.FireSprint() &&

                (GeneralFacade.Game.ThisPlayer.CooldownTimer >=
                GeneralFacade.Game.ThisPlayer.SprintCooldown) &&

                (GeneralFacade.Game.ThisPlayerPhysics.GravityState ==
                LocalPlayerPhysics.PhysicalState.Walking) &&

                (GeneralFacade.Game.ThisPlayer.ks.IsKeyDown(GeneralFacade.Game.Config.Forward)) &&

                (GeneralFacade.Game.ThisPlayer.CurrentWeapon.State ==
                WeaponState.AtEase));
        }

        private bool CanJump()
        {
            return (GeneralFacade.Game.ThisPlayerPhysics.GravityState ==
                LocalPlayerPhysics.PhysicalState.Walking);
        }


        private void CheckActions()
        {
            GeneralFacade.Game.ThisPlayer.ks = Keyboard.GetState();

            if (GeneralFacade.Game.ThisPlayer.ks.IsKeyDown(GeneralFacade.Game.Config.Walk))
            {
                GeneralFacade.Game.ThisPlayer.CurrentSpeedState = 
                    LocalPlayer.SpeedState.Walking;
            }

            if (GeneralFacade.Game.ThisPlayer.ks.IsKeyDown(GeneralFacade.Game.Config.Sprint) &&
                CanSprint())
            {
                GeneralFacade.Game.ThisPlayer.CurrentSpeedState = 
                    LocalPlayer.SpeedState.Sprinting;
            }

            if (GeneralFacade.Game.ThisPlayer.ks.IsKeyDown(GeneralFacade.Game.Config.Jump) &&
                CanJump())
            {
                GeneralFacade.Game.ThisPlayerPhysics.Jump();
            }
        }

        private void CheckReset()
        {
            // Let's handle walkstate resets
            if (GeneralFacade.Game.ThisPlayer.ks.IsKeyUp(GeneralFacade.Game.Config.Walk) &&
               (GeneralFacade.Game.ThisPlayer.ks.IsKeyUp(GeneralFacade.Game.Config.Sprint)) &&
               (GeneralFacade.Game.ThisPlayer.ks.IsKeyUp(GeneralFacade.Game.Config.Crouch)))
            {
                GeneralFacade.Game.ThisPlayer.CurrentSpeedState = 
                    LocalPlayer.SpeedState.Running;
            }
        }
 
        public void HandleActions(float dt)
        {
            CheckReset();
            HandleSpeed();
            CheckActions();
            HandleTimers(dt);
        }
    }
}
