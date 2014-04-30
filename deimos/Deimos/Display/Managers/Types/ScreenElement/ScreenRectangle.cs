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
            set;
        }
        public int Height
        {
            get;
            set;
        }
        public Color Color
        {
            get;
            set;
        }

        public ScreenRectangle(int posX, int posY, int zIndex, 
            int width, int height, Color color)
        {
            Pos = new Vector2(posX, posY);
            ZIndex = zIndex;
            Width = width;
            Height = height;
            Color = color;
        }
    }
}
