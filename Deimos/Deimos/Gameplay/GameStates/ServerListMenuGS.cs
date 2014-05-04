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
            float coeffX = GeneralFacade.Game.GraphicsDevice.Viewport.Width / DisplayFacade.BackgroundMenu.Width;
            float coeffY = GeneralFacade.Game.GraphicsDevice.Viewport.Height / DisplayFacade.BackgroundMenu.Height;
            int imageWidth = DisplayFacade.MenuImages["StartMenuPlay"].Width;

            DisplayFacade.ScreenElementManager.AddImage(
                "StartScreenMenuBackground",
                0,
                0,
                coeffX,
                coeffY,
                0,
                DisplayFacade.BackgroundMenu,
                null, null, null
            );

            DisplayFacade.ScreenElementManager.AddImage(
                "ServerListMenuMain",
                (int)((510 - imageWidth) * coeffX),
                390,
                1,
                1,
                1,
                DisplayFacade.MenuImages["PauseMenuMain"],
                delegate(ScreenElement el, DeimosGame game)
                {
                    GeneralFacade.GameStateManager.Set(new StartMenuGS());
                },
                delegate(ScreenElement el, DeimosGame game)
                {
                    ScreenImage t = DisplayFacade.ScreenElementManager.GetImage("ServerListMenuMain");
                    t.Image = DisplayFacade.MenuImages["PauseMenuMainHover"];
                },
                delegate(ScreenElement el, DeimosGame game)
                {
                    ScreenImage t = DisplayFacade.ScreenElementManager.GetImage("ServerListMenuMain");
                    t.Image = DisplayFacade.MenuImages["PauseMenuMain"];
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
                    NetworkFacade.NetworkHandling.SetConnectivity(
                        (string)item["ip"], " ", 0000);

                    //throw new Exception((string)item["ip"]);
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
            DisplayFacade.ScreenElementManager.RemoveImage("ServerListMenuMain");
            DisplayFacade.ScreenElementManager.RemoveImage("StartScreenMenuBackground");
        }
    }
}
