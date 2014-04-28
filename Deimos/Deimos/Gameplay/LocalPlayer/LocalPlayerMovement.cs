using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Deimos
{
    class LocalPlayerMovement
    {
        Vector3 MoveVector;
        Vector3 Acceleration;
        Vector3 LastMoveVector;

        public LocalPlayerMovement()
        {
            //
        }

        public Vector3 GetMovementVector(float dt)
        {
            GeneralFacade.Game.ThisPlayer.ks = Keyboard.GetState();
            Vector3 movementVector = Vector3.Zero;

            // Let's handle movement input
            if (GeneralFacade.Game.CurrentPlayingState != DeimosGame.PlayingStates.NoClip)
            {
                if (GeneralFacade.Game.ThisPlayer.ks.IsKeyDown(GeneralFacade.Game.Config.Forward))
                {
                    GeneralFacade.Game.ThisPlayerPhysics.Accelerate(LocalPlayerPhysics.AccelerationDirection.Z);
                    movementVector += Vector3.Backward;
                }
                if (GeneralFacade.Game.ThisPlayer.ks.IsKeyDown(GeneralFacade.Game.Config.Backward))
                {
                    GeneralFacade.Game.ThisPlayerPhysics.Decelerate(LocalPlayerPhysics.AccelerationDirection.Z);
                    movementVector += Vector3.Backward;
                }
                if (GeneralFacade.Game.ThisPlayer.ks.IsKeyDown(GeneralFacade.Game.Config.Left))
                {
                    GeneralFacade.Game.ThisPlayerPhysics.Accelerate(LocalPlayerPhysics.AccelerationDirection.X);
                    movementVector += Vector3.Right;
                }
                if (GeneralFacade.Game.ThisPlayer.ks.IsKeyDown(GeneralFacade.Game.Config.Right))
                {
                    GeneralFacade.Game.ThisPlayerPhysics.Decelerate(LocalPlayerPhysics.AccelerationDirection.X);
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

                //&& GeneralFacade.Game.ThisPlayerPhysics.ShouldResetMovement(LocalPlayerPhysics.AccelerationDirection.Z))
                if (GeneralFacade.Game.ThisPlayer.ks.IsKeyUp(GeneralFacade.Game.Config.Forward)
                    && GeneralFacade.Game.ThisPlayer.ks.IsKeyUp(GeneralFacade.Game.Config.Backward))
                {
                    GeneralFacade.Game.ThisPlayerPhysics.Reset(LocalPlayerPhysics.AccelerationDirection.Z);
                }
                if (GeneralFacade.Game.ThisPlayer.ks.IsKeyUp(GeneralFacade.Game.Config.Left)
                    && GeneralFacade.Game.ThisPlayer.ks.IsKeyUp(GeneralFacade.Game.Config.Right))
                {
                    GeneralFacade.Game.ThisPlayerPhysics.Reset(LocalPlayerPhysics.AccelerationDirection.X);
                }

                    GeneralFacade.Game.ThisPlayerPhysics.ApplyGravity(dt);
                    movementVector.Y = 1;

                LastMoveVector = movementVector;

                // * GeneralFacade.Game.ThisPlayerPhysics.GetMomentum()
                return (movementVector * 
                    GeneralFacade.Game.ThisPlayerPhysics.GetAcceleration());
            }
            else
            {
                if (GeneralFacade.Game.ThisPlayer.ks.IsKeyDown(GeneralFacade.Game.Config.Forward))
                {
                    movementVector += Vector3.Backward;
                }
                if (GeneralFacade.Game.ThisPlayer.ks.IsKeyDown(GeneralFacade.Game.Config.Backward))
                {
                    movementVector += Vector3.Forward;
                }
                if (GeneralFacade.Game.ThisPlayer.ks.IsKeyDown(GeneralFacade.Game.Config.Left))
                {
                    movementVector += Vector3.Right;
                }
                if (GeneralFacade.Game.ThisPlayer.ks.IsKeyDown(GeneralFacade.Game.Config.Right))
                {
                    movementVector += Vector3.Left;
                }
                if (GeneralFacade.Game.ThisPlayer.ks.IsKeyDown(GeneralFacade.Game.Config.Jump))
                {
                    movementVector += Vector3.Up;
                }
                if (GeneralFacade.Game.ThisPlayer.ks.IsKeyDown(GeneralFacade.Game.Config.Crouch))
                {
                    movementVector += Vector3.Down;
                }

                return movementVector;
            }

            
        }

        public void GetMouseMovement(float dt)
        {
            GeneralFacade.Game.ThisPlayer.CurrentMouseState = Mouse.GetState();

            // Handle mouse movement
            float deltaX;
            float deltaY;
            if (GeneralFacade.Game.ThisPlayer.CurrentMouseState != GeneralFacade.Game.ThisPlayer.PreviousMouseState)
            {
                // Cache mouse location
                // We devide by 2 because mouse will be in the center
                deltaX = GeneralFacade.Game.ThisPlayer.CurrentMouseState.X
                    - (GeneralFacade.Game.GraphicsDevice.Viewport.Width / 2);
                deltaY = GeneralFacade.Game.ThisPlayer.CurrentMouseState.Y
                    - (GeneralFacade.Game.GraphicsDevice.Viewport.Height / 2);
           

                GeneralFacade.Game.ThisPlayer.MouseRotationBuffer.X -= 
                    GeneralFacade.Game.Config.MouseSensivity * deltaX * dt;
                GeneralFacade.Game.ThisPlayer.MouseRotationBuffer.Y -= 
                    GeneralFacade.Game.Config.MouseSensivity * deltaY * dt;

                // Limit the user so he can't do an unlimited movement with 
                // his mouse (like a 7683°)
                if (GeneralFacade.Game.ThisPlayer.MouseRotationBuffer.Y < MathHelper.ToRadians(-75.0f))
                {
                    GeneralFacade.Game.ThisPlayer.MouseRotationBuffer.Y = GeneralFacade.Game.ThisPlayer.MouseRotationBuffer.Y -
                        (GeneralFacade.Game.ThisPlayer.MouseRotationBuffer.Y - MathHelper.ToRadians(-75.0f));
                }
                if (GeneralFacade.Game.ThisPlayer.MouseRotationBuffer.Y > MathHelper.ToRadians(75.0f))
                {
                    GeneralFacade.Game.ThisPlayer.MouseRotationBuffer.Y = GeneralFacade.Game.ThisPlayer.MouseRotationBuffer.Y -
                        (GeneralFacade.Game.ThisPlayer.MouseRotationBuffer.Y - MathHelper.ToRadians(75.0f));
                }

                float mouseInverted = (GeneralFacade.Game.Config.MouseInverted) ? 1 : -1;

                DisplayFacade.Camera.Rotation = new Vector3(
                    mouseInverted * MathHelper.Clamp(
                        GeneralFacade.Game.ThisPlayer.MouseRotationBuffer.Y,
                        MathHelper.ToRadians(-75.0f),
                        MathHelper.ToRadians(75.0f)
                    ),
                    MathHelper.WrapAngle(GeneralFacade.Game.ThisPlayer.MouseRotationBuffer.X), // This is so 
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

            GeneralFacade.Game.ThisPlayer.PreviousMouseState = GeneralFacade.Game.ThisPlayer.CurrentMouseState;
        }

        // Set camera position and rotation
        public void MoveTo(Vector3 position, Vector3 rotation)
        {
            // Thanks to the properties set at the beginning, setting up these 
            // values will execute the code inside the property (i.e update our
            // vectors)
            GeneralFacade.Game.ThisPlayer.CameraOldPosition = GeneralFacade.Game.ThisPlayer.Position;

            DisplayFacade.Camera.Position = position;
            DisplayFacade.Camera.Rotation = rotation;
        }

        // Methods that simulate movement
        private Vector3 PreviewMove(Vector3 movement, float dt)
        {
            // Create a rotate matrix
            Matrix rotate = Matrix.CreateRotationY(GeneralFacade.Game.ThisPlayer.Rotation.Y);
            // Create a movement vector
            Vector3 movementGravity = new Vector3(0, movement.Y, 0);
            movement = Vector3.Transform(movement, rotate);
            movementGravity = Vector3.Transform(movementGravity, rotate);
            // Return the value of camera position + movement vector

            // Testing for the UPCOMING position
            if (GeneralFacade.Game.SceneManager.Collision.CheckCollision(GeneralFacade.Game.ThisPlayer.Position + movement))
            {
                if (GeneralFacade.Game.SceneManager.Collision.CheckCollision(GeneralFacade.Game.ThisPlayer.Position + movementGravity))
                {
                    if (GeneralFacade.Game.ThisPlayerPhysics.GravityState == LocalPlayerPhysics.PhysicalState.Falling)
                    {
                        movement.Y = -GetNearFloorDistance(GeneralFacade.Game.ThisPlayer.Position + new Vector3(movement.X, 0, movement.Z), 0.1f);
                        if (movement.Y > 0)
                        {
                            movement.Y = 0;
                        }
                    }
                    // Hit floor or ceiling
                    GeneralFacade.Game.ThisPlayerPhysics.StabilizeGravity();
                    //GeneralFacade.Game.ThisPlayerPhysics.Reset(LocalPlayerPhysics.AccelerationDirection.Y);
                }
                else if (GeneralFacade.Game.ThisPlayerPhysics.GravityState == LocalPlayerPhysics.PhysicalState.Walking &&
                    !GeneralFacade.Game.SceneManager.Collision.CheckCollision(
                        GeneralFacade.Game.ThisPlayer.Position + new Vector3(movement.X, 2, movement.Z)))
                {
                    movement.Y = GetNearFloorDistance(GeneralFacade.Game.ThisPlayer.Position + new Vector3(movement.X, 2, movement.Z), 0.1f);
                }
                // Creating the new movement vector, which will make us 
                // able to have a smooth collision: being able to "slide" on 
                // the wall while colliding
                movement.X = GeneralFacade.Game.SceneManager.Collision.CheckCollision(
                                    GeneralFacade.Game.ThisPlayer.Position +
                                    new Vector3(movement.X, 0, 0)
                                ) ? 0 : movement.X;
                movement.Y = GeneralFacade.Game.SceneManager.Collision.CheckCollision(
                                    GeneralFacade.Game.ThisPlayer.Position +
                                    new Vector3(0, movement.Y, 0)
                                ) ? 0 : movement.Y;
                movement.Z = GeneralFacade.Game.SceneManager.Collision.CheckCollision(
                                    GeneralFacade.Game.ThisPlayer.Position +
                                    new Vector3(0, 0, movement.Z)
                             ) ? 0 : movement.Z;
                return GeneralFacade.Game.ThisPlayer.Position + movement;
            }
            else
            {
                // There isn't any collision, so we just move the user with 
                // the movement he wanted to do
                return GeneralFacade.Game.ThisPlayer.Position + movement;
            }
        }

        public void Move(Vector3 scale, float dt)
        {
            MoveTo(PreviewMove(scale, dt), GeneralFacade.Game.ThisPlayer.Rotation);
        }

        // Movement handling
        public void HandleMovement(float dt)
        {
            GetMouseMovement(dt);

            Acceleration = GetMovementVector(dt);
            MoveVector = Acceleration * dt * GeneralFacade.Game.ThisPlayer.Speed;
            Move(MoveVector, dt);
        }

        private float GetNearFloor(Vector3 pos, float increment)
        {
            while (!GeneralFacade.Game.SceneManager.Collision.CheckCollision(pos))
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
