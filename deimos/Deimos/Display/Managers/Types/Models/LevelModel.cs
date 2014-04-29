using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    public class LevelModel
    {
        private Vector3 position;
        public Vector3 Position
        {
            get { return position; }
            set {
                position = value;
                UpdateMatrix();
            }
        }
        private float scale;
        public float Scale
        {
            get { return scale; }
            set {
                scale = value;
                UpdateMatrix();
            }
        }
        private Vector3 rotation;
        public Vector3 Rotation
        {
            get { return rotation; }
            set
            {
                rotation = value;
                UpdateMatrix();
            }
        }
        public Matrix WorldMatrix
        {
            get;
            set;
        }
        public CollisionType CollisionDetection
        {
            get;
            set;
        }
        public CollidableModel.CollidableModel CollisionModel
        {
            get;
            set;
        }
        public enum CollisionType
        {
            None,
            Accurate
        }

        private void UpdateMatrix()
        {
            WorldMatrix = Matrix.CreateScale(Scale) *
                Matrix.CreateRotationX(Rotation.X) *
                Matrix.CreateRotationY(Rotation.Y) *
                Matrix.CreateRotationZ(Rotation.Z) *
                Matrix.CreateTranslation(Position);
        }

        public bool show = true;
    }
}
