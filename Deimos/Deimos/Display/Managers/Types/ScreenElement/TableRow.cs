using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    class TableRow
    {
        public List<string> Content = new List<string>();

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

        public TableRow()
        {
            OnClick = delegate(ScreenElement el, DeimosGame g) { };
        }
    }
}
