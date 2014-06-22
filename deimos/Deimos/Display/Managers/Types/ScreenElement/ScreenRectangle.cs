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
            int width, int height, Color color,
            Action<ScreenElement, DeimosGame> onClick = null,
            Action<ScreenElement, DeimosGame> onHover = null,
            Action<ScreenElement, DeimosGame> onOut = null,
            Action<ScreenElement, DeimosGame, Keys> onKeyPress = null)
        {
            Pos = new Vector2(posX, posY);
            ZIndex = zIndex;
            Width = width;
            Height = height;
            Color = color;
            OnClick = onClick;
            OnHover = onHover;
            OnOut = onOut;
            OnKeyPress = onKeyPress;
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
                    if (OnHover != null)
                    {
                        OnHover(this, GeneralFacade.Game);
                    }
                    
                    LastState = ScreenElement.ElState.Hover;
                }
                if (mouseState.LeftButton == ButtonState.Pressed
                    && LastState != ScreenElement.ElState.Click)
                {
                    if (OnClick != null)
                    {
                        OnClick(this, GeneralFacade.Game);
                    }
                    
                    LastState = ScreenElement.ElState.Click;
                    return true;
                }
            }
            else
            {
                if (LastState != ScreenElement.ElState.Out)
                {
                    if (OnOut != null)
                    {
                        OnOut(this, GeneralFacade.Game);
                    }
                    
                    LastState = ScreenElement.ElState.Out;
                }
            }

            Keys[] keys = Keyboard.GetState().GetPressedKeys();
            if (LastState == ScreenElement.ElState.Hover && OnKeyPress != null && keys.Count() > 0)
            {
                OnKeyPress(this, GeneralFacade.Game, keys[0]);
            }

            return false;
        }
    }
}
