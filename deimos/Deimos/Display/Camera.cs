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


        // Properties
        public Vector3 Position
        {
            get { return Game.ThisPlayer.Position; }
            set
            {
                Game.ThisPlayer.Position = value;
                UpdateLookAt();
            }
        }

        public Vector3 Rotation
        {
            get { return Game.ThisPlayer.Rotation; }
            set
            {
                Game.ThisPlayer.Rotation = value;
                UpdateLookAt();
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
        public Camera(DeimosGame game, Vector3 position, Vector3 rotation)
        {
            Game = game;

            AspectRatio = 1;

            // Setup projection matrix
            AspectRatio = Game.GraphicsDevice.Viewport.AspectRatio;

            Projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4,
                AspectRatio,
                1.0f,
                1000.0f // Draw distance
            );
        }

        // Update the look at vector
        public void UpdateLookAt()
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
    }
}
