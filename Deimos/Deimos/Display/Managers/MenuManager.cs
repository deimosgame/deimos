using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    class MenuManager
    {
        private Dictionary<string, MenuScreen> MenuElements = new Dictionary<string, MenuScreen>();
        private MenuScreen Current;


        public MenuManager()
        {
            //
        }

        public MenuScreen Add(string name)
        {
            MenuScreen nScreen = new MenuScreen(name);
            MenuElements.Add(name, nScreen);
            return nScreen;
        }

        public void Set(string name)
        {
            if (Current != null)
            {
                Current.HideElements();
            }
            Current = MenuElements[name];
            Current.ShowElements();
        }

        public void Hide()
        {
            Current.HideElements();
            Current = null;
        }
    }
}
