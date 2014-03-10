using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Deimos
{
    public class LocalPlayerActions
    {
        DeimosGame Game;
        LocalPlayer Player;

        public LocalPlayerActions(DeimosGame game)
        {
            Game = game;
        }

        private void HandleSpeed()
        {
            switch (Game.ThisPlayer.CurrentSpeedState)
            {
                case LocalPlayer.SpeedState.Running:
                    //Game.ThisPlayer.Speed = Game.ThisPlayer.RunSpeed;
                    break;

                case LocalPlayer.SpeedState.Sprinting:
                    if (Game.ThisPlayer.ks.IsKeyDown(Game.Config.Forward))
                    {
                        if (Game.ThisPlayer.SprintTimer < Game.ThisPlayer.MaxSprintTime)
                        {
                            //Game.ThisPlayer.Speed = Game.ThisPlayer.SprintSpeed;
                        }
                        else
                        {
                            Game.ThisPlayer.CurrentSpeedState = LocalPlayer.SpeedState.Running;
                            //Game.ThisPlayer.Speed = Game.ThisPlayer.RunSpeed;
                            Game.ThisPlayer.SprintTimer = 0f;
                            Game.ThisPlayer.CooldownTimer = 0f;
                        }
                    }
                    else
                    {
                        Game.ThisPlayer.CurrentSpeedState = LocalPlayer.SpeedState.Running;
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
            if (Game.ThisPlayer.CurrentSpeedState ==
                LocalPlayer.SpeedState.Sprinting && 
                Game.ThisPlayer.ks.IsKeyDown(Game.Config.Forward))
            {
                Game.ThisPlayer.SprintTimer += dt;
            }

            else
            {
                // cooling down sprint if not already
                if (Game.ThisPlayer.CooldownTimer < Game.ThisPlayer.SprintCooldown)
                {
                    Game.ThisPlayer.CooldownTimer += dt;
                }

                // setting the sprint timer
                if (Game.ThisPlayer.SprintTimer > 0f)
                {
                    float cd = 0f;

                    if (Game.ThisPlayer.CurrentSpeedState == LocalPlayer.SpeedState.Running)
                    {
                        cd = dt / 2f;
                    }
                    else if (Game.ThisPlayer.CurrentSpeedState == LocalPlayer.SpeedState.Walking)
                    {
                        cd = dt;
                    }

                    Game.ThisPlayer.SprintTimer -= cd;
                }
            }
        }

        private bool CanSprint()
        {
            return ((Game.ThisPlayer.CooldownTimer >=
                Game.ThisPlayer.SprintCooldown) &&

                (Game.ThisPlayerPhysics.GravityState ==
                LocalPlayerPhysics.PhysicalState.Walking) &&

                (Game.ThisPlayer.ks.IsKeyDown(Game.Config.Forward)) &&

                (Game.ThisPlayer.CurrentWeapon.State ==
                WeaponState.AtEase));
        }


        private void CheckActions()
        {
            Game.ThisPlayer.ks = Keyboard.GetState();

            if (Game.ThisPlayer.ks.IsKeyDown(Game.Config.Walk))
            {
                Game.ThisPlayer.CurrentSpeedState = 
                    LocalPlayer.SpeedState.Walking;
            }

            if (Game.ThisPlayer.ks.IsKeyDown(Game.Config.Sprint) &&
                CanSprint())
            {
                Game.ThisPlayer.CurrentSpeedState = 
                    LocalPlayer.SpeedState.Sprinting;
            }
        }

        private void CheckReset()
        {
            // Let's handle walkstate resets
            if (Game.ThisPlayer.ks.IsKeyUp(Game.Config.Walk) &&
               (Game.ThisPlayer.ks.IsKeyUp(Game.Config.Sprint)) &&
               (Game.ThisPlayer.ks.IsKeyUp(Game.Config.Crouch)))
            {
                Game.ThisPlayer.CurrentSpeedState = 
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
