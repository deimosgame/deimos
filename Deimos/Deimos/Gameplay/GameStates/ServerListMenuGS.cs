using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    class ServerListMenuGS : GameStateObj
    {
        public override GameStates GameState
        {
            get { return GameStates.ServerListMenu; }
        }

        public override void PreSet()
        {
            GeneralFacade.Game.IsMouseVisible = true;

            DisplayFacade.ScreenElementManager.AddRectangle(
                "ServerListMenuBack",
                50,
                250,
                1,
                200,
                30,
                Color.Green,
                delegate(ScreenElement el, DeimosGame game)
                {
                    GeneralFacade.GameStateManager.Set(new StartMenuGS());
                }
            );

            TableRow row1 = new TableRow();
            row1.Content = new List<string> { "Hello", "World" };

            DisplayFacade.ScreenElementManager.AddTable("serverList", 500, 200, 1,
                Color.Yellow, Color.Red, Color.Gray, Color.Black, Color.Black, DisplayFacade.DebugScreen.Font,
                200, 10, new List<string> { "Column 1", "Column 2" },
                new List<TableRow> { row1, row1, row1, row1, row1, row1, row1, row1, row1, row1, row1, row1 });
        }

        public override void PostUnset()
        {
            DisplayFacade.ScreenElementManager.RemoveTable("serverList");
            DisplayFacade.ScreenElementManager.RemoveRectangle("ServerListMenuBack");
        }
    }
}
