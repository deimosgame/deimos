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
            float coeffX = (float)GeneralFacade.Game.GraphicsDevice.Viewport.Width / (float)DisplayFacade.BackgroundMenu.Width;
            float coeffY = (float)GeneralFacade.Game.GraphicsDevice.Viewport.Height / (float)DisplayFacade.BackgroundMenu.Height;
            int imageWidth = DisplayFacade.MenuImages["StartMenuPlay"].Width;

            DisplayFacade.ScreenElementManager.AddText(
                "ServerListMenuTitle", 
                (int)((540 + 20) * coeffX), 
                100, 
                1, 
                DisplayFacade.TitleFont, 
                "Server list",
                Color.White
            );

            DisplayFacade.ScreenElementManager.AddImage(
                "ServerListMenuBackground",
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
                coeffX,
                coeffY,
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
                        (string)item["ip"], "192.168.0.206", 8462);

                    NetworkFacade.Outgoing.Start();
                    NetworkFacade.Incoming.Start();
                    NetworkFacade.Interpret.Start();

                    NetworkFacade.NetworkHandling.ShakeHands();
                    System.Threading.Thread.Sleep(5000);

                    if (NetworkFacade.NetworkHandling.Handshook)
                    {
                        NetworkFacade.NetworkHandling.Connect();

                        System.Threading.Thread.Sleep(5000);

                        if (NetworkFacade.NetworkHandling.ServerConnected)
                        {
                            GameplayFacade.Objects = new ObjectsList();
                            GameplayFacade.Weapons = new WeaponsList();
                            GameplayFacade.Weapons.Initialise();
                            GameplayFacade.Objects.Initialize();

                            switch (NetworkFacade.MainHandling.Connections.CurrentMap)
                            {
                                case "coolmap":
                                    GeneralFacade.GameStateManager.Set(
                                        new LoadingLevelGS<SceneCompound>()
                                        );
                                    break;
                                default:
                                    GeneralFacade.GameStateManager.Set(
                                        new LoadingLevelGS<SceneDeimos>()
                                        );
                                    break;
                            }

                            GeneralFacade.GameStateManager.Set(new SpawningGS());
                            GeneralFacade.GameStateManager.Set(new PlayingGS());
                        }

                    }
                    else
                    {
                        throw new Exception("Could not connect");
                    }
                };
                serverList.Add(row);
            }

            DisplayFacade.ScreenElementManager.AddTable(
                "ServerListMenuTable",
                (int)((540 + 20) * coeffX), 
                150, 
                1,
                Color.Black, 
                Color.DarkGray,
                Color.DarkGray, 
                Color.White, 
                Color.LightGray, 
                DisplayFacade.TableFont,
                (int)(GeneralFacade.Game.GraphicsDevice.Viewport.Width - ((540 + 20) * coeffX) - 20 - (10 * 6)) / 3, 
                10, 
                new List<string> { "Name", "Map", "Slots" },
                serverList
            );
        }

        public override void PostUnset()
        {
            DisplayFacade.ScreenElementManager.RemoveTable("ServerListMenuTable");
            DisplayFacade.ScreenElementManager.RemoveImage("ServerListMenuMain");
            DisplayFacade.ScreenElementManager.RemoveImage("ServerListMenuBackground");
            DisplayFacade.ScreenElementManager.RemoveText("ServerListMenuTitle");
        }
    }
}
