using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Deimos
{
    public class LocalPlayer : Player
    {

        DeimosGame Game;
        MouseState CurrentMouseState;
        MouseState PreviousMouseState;
        Vector3 MouseRotationBuffer;

        LocalPlayerCollision Collision;
        Vector3 CameraOldPosition;

        public KeyboardState ks;

        public LocalPlayer(DeimosGame game)
        {
            Game = game;
            Collision = new LocalPlayerCollision(8f, 1f, 1f, game);
        }

        private Vector3 GetMovementVector(float dt)
        {
             ks = Keyboard.GetState();

            Vector3 moveVector = Vector3.Zero;
            if (ks.IsKeyDown(Game.Config.Forward))
            {
                moveVector.Z = 1;
            }
            if (ks.IsKeyDown(Game.Config.Backward))
            {
                moveVector.Z = -1;
            }

            if (ks.IsKeyDown(Game.Config.Left))
            {
                moveVector.X = 1;
            }
            if (ks.IsKeyDown(Game.Config.Right))
            {
                moveVector.X = -1;
            }

            if (ks.IsKeyDown(Game.Config.Jump))
            {
                if (Game.ThisPlayerPhysics.State == LocalPlayerPhysics.PhysicalState.Walking)
                {
                    Game.ThisPlayerPhysics.InitiateJump(5f);
                }
            }
            else
            {
                Game.ThisPlayerPhysics.BunnyhopCoeff = 1;
            }

            if (ks.IsKeyDown(Game.Config.Crouch))
            {
                // Crouch
            }

            if (CurrentMouseState.LeftButton == ButtonState.Pressed)
            {
                Game.ThisPlayer.CurrentWeapon.Fire();
            }

            Game.Config.DebugScreen = true;
            if (ks.IsKeyDown(Game.Config.ShowDebug))
            {
                Game.Config.DebugScreen = true;
            }

            moveVector.Y = Game.ThisPlayerPhysics.ApplyGravity(dt);

            return moveVector;
        }

        public void GetMouseMovement(float dt)
        {
            CurrentMouseState = Mouse.GetState();

            // Handle mouse movement
            float deltaX;
            float deltaY;
            if (CurrentMouseState != PreviousMouseState)
            {
                // Cache mouse location
                // We devide by 2 because mouse will be in the center
                deltaX = CurrentMouseState.X
                    - (Game.GraphicsDevice.Viewport.Width / 2);
                deltaY = CurrentMouseState.Y
                    - (Game.GraphicsDevice.Viewport.Height / 2);

                MouseRotationBuffer.X -= Game.Config.MouseSensivity * deltaX * dt;
                MouseRotationBuffer.Y -= Game.Config.MouseSensivity * deltaY * dt;

                // Limit the user so he can't do an unlimited movement with 
                // his mouse (like a 7683°)
                if (MouseRotationBuffer.Y < MathHelper.ToRadians(-75.0f))
                {
                    MouseRotationBuffer.Y = MouseRotationBuffer.Y -
                        (MouseRotationBuffer.Y - MathHelper.ToRadians(-75.0f));
                }
                if (MouseRotationBuffer.Y > MathHelper.ToRadians(75.0f))
                {
                    MouseRotationBuffer.Y = MouseRotationBuffer.Y -
                        (MouseRotationBuffer.Y - MathHelper.ToRadians(75.0f));
                }

                float mouseInverted = (Game.Config.MouseInverted) ? 1 : -1;

                Game.Camera.Rotation = new Vector3(
                    mouseInverted * MathHelper.Clamp(
                        MouseRotationBuffer.Y,
                        MathHelper.ToRadians(-75.0f),
                        MathHelper.ToRadians(75.0f)
                    ),
                    MathHelper.WrapAngle(MouseRotationBuffer.X), // This is so 
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

            PreviousMouseState = CurrentMouseState;
        }

        // Set camera position and rotation
        public void MoveTo(Vector3 position, Vector3 rotation)
        {
            // Thanks to the properties set at the beginning, setting up these 
            // values will execute the code inside the property (i.e update our
            // vectors)
            CameraOldPosition = Position;

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
            if (Collision.CheckCollision(Game.ThisPlayer.Position + movement))
            {
                if (Collision.CheckCollision(Game.ThisPlayer.Position + movementGravity))
                {
                    // Hit floor or ceiling
                    Game.ThisPlayerPhysics.StopGravity();
                    movement.Y = 0;
                }
                else if(Game.ThisPlayerPhysics.State == LocalPlayerPhysics.PhysicalState.Walking &&
                    !Collision.CheckCollision(Game.ThisPlayer.Position + new Vector3(movement.X, 2, movement.Z)))
                {
                    movement.Y = 2;
                }
                // Creating the new movement vector, which will make us 
                // able to have a smooth collision: being able to "slide" on 
                // the wall while colliding
                movement.X = Collision.CheckCollision(Game.ThisPlayer.Position +
                                new Vector3(movement.X, 0, 0)) ? 0 : movement.X;
                movement.Y = Collision.CheckCollision(Game.ThisPlayer.Position +
                                new Vector3(0, movement.Y, 0)) ? 0 : movement.Y;
                movement.Z = Collision.CheckCollision(Game.ThisPlayer.Position +
                                new Vector3(0, 0, movement.Z)) ? 0 : movement.Z;
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

        public void HandleInput(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f;

            GetMouseMovement(dt);

            Vector3 moveVector = GetMovementVector(dt);
            if (moveVector != Vector3.Zero)
            {
                float tempY = moveVector.Y;
                moveVector.Y = 0;
                // Normalize that vector so that we don't move faster diagonally
                if (moveVector != Vector3.Zero) moveVector.Normalize();
                // Now we add in move factor and speed
                moveVector *= dt * Speed * Game.ThisPlayerPhysics.BunnyhopCoeff;
                moveVector.Y = tempY;
                // Move camera!
                Move(moveVector, dt);
            }


        }
    }
}
