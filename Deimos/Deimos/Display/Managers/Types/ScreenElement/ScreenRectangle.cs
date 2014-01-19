using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    class ScreenRectangle : ScreenElement
    {
        public int Width
        {
            get;
            private set;
        }
        public int Height
        {
            get;
            private set;
        }
        public Color Color
        {
            get;
            private set;
        }

        public ScreenRectangle(int posX, int posY, int zIndex, 
            int width, int height, Color color)
        {
            PosX = posX;
            PosY = posY;
            ZIndex = zIndex;
            Width = width;
            Height = height;
            Color = color;
        }
    }
}
