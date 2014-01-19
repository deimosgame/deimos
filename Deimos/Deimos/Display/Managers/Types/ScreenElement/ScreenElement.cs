using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    abstract class ScreenElement
    {
        public int PosX
        {
            get;
            protected set;
        }
        public int PosY
        {
            get;
            protected set;
        }
        public int ZIndex
        {
            get;
            protected set;
        }
    }
}
