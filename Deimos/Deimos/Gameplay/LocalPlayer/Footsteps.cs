using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    class Footsteps
    {
        Random rng = new Random();
        int current = 0;
        float timer = 0;
        float time = 0;

        public void HandleFootsteps(float dt)
        {
            if (GameplayFacade.ThisPlayerPhysics.GetMoveState()
                == Physics.MoveState.Moving
                && GameplayFacade.ThisPlayerPhysics.GravityState ==
                Physics.PhysicalState.Walking)
            {
                timer = GameplayFacade.ThisPlayer.Speed / 100;

                if (GameplayFacade.ThisPlayer.CurrentSpeedState ==
                    Player.SpeedState.Sprinting)
                {
                    timer /= 1.25f;
                }
                else if (GameplayFacade.ThisPlayer.CurrentSpeedState ==
                    Player.SpeedState.Walking ||
                    GameplayFacade.ThisPlayer.CurrentSpeedState ==
                    Player.SpeedState.Crouching)
                {
                    timer *= 1.8f;
                }

                if (time >= timer)
                {
                    time = 0;

                    current = rng.Next(0, 4);

                    switch (current)
                    {
                        case 0:
                            GeneralFacade.SceneManager.SoundManager.Play(
                                "s1");
                            return;
                        case 1:
                            GeneralFacade.SceneManager.SoundManager.Play(
                                "s2");
                            return;
                        case 2:
                            GeneralFacade.SceneManager.SoundManager.Play(
                                "s3");
                            return;
                        case 3:
                            GeneralFacade.SceneManager.SoundManager.Play(
                                "s4");
                            return;
                        default:
                            GeneralFacade.SceneManager.SoundManager.Play(
                                "s1");
                            return;
                    }
                }
                else
                {
                    time += dt;
                }
            }
        }
    }
}
