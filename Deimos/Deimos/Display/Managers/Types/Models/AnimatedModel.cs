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
        Loop,
        Back
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

        private Vector3 startPosition;
        public Vector3 StartPosition
        {
            get { return startPosition; }
            set
            {
                startPosition = value;
                UpdateCoeffs();
            }
        }
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

        public Vector3 startRotation;
        public Vector3 StartRotation
        {
            get { return startRotation; }
            set
            {
                startRotation = value;
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
            CoeffPX = (EndPosition.X - StartPosition.X) / Milliseconds;
            CoeffPY = (EndPosition.Y - StartPosition.Y) / Milliseconds;
            CoeffPZ = (EndPosition.Z - StartPosition.Z) / Milliseconds;

            CoeffRX = (EndRotation.X - StartRotation.X) / Milliseconds;
            CoeffRY = (EndRotation.Y - StartRotation.Y) / Milliseconds;
            CoeffRZ = (EndRotation.Z - StartRotation.Z) / Milliseconds;
        }
    }
}
