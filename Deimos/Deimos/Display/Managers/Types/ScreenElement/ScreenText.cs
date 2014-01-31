using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    class ScreenText : ScreenElement
    {
        public SpriteFont Font
        {
            get;
            private set;
        }
        public string Text
        {
            get;
            private set;
        }
        public Color Color
        {
            get;
            private set;
        }

        public ScreenText(int posX, int posY, int zIndex, 
            SpriteFont font, string text, Color color)
        {
            PosX = posX;
            PosY = posY;
            ZIndex = zIndex;
            Font = font;
            Text = text;
            Color = color;
        }
    }
}
