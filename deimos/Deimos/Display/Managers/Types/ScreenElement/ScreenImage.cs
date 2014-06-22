using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
            Texture2D image,
            Action<ScreenElement, DeimosGame> onClick = null,
            Action<ScreenElement, DeimosGame> onHover = null,
            Action<ScreenElement, DeimosGame> onOut = null,
            Action<ScreenElement, DeimosGame, Keys> onKeyPress = null)
        {
            Pos = new Vector2(posX, posY);
            ScaleX = scaleX;
            ScaleY = scaleY;
            ZIndex = zIndex;
            Image = image;
            OnClick = onClick;
            OnHover = onHover;
            OnOut = onOut;
            OnKeyPress = onKeyPress;
            if (OnHover != null || OnClick != null || OnOut != null || OnKeyPress != null)
            {
                NoEvent = false;
            }
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

        public override bool HandleEvent(Rectangle mouse, MouseState mouseState)
        {
            if (NoEvent)
            {
                return false;
            }
            Rectangle rectangle = new Rectangle(
                (int)Pos.X,
                (int)Pos.Y,
                (int)(Image.Width * ScaleX),
                (int)(Image.Height * ScaleY)
            );
            if (mouse.Intersects(rectangle))
            {
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
                else
                {
                    if (OnHover != null)
                    {
                        OnHover(this, GeneralFacade.Game);
                    }

                    LastState = ScreenElement.ElState.Hover;
                }
            }
            else
            {
                if (OnOut != null)
                {
                    OnOut(this, GeneralFacade.Game);
                }

                LastState = ScreenElement.ElState.Out;
            }

            Keys[] keys = Keyboard.GetState().GetPressedKeys();
            if (LastState == ScreenElement.ElState.Hover && keys.Count() > 0)
            {
                OnKeyPress(this, GeneralFacade.Game, keys[0]);
            }
            
            return false;
        }
    }
}
