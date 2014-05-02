using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    class ScreenImage : ScreenElement
    {
        public Texture2D Image;
        public float ScaleX;
        public float ScaleY;

        public ScreenImage(int posX, int posY, float scaleX, float scaleY, int zIndex,
            Texture2D image)
        {
            Pos = new Vector2(posX, posY);
            ScaleX = scaleX;
            ScaleY = scaleY;
            ZIndex = zIndex;
            Image = image;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Visible)
            {
                return;
            }
            spriteBatch.Draw(
                Image,
                Pos,
                null,
                Color.White,
                0,
                Vector2.Zero,
                new Vector2(ScaleX, ScaleY),
                SpriteEffects.None,
                0f
            );
        }
    }
}
