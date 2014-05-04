using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    enum AnimationLoop
    {
        None,
        Loop
    }

    class AnimatedModel
    {
        public LevelModel Model;
        private int milliseconds;
        public int Milliseconds
        {
            get { return milliseconds; }
            set
            {
                milliseconds = value;
                RemainingMilliseconds = value;
                UpdateCoeffs();
            }
        }
        public int RemainingMilliseconds;

        private Vector3 endPosition;
        public Vector3 EndPosition
        {
            get { return endPosition; }
            set
            {
                endPosition = value;
                UpdateCoeffs();
            }
        }

        public Vector3 endRotation;
        public Vector3 EndRotation
        {
            get { return endRotation; }
            set
            {
                endRotation = value;
                UpdateCoeffs();
            }
        }

        public AnimationLoop Loop;

        public float CoeffPX;
        public float CoeffPY;
        public float CoeffPZ;

        public float CoeffRX;
        public float CoeffRY;
        public float CoeffRZ;

        private void UpdateCoeffs()
        {
            CoeffPX = (EndPosition.X - Model.Position.X) / Milliseconds;
            CoeffPY = (EndPosition.Y - Model.Position.Y) / Milliseconds;
            CoeffPZ = (EndPosition.Z - Model.Position.Z) / Milliseconds;

            CoeffRX = (EndRotation.X - Model.Rotation.X) / Milliseconds;
            CoeffRY = (EndRotation.Y - Model.Rotation.Y) / Milliseconds;
            CoeffRZ = (EndRotation.Z - Model.Rotation.Z) / Milliseconds;
        }
    }
}
