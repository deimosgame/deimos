using Microsoft.Xna.Framework;
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

        public bool Show
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
            Show = true; // Default value
            OnHover = delegate(ScreenElement el, DeimosGame g) { };
            OnOut = delegate(ScreenElement el, DeimosGame g) { };
            OnClick = delegate(ScreenElement el, DeimosGame g) { };
        }
    }
}
