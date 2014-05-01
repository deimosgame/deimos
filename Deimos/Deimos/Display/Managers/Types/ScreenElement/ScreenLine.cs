using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    class ScreenLine : ScreenElement
    {

        public Color Color
        {
            get;
            set;
        }

        public Vector2 Start
        {
            get;
            set;
        }

        public Vector2 End
        {
            get;
            set;
        }

        public ScreenLine(Vector2 start, Vector2 end, int zIndex, Color color)
        {
            ZIndex = zIndex;
            Color = color;
            Start = start;
            End = end;
        }
    }
}
