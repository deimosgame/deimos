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


        public LocalPlayer(DeimosGame game)
        {
            Game = game;
        }

        private Vector3 GetMovementVector()
        {
            // Getting Mouse state
            CurrentMouseState = Mouse.GetState();
            // Let's get user inputs
            KeyboardState ks = Keyboard.GetState();

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
                moveVector.Y = 1;
            }
            if (ks.IsKeyDown(Game.Config.Crouch))
            {
                moveVector.Y = -1;
            }

            return moveVector;
        }

        public void HandleInput(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Getting Mouse state
            CurrentMouseState = Mouse.GetState();
            // Let's get user inputs
            KeyboardState ks = Keyboard.GetState();

            Vector3 moveVector = GetMovementVector();
            if (moveVector != Vector3.Zero)
            {
                // Normalize that vector so that we don't move faster diagonally
                moveVector.Normalize();
                // Now we add in move factor and speed
                moveVector *= dt * Speed;
                // Move camera!
                Game.Camera.Move(moveVector, dt);
            }


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

                float mouseInverted = (Game.Config.MouseInverted == true) ? 1 : -1;

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
    }
}
