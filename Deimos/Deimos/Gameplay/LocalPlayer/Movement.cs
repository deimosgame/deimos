using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Deimos
{
    class Movement
    {
        Vector3 MoveVector;
        Vector3 Acceleration;
        Vector3 LastMoveVector;

        public Movement()
        {
            //
        }

        public Vector3 GetMovementVector(float dt)
        {
            GameplayFacade.ThisPlayer.ks = Keyboard.GetState();
            Vector3 movementVector = Vector3.Zero;

            // Let's handle movement input
            if (GeneralFacade.Game.CurrentPlayingState != DeimosGame.PlayingStates.NoClip)
            {
                if (GameplayFacade.ThisPlayer.ks.IsKeyDown(GeneralFacade.Config.Forward))
                {
                    GameplayFacade.ThisPlayerPhysics.Accelerate(Physics.AccelerationDirection.Z);
                    movementVector += Vector3.Backward;
                }
                if (GameplayFacade.ThisPlayer.ks.IsKeyDown(GeneralFacade.Config.Backward))
                {
                    GameplayFacade.ThisPlayerPhysics.Decelerate(Physics.AccelerationDirection.Z);
                    movementVector += Vector3.Backward;
                }
                if (GameplayFacade.ThisPlayer.ks.IsKeyDown(GeneralFacade.Config.Left))
                {
                    GameplayFacade.ThisPlayerPhysics.Accelerate(Physics.AccelerationDirection.X);
                    movementVector += Vector3.Right;
                }
                if (GameplayFacade.ThisPlayer.ks.IsKeyDown(GeneralFacade.Config.Right))
                {
                    GameplayFacade.ThisPlayerPhysics.Decelerate(Physics.AccelerationDirection.X);
                    movementVector += Vector3.Right;
                }

                if (movementVector != Vector3.Zero)
                {
                    movementVector.Normalize();
                    LastMoveVector = movementVector;
                }
                else
                {
                    movementVector = LastMoveVector;
                }

                //&& GameplayFacade.ThisPlayerPhysics.ShouldResetMovement(LocalPlayerPhysics.AccelerationDirection.Z))
                if (GameplayFacade.ThisPlayer.ks.IsKeyUp(GeneralFacade.Config.Forward)
                    && GameplayFacade.ThisPlayer.ks.IsKeyUp(GeneralFacade.Config.Backward))
                {
                    GameplayFacade.ThisPlayerPhysics.Reset(Physics.AccelerationDirection.Z);
                }
                if (GameplayFacade.ThisPlayer.ks.IsKeyUp(GeneralFacade.Config.Left)
                    && GameplayFacade.ThisPlayer.ks.IsKeyUp(GeneralFacade.Config.Right))
                {
                    GameplayFacade.ThisPlayerPhysics.Reset(Physics.AccelerationDirection.X);
                }

                    GameplayFacade.ThisPlayerPhysics.ApplyGravity(dt);
                    movementVector.Y = 1;

                LastMoveVector = movementVector;

                // * GameplayFacade.ThisPlayerPhysics.GetMomentum()
                return (movementVector * 
                    GameplayFacade.ThisPlayerPhysics.GetAcceleration());
            }
            else
            {
                if (GameplayFacade.ThisPlayer.ks.IsKeyDown(GeneralFacade.Config.Forward))
                {
                    movementVector += Vector3.Backward;
                }
                if (GameplayFacade.ThisPlayer.ks.IsKeyDown(GeneralFacade.Config.Backward))
                {
                    movementVector += Vector3.Forward;
                }
                if (GameplayFacade.ThisPlayer.ks.IsKeyDown(GeneralFacade.Config.Left))
                {
                    movementVector += Vector3.Right;
                }
                if (GameplayFacade.ThisPlayer.ks.IsKeyDown(GeneralFacade.Config.Right))
                {
                    movementVector += Vector3.Left;
                }
                if (GameplayFacade.ThisPlayer.ks.IsKeyDown(GeneralFacade.Config.Jump))
                {
                    movementVector += Vector3.Up;
                }
                if (GameplayFacade.ThisPlayer.ks.IsKeyDown(GeneralFacade.Config.Crouch))
                {
                    movementVector += Vector3.Down;
                }

                return movementVector;
            }

            
        }

        public void GetMouseMovement(float dt)
        {
            GameplayFacade.ThisPlayer.CurrentMouseState = Mouse.GetState();

            // Handle mouse movement
            float deltaX;
            float deltaY;
            if (GameplayFacade.ThisPlayer.CurrentMouseState != GameplayFacade.ThisPlayer.PreviousMouseState)
            {
                // Cache mouse location
                // We devide by 2 because mouse will be in the center
                deltaX = GameplayFacade.ThisPlayer.CurrentMouseState.X
                    - (GeneralFacade.Game.GraphicsDevice.Viewport.Width / 2);
                deltaY = GameplayFacade.ThisPlayer.CurrentMouseState.Y
                    - (GeneralFacade.Game.GraphicsDevice.Viewport.Height / 2);
           

                GameplayFacade.ThisPlayer.MouseRotationBuffer.X -= 
                    GeneralFacade.Config.MouseSensivity * deltaX * dt;
                GameplayFacade.ThisPlayer.MouseRotationBuffer.Y -= 
                    GeneralFacade.Config.MouseSensivity * deltaY * dt;

                // Limit the user so he can't do an unlimited movement with 
                // his mouse (like a 7683°)
                if (GameplayFacade.ThisPlayer.MouseRotationBuffer.Y < MathHelper.ToRadians(-75.0f))
                {
                    GameplayFacade.ThisPlayer.MouseRotationBuffer.Y = GameplayFacade.ThisPlayer.MouseRotationBuffer.Y -
                        (GameplayFacade.ThisPlayer.MouseRotationBuffer.Y - MathHelper.ToRadians(-75.0f));
                }
                if (GameplayFacade.ThisPlayer.MouseRotationBuffer.Y > MathHelper.ToRadians(75.0f))
                {
                    GameplayFacade.ThisPlayer.MouseRotationBuffer.Y = GameplayFacade.ThisPlayer.MouseRotationBuffer.Y -
                        (GameplayFacade.ThisPlayer.MouseRotationBuffer.Y - MathHelper.ToRadians(75.0f));
                }

                float mouseInverted = (GeneralFacade.Config.MouseInverted) ? 1 : -1;

                DisplayFacade.Camera.Rotation = new Vector3(
                    mouseInverted * MathHelper.Clamp(
                        GameplayFacade.ThisPlayer.MouseRotationBuffer.Y,
                        MathHelper.ToRadians(-75.0f),
                        MathHelper.ToRadians(75.0f)
                    ),
                    MathHelper.WrapAngle(GameplayFacade.ThisPlayer.MouseRotationBuffer.X), // This is so 
                    // the camera isn't going really fast after some time 
                    // (as we are increasing the speed with time)
                    0
                );

                // Resetting them
                deltaX = 0;
                deltaY = 0;

            }

            // Putting the cursor in the middle of the screen
            Mouse.SetPosition(GeneralFacade.Game.GraphicsDevice.Viewport.Width / 2,
                GeneralFacade.Game.GraphicsDevice.Viewport.Height / 2);

            GameplayFacade.ThisPlayer.PreviousMouseState = GameplayFacade.ThisPlayer.CurrentMouseState;
        }

        // Set camera position and rotation
        public void MoveTo(Vector3 position, Vector3 rotation)
        {
            // Thanks to the properties set at the beginning, setting up these 
            // values will execute the code inside the property (i.e update our
            // vectors)
            GameplayFacade.ThisPlayer.CameraOldPosition = GameplayFacade.ThisPlayer.Position;

            DisplayFacade.Camera.Position = position;
            DisplayFacade.Camera.Rotation = rotation;
        }

        // Methods that simulate movement
        private Vector3 PreviewMove(Vector3 movement, float dt)
        {
            // Create a rotate matrix
            Matrix rotate = Matrix.CreateRotationY(GameplayFacade.ThisPlayer.Rotation.Y);
            // Create a movement vector
            Vector3 movementGravity = new Vector3(0, movement.Y, 0);
            movement = Vector3.Transform(movement, rotate);
            movementGravity = Vector3.Transform(movementGravity, rotate);
            // Return the value of camera position + movement vector

            // Testing for the UPCOMING position
            if (GeneralFacade.SceneManager.PlayerCollision.CheckCollision(GameplayFacade.ThisPlayer.Position + movement))
            {
                if (GeneralFacade.SceneManager.PlayerCollision.CheckCollision(GameplayFacade.ThisPlayer.Position + movementGravity))
                {
                    if (GameplayFacade.ThisPlayerPhysics.GravityState == Physics.PhysicalState.Falling)
                    {
                        movement.Y = -GetNearFloorDistance(GameplayFacade.ThisPlayer.Position + new Vector3(movement.X, 0, movement.Z), 0.1f);
                        if (movement.Y > 0)
                        {
                            movement.Y = 0;
                        }
                    }
                    // Hit floor or ceiling
                    GameplayFacade.ThisPlayerPhysics.StabilizeGravity();
                    //GameplayFacade.ThisPlayerPhysics.Reset(LocalPlayerPhysics.AccelerationDirection.Y);
                }
                else if (GameplayFacade.ThisPlayerPhysics.GravityState == Physics.PhysicalState.Walking &&
                    !GeneralFacade.SceneManager.PlayerCollision.CheckCollision(
                        GameplayFacade.ThisPlayer.Position + new Vector3(movement.X, 2, movement.Z)))
                {
                    movement.Y = GetNearFloorDistance(GameplayFacade.ThisPlayer.Position + new Vector3(movement.X, 2, movement.Z), 0.1f);
                }
                // Creating the new movement vector, which will make us 
                // able to have a smooth collision: being able to "slide" on 
                // the wall while colliding
                movement.X = GeneralFacade.SceneManager.PlayerCollision.CheckCollision(
                                    GameplayFacade.ThisPlayer.Position +
                                    new Vector3(movement.X, 0, 0)
                                ) ? 0 : movement.X;
                movement.Y = GeneralFacade.SceneManager.PlayerCollision.CheckCollision(
                                    GameplayFacade.ThisPlayer.Position +
                                    new Vector3(0, movement.Y, 0)
                                ) ? 0 : movement.Y;
                movement.Z = GeneralFacade.SceneManager.PlayerCollision.CheckCollision(
                                    GameplayFacade.ThisPlayer.Position +
                                    new Vector3(0, 0, movement.Z)
                             ) ? 0 : movement.Z;
                return GameplayFacade.ThisPlayer.Position + movement;
            }
            else
            {
                // There isn't any collision, so we just move the user with 
                // the movement he wanted to do
                return GameplayFacade.ThisPlayer.Position + movement;
            }
        }

        public void Move(Vector3 scale, float dt)
        {
            MoveTo(PreviewMove(scale, dt), GameplayFacade.ThisPlayer.Rotation);
        }

        // Movement handling
        public void HandleMovement(float dt)
        {
            GetMouseMovement(dt);

            Acceleration = GetMovementVector(dt);
            MoveVector = Acceleration * dt * GameplayFacade.ThisPlayer.Speed;
            Move(MoveVector, dt);
        }

        private float GetNearFloor(Vector3 pos, float increment)
        {
            while (!GeneralFacade.SceneManager.PlayerCollision.CheckCollision(pos))
            {
                pos.Y -= increment;
            }
            return pos.Y + increment;
        }

        private float GetNearFloorDistance(Vector3 pos, float increment)
        {
            float x = GetNearFloor(pos, increment);
            return pos.Y - x;
        }

    }
}
