using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

        public int LineWidth
        {
            get;
            set;
        }

        public ScreenLine(Vector2 start, Vector2 end, int zIndex, Color color, int width = 1)
        {
            ZIndex = zIndex;
            Color = color;
            Start = start;
            End = end;
            LineWidth = width;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Visible)
            {
                return;
            }

            Vector2 edge = End - Start;
            // calculate angle to rotate line
            float angle =
                (float)Math.Atan2(edge.Y, edge.X);


            spriteBatch.Draw(DisplayFacade.ScreenElementManager.DummyTexture,
                new Rectangle(// rectangle defines shape of line and position of start of line
                    (int)Start.X,
                    (int)Start.Y,
                    (int)edge.Length(), //sb will strech the texture to fill this rectangle
                    LineWidth), //width of line, change this to make thicker line
                null,
                Color, //colour of line
                angle,     //angle of line (calulated above)
                new Vector2(0, 0), // point in line about which to rotate
                SpriteEffects.None,
                0);
        }
    }
}
