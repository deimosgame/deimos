using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Deimos
{
    class DebugScreen
    {
        DeimosGame Game;
        public DebugScreen(DeimosGame game)
        {
            Game = game;

            Game.ScreenElementManager.AddRectangle("test", 0, 0, 1, 20, 20, Color.Red);
        }

        public void Draw()
        {
            
        }
    }
}
