using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    class AnimatedModel
    {
        public LevelModel Model;
        public int Milliseconds;
        public int RemainingMilliseconds;

        public Vector3 StartPosition;
        public Vector3 EndPosition;

        public Vector3 StartRotation;
        public Vector3 EndRotation;

        public bool Loop;
    }
}
