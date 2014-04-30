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
            set;
        }
        public string Text
        {
            get;
            set;
        }
        public Color Color
        {
            get;
            set;
        }

        public ScreenText(int posX, int posY, int zIndex, 
            SpriteFont font, string text, Color color)
        {
            Pos = new Vector2(posX, posY);
            ZIndex = zIndex;
            Font = font;
            Text = text;
            Color = color;
        }
    }
}
