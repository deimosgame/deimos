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
            switch (GameplayFacade.ThisPlayer.CurrentSpeedState)
            {
                case LocalPlayer.SpeedState.Running:
                    //Game.ThisPlayer.Speed = Game.ThisPlayer.RunSpeed;
                    break;

                case LocalPlayer.SpeedState.Sprinting:
                    if (GameplayFacade.ThisPlayer.ks.IsKeyDown(GeneralFacade.Game.Config.Forward))
                    {
                        if (GameplayFacade.ThisPlayer.SprintTimer < GameplayFacade.ThisPlayer.MaxSprintTime)
                        {
                            //Game.ThisPlayer.Speed = Game.ThisPlayer.SprintSpeed;
                        }
                        else
                        {
                            GameplayFacade.ThisPlayer.CurrentSpeedState = LocalPlayer.SpeedState.Running;
                            //Game.ThisPlayer.Speed = Game.ThisPlayer.RunSpeed;
                            GameplayFacade.ThisPlayer.SprintTimer = 0f;
                            GameplayFacade.ThisPlayer.CooldownTimer = 0f;
                        }
                    }
                    else
                    {
                        GameplayFacade.ThisPlayer.CurrentSpeedState = LocalPlayer.SpeedState.Running;
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
            if (GameplayFacade.ThisPlayer.CurrentSpeedState ==
                LocalPlayer.SpeedState.Sprinting &&
                GameplayFacade.ThisPlayer.ks.IsKeyDown(GeneralFacade.Game.Config.Forward))
            {
                GameplayFacade.ThisPlayer.SprintTimer += dt;
            }

            else
            {
                // cooling down sprint if not already
                if (GameplayFacade.ThisPlayer.CooldownTimer < 
                    GameplayFacade.ThisPlayer.SprintCooldown)
                {
                    GameplayFacade.ThisPlayer.CooldownTimer += dt;
                }

                // setting the sprint timer
                if (GameplayFacade.ThisPlayer.SprintTimer > 0f)
                {
                    float cd = 0f;

                    if (GameplayFacade.ThisPlayer.CurrentSpeedState == LocalPlayer.SpeedState.Running)
                    {
                        cd = dt / 2f;
                    }
                    else if (GameplayFacade.ThisPlayer.CurrentSpeedState == LocalPlayer.SpeedState.Walking)
                    {
                        cd = dt;
                    }

                    GameplayFacade.ThisPlayer.SprintTimer -= cd;
                }
            }
        }

        private bool CanSprint()
        {
            return (GameplayFacade.ThisPlayer.FireSprint() &&

                (GameplayFacade.ThisPlayer.CooldownTimer >=
                GameplayFacade.ThisPlayer.SprintCooldown) &&

                (GameplayFacade.ThisPlayerPhysics.GravityState ==
                LocalPlayerPhysics.PhysicalState.Walking) &&

                (GameplayFacade.ThisPlayer.ks.IsKeyDown(GeneralFacade.Game.Config.Forward)) &&

                (GameplayFacade.ThisPlayer.CurrentWeapon.State ==
                WeaponState.AtEase));
        }

        private bool CanJump()
        {
            return (GameplayFacade.ThisPlayerPhysics.GravityState ==
                LocalPlayerPhysics.PhysicalState.Walking);
        }


        private void CheckActions()
        {
            GameplayFacade.ThisPlayer.ks = Keyboard.GetState();

            if (GameplayFacade.ThisPlayer.ks.IsKeyDown(GeneralFacade.Game.Config.Walk))
            {
                GameplayFacade.ThisPlayer.CurrentSpeedState = 
                    LocalPlayer.SpeedState.Walking;
            }

            if (GameplayFacade.ThisPlayer.ks.IsKeyDown(GeneralFacade.Game.Config.Sprint) &&
                CanSprint())
            {
                GameplayFacade.ThisPlayer.CurrentSpeedState = 
                    LocalPlayer.SpeedState.Sprinting;
            }

            if (GameplayFacade.ThisPlayer.ks.IsKeyDown(GeneralFacade.Game.Config.Jump) &&
                CanJump())
            {
                GameplayFacade.ThisPlayerPhysics.Jump();
            }
        }

        private void CheckReset()
        {
            // Let's handle walkstate resets
            if (GameplayFacade.ThisPlayer.ks.IsKeyUp(GeneralFacade.Game.Config.Walk) &&
               (GameplayFacade.ThisPlayer.ks.IsKeyUp(GeneralFacade.Game.Config.Sprint)) &&
               (GameplayFacade.ThisPlayer.ks.IsKeyUp(GeneralFacade.Game.Config.Crouch)))
            {
                GameplayFacade.ThisPlayer.CurrentSpeedState = 
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
