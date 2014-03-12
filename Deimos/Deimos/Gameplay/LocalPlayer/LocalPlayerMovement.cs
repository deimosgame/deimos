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
        DeimosGame Game;
        Vector3 MoveVector;
        Vector3 Acceleration;
        Vector3 LastMoveVector;

        public LocalPlayerMovement(DeimosGame game)
        {
            Game = game;
        }

        public Vector3 GetMovementVector(float dt)
        {
            Game.ThisPlayer.ks = Keyboard.GetState();
            Vector3 movementVector = Vector3.Zero;

            // Let's handle movement input
            if (Game.ThisPlayer.ks.IsKeyDown(Game.Config.Forward))
            {
                Game.ThisPlayerPhysics.Accelerate(LocalPlayerPhysics.AccelerationDirection.Z);
                movementVector += Vector3.Backward;
            }
            if (Game.ThisPlayer.ks.IsKeyDown(Game.Config.Backward))
            {
                Game.ThisPlayerPhysics.Decelerate(LocalPlayerPhysics.AccelerationDirection.Z);
                movementVector += Vector3.Backward;
            }
            if (Game.ThisPlayer.ks.IsKeyDown(Game.Config.Left))
            {
                Game.ThisPlayerPhysics.Accelerate(LocalPlayerPhysics.AccelerationDirection.X);
                movementVector += Vector3.Right;
            }
            if (Game.ThisPlayer.ks.IsKeyDown(Game.Config.Right))
            {
                Game.ThisPlayerPhysics.Decelerate(LocalPlayerPhysics.AccelerationDirection.X);
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

            if (Game.ThisPlayer.ks.IsKeyUp(Game.Config.Forward)
                && Game.ThisPlayer.ks.IsKeyUp(Game.Config.Backward)
                && Game.ThisPlayerPhysics.ShouldResetMovement(LocalPlayerPhysics.AccelerationDirection.Z))
            {
                Game.ThisPlayerPhysics.Reset(LocalPlayerPhysics.AccelerationDirection.Z);
            }
            if (Game.ThisPlayer.ks.IsKeyUp(Game.Config.Left)
                && Game.ThisPlayer.ks.IsKeyUp(Game.Config.Right))
            {
                Game.ThisPlayerPhysics.Reset(LocalPlayerPhysics.AccelerationDirection.X);
            }

            if (Game.CurrentPlayingState != DeimosGame.PlayingStates.NoClip)
            {
                Game.ThisPlayerPhysics.ApplyGravity(dt);
                movementVector.Y = 1;
            }

            LastMoveVector = movementVector;

            return movementVector * Game.ThisPlayerPhysics.GetAcceleration();
        }

        public void GetMouseMovement(float dt)
        {
            Game.ThisPlayer.CurrentMouseState = Mouse.GetState();

            // Handle mouse movement
            float deltaX;
            float deltaY;
            if (Game.ThisPlayer.CurrentMouseState != Game.ThisPlayer.PreviousMouseState)
            {
                // Cache mouse location
                // We devide by 2 because mouse will be in the center
                deltaX = Game.ThisPlayer.CurrentMouseState.X
                    - (Game.GraphicsDevice.Viewport.Width / 2);
                deltaY = Game.ThisPlayer.CurrentMouseState.Y
                    - (Game.GraphicsDevice.Viewport.Height / 2);

                Game.ThisPlayer.MouseRotationBuffer.X -= 
                    Game.Config.MouseSensivity * deltaX * dt;
                Game.ThisPlayer.MouseRotationBuffer.Y -= 
                    Game.Config.MouseSensivity * deltaY * dt;

                // Limit the user so he can't do an unlimited movement with 
                // his mouse (like a 7683°)
                if (Game.ThisPlayer.MouseRotationBuffer.Y < MathHelper.ToRadians(-75.0f))
                {
                    Game.ThisPlayer.MouseRotationBuffer.Y = Game.ThisPlayer.MouseRotationBuffer.Y -
                        (Game.ThisPlayer.MouseRotationBuffer.Y - MathHelper.ToRadians(-75.0f));
                }
                if (Game.ThisPlayer.MouseRotationBuffer.Y > MathHelper.ToRadians(75.0f))
                {
                    Game.ThisPlayer.MouseRotationBuffer.Y = Game.ThisPlayer.MouseRotationBuffer.Y -
                        (Game.ThisPlayer.MouseRotationBuffer.Y - MathHelper.ToRadians(75.0f));
                }

                float mouseInverted = (Game.Config.MouseInverted) ? 1 : -1;

                Game.Camera.Rotation = new Vector3(
                    mouseInverted * MathHelper.Clamp(
                        Game.ThisPlayer.MouseRotationBuffer.Y,
                        MathHelper.ToRadians(-75.0f),
                        MathHelper.ToRadians(75.0f)
                    ),
                    MathHelper.WrapAngle(Game.ThisPlayer.MouseRotationBuffer.X), // This is so 
                    // the camera isn't going really fast after some time 
                    // (as we are increasing the speed with time)
                    0
                );

                // Resetting them
                deltaX = 0;
                deltaY = 0;

            }

            // Putting the cursor in the middle of the screen
            Mouse.SetPosition(Game.GraphicsDevice.Viewport.Width / 2,
                Game.GraphicsDevice.Viewport.Height / 2);

            Game.ThisPlayer.PreviousMouseState = Game.ThisPlayer.CurrentMouseState;
        }

        // Set camera position and rotation
        public void MoveTo(Vector3 position, Vector3 rotation)
        {
            // Thanks to the properties set at the beginning, setting up these 
            // values will execute the code inside the property (i.e update our
            // vectors)
            Game.ThisPlayer.CameraOldPosition = Game.ThisPlayer.Position;

            Game.Camera.Position = position;
            Game.Camera.Rotation = rotation;
        }

        // Methods that simulate movement
        private Vector3 PreviewMove(Vector3 movement, float dt)
        {
            // Create a rotate matrix
            Matrix rotate = Matrix.CreateRotationY(Game.ThisPlayer.Rotation.Y);
            // Create a movement vector
            Vector3 movementGravity = new Vector3(0, movement.Y, 0);
            movement = Vector3.Transform(movement, rotate);
            movementGravity = Vector3.Transform(movementGravity, rotate);
            // Return the value of camera position + movement vector

            // Testing for the UPCOMING position
            if (Game.SceneManager.Collision.CheckCollision(Game.ThisPlayer.Position + movement))
            {
                if (Game.SceneManager.Collision.CheckCollision(Game.ThisPlayer.Position + movementGravity))
                {
                    if (Game.ThisPlayerPhysics.GravityState == LocalPlayerPhysics.PhysicalState.Falling)
                    {
                        movement.Y = -GetNearFloorDistance(Game.ThisPlayer.Position + new Vector3(movement.X, 0, movement.Z), 0.1f);
                        if (movement.Y > 0)
                        {
                            movement.Y = 0;
                        }
                    }
                    // Hit floor or ceiling
                    Game.ThisPlayerPhysics.StabilizeGravity();
                    //Game.ThisPlayerPhysics.Reset(LocalPlayerPhysics.AccelerationDirection.Y);
                }
                else if (Game.ThisPlayerPhysics.GravityState == LocalPlayerPhysics.PhysicalState.Walking &&
                    !Game.SceneManager.Collision.CheckCollision(
                        Game.ThisPlayer.Position + new Vector3(movement.X, 2, movement.Z)))
                {
                    movement.Y = GetNearFloorDistance(Game.ThisPlayer.Position + new Vector3(movement.X, 2, movement.Z), 0.1f);
                }
                // Creating the new movement vector, which will make us 
                // able to have a smooth collision: being able to "slide" on 
                // the wall while colliding
                movement.X = Game.SceneManager.Collision.CheckCollision(
                                    Game.ThisPlayer.Position +
                                    new Vector3(movement.X, 0, 0)
                                ) ? 0 : movement.X;
                movement.Y = Game.SceneManager.Collision.CheckCollision(
                                    Game.ThisPlayer.Position +
                                    new Vector3(0, movement.Y, 0)
                                ) ? 0 : movement.Y;
                movement.Z = Game.SceneManager.Collision.CheckCollision(
                                    Game.ThisPlayer.Position +
                                    new Vector3(0, 0, movement.Z)
                             ) ? 0 : movement.Z;
                return Game.ThisPlayer.Position + movement;
            }
            else
            {
                // There isn't any collision, so we just move the user with 
                // the movement he wanted to do
                return Game.ThisPlayer.Position + movement;
            }
        }

        public void Move(Vector3 scale, float dt)
        {
            MoveTo(PreviewMove(scale, dt), Game.ThisPlayer.Rotation);
        }

        // Movement handling
        public void HandleMovement(float dt)
        {
            GetMouseMovement(dt);

            Acceleration = GetMovementVector(dt);
            MoveVector = Acceleration * dt * Game.ThisPlayer.Speed;
            Move(MoveVector, dt);
        }

        private float GetNearFloor(Vector3 pos, float increment)
        {
            while (!Game.SceneManager.Collision.CheckCollision(pos))
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
