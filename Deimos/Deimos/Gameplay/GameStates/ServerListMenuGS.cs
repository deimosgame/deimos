using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
            string serverListRequest = HelperFacade.Helpers.FileGetContents(
                "https://akadok.deimos-ga.me"  
            );
            if(serverListRequest == "")
            {
                // network error
                return;
            }
            JArray serverJson = JArray.Parse(serverListRequest);

            foreach (JToken item in serverJson)
            {
                var i = item;
                TableRow row = new TableRow();
                string players = (string)item["players"];
                row.Content = new List<string>() {
                    (string)item["name"],
                    (string)item["map"],
                    players.Split(',').Count() + "/" + (string)item["max_players"]
                };
                // Erenus, edit below so we can connect to a server
                row.OnClick = delegate(ScreenElement el, DeimosGame g) 
                {
                    // Add connection code here
                };
                serverList.Add(row);
            }

            DisplayFacade.ScreenElementManager.AddTable("ServerListMenuTable", 500, 200, 1,
                Color.Black, Color.DarkGray, Color.DarkGray, Color.White, Color.LightGray, DisplayFacade.TableFont,
                250, 10, new List<string> { "Name", "Map", "Slots" },
                serverList);
        }

        public override void PostUnset()
        {
            DisplayFacade.ScreenElementManager.RemoveTable("ServerListMenuTable");
            DisplayFacade.ScreenElementManager.RemoveRectangle("ServerListMenuBack");
        }
    }
}
