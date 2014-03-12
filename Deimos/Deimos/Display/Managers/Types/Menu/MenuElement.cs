using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    class MenuElement
    {
        public string Title;
        public int MarginTop;
        public Action<ScreenElement, DeimosGame> ClickEvent;
    }
}
