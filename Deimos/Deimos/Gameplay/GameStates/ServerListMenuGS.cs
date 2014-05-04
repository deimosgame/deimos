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

            List<TableRow> serverList = new List<TableRow>();


            DisplayFacade.ScreenElementManager.AddTable("ServerListMenuTable", 500, 200, 1,
                Color.Yellow, Color.Red, Color.Gray, Color.Black, Color.Black, DisplayFacade.DebugScreen.Font,
                200, 10, new List<string> { "Name", "IP", "Map", "Slots" },
                serverList);
        }

        public override void PostUnset()
        {
            DisplayFacade.ScreenElementManager.RemoveTable("ServerListMenuTable");
            DisplayFacade.ScreenElementManager.RemoveRectangle("ServerListMenuBack");
        }
    }
}
