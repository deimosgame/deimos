using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Visible)
            {
                return;
            }

            spriteBatch.Draw(
                DisplayFacade.ScreenElementManager.DummyTexture,
                new Rectangle((int)Pos.X, (int)Pos.Y, Width, Height), Color
            );
        }

        public override bool HandleEvent(Rectangle mouse, MouseState mouseState)
        {
            Rectangle rectangle = new Rectangle(
                (int)Pos.X,
                (int)Pos.Y,
                Width,
                Height
            );
            if (mouse.Intersects(rectangle))
            {
                if (LastState != ScreenElement.ElState.Hover
                    && LastState != ScreenElement.ElState.Click)
                {
                    OnHover(this, GeneralFacade.Game);
                    LastState = ScreenElement.ElState.Hover;
                }
                if (mouseState.LeftButton == ButtonState.Pressed
                    && LastState != ScreenElement.ElState.Click)
                {
                    OnClick(this, GeneralFacade.Game);
                    LastState = ScreenElement.ElState.Click;
                    return true;
                }
            }
            else
            {
                if (LastState != ScreenElement.ElState.Out)
                {
                    OnOut(this, GeneralFacade.Game);
                    LastState = ScreenElement.ElState.Out;
                }
            }
            return false;
        }
    }
}
