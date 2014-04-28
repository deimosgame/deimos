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
        public Vector3 CameraLookAt;
        public float AspectRatio;


        // Properties
        private Vector3 position;
        public Vector3 Position
        {
            get { return position; }
            set
            {
                position = value;
                //GameplayFacade.ThisPlayer.Position = value;
                UpdateLookAt();
            }
        }

        private Vector3 rotation;
        public Vector3 Rotation
        {
            get { return rotation; }
            set
            {
                rotation = value;
                //GameplayFacade.ThisPlayer.Rotation = value;
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
                    Position, 
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
                    Position - CameraLookAt, 
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
        public Camera(Vector3 position, Vector3 rotation)
        {
            AspectRatio = 1;

            // Setup projection matrix
            AspectRatio = GeneralFacade.Game.GraphicsDevice.Viewport.AspectRatio;

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
            Matrix rotationMatrix = Matrix.CreateRotationX(Rotation.X) *
                                    Matrix.CreateRotationY(Rotation.Y);
            // Build look at offset vector
            Vector3 lookAtOffset = Vector3.Transform(
                Vector3.UnitZ, 
                rotationMatrix
            );
            // Update our camera's look at vector
            CameraLookAt = Position + lookAtOffset;
        }
    }
}
