using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    abstract class ScreenElement
    {
        public Vector2 Pos
        {
            get;
            set;
        }
        public int ZIndex
        {
            get;
            set;
        }

        public bool Visible
        {
            get;
            set;
        }

        private Action<ScreenElement, DeimosGame> onHover;
        public Action<ScreenElement, DeimosGame> OnHover
        {
            get { return onHover; }
            set
            {
                if (value != null)
                {
                    onHover = value;
                }
            }
        }
        private Action<ScreenElement, DeimosGame> onOut;
        public Action<ScreenElement, DeimosGame> OnOut
        {
            get { return onOut; }
            set
            {
                if (value != null)
                {
                    onOut = value;
                }
            }
        }
        private Action<ScreenElement, DeimosGame> onClick;
        public Action<ScreenElement, DeimosGame> OnClick
        {
            get { return onClick; }
            set
            {
                if (value != null)
                {
                    onClick = value;
                }
            }
        }
        private Action<ScreenElement, DeimosGame, Keys> onKeyPress;
        public Action<ScreenElement, DeimosGame, Keys> OnKeyPress
        {
            get { return onKeyPress; }
            set
            {
                if (value != null)
                {
                    onKeyPress = value;
                }
            }
        }

        protected bool NoEvent = true;

        public enum ElState
        {
            Hover,
            Out,
            Click
        }
        public ElState LastState
        {
            get;
            set;
        }

        protected ScreenElement()
        {
            Visible = true; // Default value
        }

        public abstract void Draw(SpriteBatch spriteBatch);

        public virtual bool HandleEvent(Rectangle mouse, MouseState mouseState)
        {
            return false;
        }
    }
}
