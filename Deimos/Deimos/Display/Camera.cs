using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Deimos
{
    public class Camera
    {
        // Atributes
        private DeimosGame Game;

        public Vector3 CameraLookAt;
        public float AspectRatio;

        public Vector3 CameraOldPosition;
        private Vector3 CameraMovement = new Vector3(0, 0, 0);

        private MouseState PreviousMouseState;

        private MainPlayerCollision Collision;


        // Properties
        public Vector3 Position
        {
            get { return Game.ThisPlayer.Position; }
            set
            {
                Game.ThisPlayer.Position = value;
                updateLookAt();
            }
        }

        public Vector3 Rotation
        {
            get { return Game.ThisPlayer.Rotation; }
            set
            {
                Game.ThisPlayer.Rotation = value;
                updateLookAt();
            }
        }

        public Matrix Projection
        {
            get;
            protected set;
        }

        public Matrix View
        {
            get
            {
                return Matrix.CreateLookAt(
                    Game.ThisPlayer.Position, 
                    CameraLookAt, 
                    Vector3.Up
                );
            }
        }

        public Vector3 ViewVector
        {
            get
            {
                Vector3 viewVector = Vector3.Transform(
                    Game.ThisPlayer.Position - CameraLookAt, 
                    Matrix.CreateRotationY(0)
                );
                viewVector.Normalize();
                return viewVector;
            }
        }


        public BoundingFrustum Frustum
        {
            get { return new BoundingFrustum(View * Projection); }
        }

        // Constructor
        public Camera(DeimosGame game, Vector3 position,
            Vector3 rotation)
        {
            Game = game;

            Collision = new MainPlayerCollision(1.2f, 2f, 2f, game);

            AspectRatio = 1;

            // Setup projection matrix
            Projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4,
                AspectRatio,
                1.0f,
                1000.0f // Draw distance
            );

            // Set the camera position and rotation
            MoveTo(position, rotation);


            PreviousMouseState = Mouse.GetState();

            AspectRatio = Game.GraphicsDevice.Viewport.AspectRatio;

            Projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4,
                AspectRatio,
                1.0f,
                1000.0f // Draw distance
            );
        }



        // Set camera position and rotation
        private void MoveTo(Vector3 position, Vector3 rotation)
        {
            // Thanks to the properties set at the beginning, setting up these 
            // values will execute the code inside the property (i.e update our
            // vectors)
            CameraOldPosition = Position;

            Position = position;
            Rotation = rotation;
        }

        // Update the look at vector
        private void updateLookAt()
        {
            // Build a rotation matrix
            Matrix rotationMatrix = Matrix.CreateRotationX(Game.ThisPlayer.Rotation.X) *
                                    Matrix.CreateRotationY(Game.ThisPlayer.Rotation.Y);
            // Build look at offset vector
            Vector3 lookAtOffset = Vector3.Transform(
                Vector3.UnitZ, 
                rotationMatrix
            );
            // Update our camera's look at vector
            CameraLookAt = Game.ThisPlayer.Position + lookAtOffset;
        }

        // Methods that simulate movement
        private Vector3 PreviewMove(Vector3 amount, float dt)
        {
            // Create a rotate matrix
            Matrix rotate = Matrix.CreateRotationY(Game.ThisPlayer.Rotation.Y);
            // Create a movement vector
            Vector3 movement = new Vector3(amount.X, amount.Y, amount.Z);
            movement = Vector3.Transform(movement, rotate);
            // Return the value of camera position + movement vector

            // Testing for the UPCOMING position
            if (Collision.CheckCollision(Game.ThisPlayer.Position + movement)) 
            {
                // Creating the new movement vector, which will make use 
                // able to have a smooth collision: being able to "slide" on 
                // the wall while colliding
                movement = new Vector3(
                    Collision.CheckCollision(Game.ThisPlayer.Position + 
                                new Vector3(movement.X, 0, 0)) ? 0 : movement.X,
                    Collision.CheckCollision(Game.ThisPlayer.Position + 
                                new Vector3(0, movement.Y, 0)) ? 0 : movement.Y,
                    Collision.CheckCollision(Game.ThisPlayer.Position + 
                                new Vector3(0, 0, movement.Z)) ? 0 : movement.Z
                );
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
    }
}
