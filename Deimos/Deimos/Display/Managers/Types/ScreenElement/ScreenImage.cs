using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    class ScreenImage : ScreenElement
    {
        public Texture2D Image
        {
            get;
            private set;
        }

        public ScreenImage(int posX, int posY, int zIndex,
            Texture2D image)
        {
            PosX = posX;
            PosY = posY;
            ZIndex = zIndex;
            Image = image;
        }
    }
}
