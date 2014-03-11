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
            protected set;
        }
        public int ZIndex
        {
            get;
            protected set;
        }

        public bool Show
        {
            get;
            set;
        }

        public Action<ScreenElement, DeimosGame> OnHover
        {
            get;
            set;
        }
        public Action<ScreenElement, DeimosGame> OnOut
        {
            get;
            set;
        }
        public Action<ScreenElement, DeimosGame> OnClick
        {
            get;
            set;
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
