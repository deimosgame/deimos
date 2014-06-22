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
            GeneralFacade.SceneManager.SetScene<SceneStartMenu>();

            DisplayFacade.ScreenElementManager.IsMouseVisible = true;
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

            TableRow testRow = new TableRow();
            testRow.Content = new List<string>() {
                    "Deimos test server",
                    "compound",
                    "0/16"
            };
            testRow.OnClick = delegate(ScreenElement el, DeimosGame g)
            {
                if (!NetworkFacade.ServerIsLocal)
                {
                    NetworkFacade.NetworkHandling.SetConnectivity(
                            "192.168.75.1", 1518, "192.168.75.51", 8000);
                }

                if (!NetworkFacade.ThreadStart1)
                {
                    NetworkFacade.Outgoing.Start();
                    NetworkFacade.TCPIncoming.Start();
                    NetworkFacade.UDPIncoming.Start();
                    NetworkFacade.Interpret.Start();

                    NetworkFacade.ThreadStart1 = true;
                }

                if (!NetworkFacade.NetworkHandling.Handshook)
                {
                    NetworkFacade.NetworkHandling.ShakeHands();
                    System.Threading.Thread.Sleep(2000);
                }

                    if (NetworkFacade.NetworkHandling.Handshook)
                    {
                        NetworkFacade.NetworkHandling.Connect();

                        System.Threading.Thread.Sleep(2000);

                        if (NetworkFacade.NetworkHandling.ServerConnected)
                        {
                            NetworkFacade.Local = false;
                            GameplayFacade.Objects = new ObjectsList();
                            GameplayFacade.Weapons = new WeaponsList();
                            GameplayFacade.Minigames = new MinigameManager();
                            GameplayFacade.Weapons.Initialise();
                            GameplayFacade.Objects.Initialize();

                            switch (NetworkFacade.MainHandling.Connections.CurrentMap)
                            {
                                case "d_compound":
                                    GeneralFacade.GameStateManager.Set(
                                        new LoadingLevelGS<SceneCompound>(delegate() { })
                                        );
                                    break;
                                case "hl_bootcamp":
                                    GeneralFacade.GameStateManager.Set(
                                        new LoadingLevelGS<SceneDeimos>(delegate() { })
                                        );
                                    break;
                                default:
                                    GeneralFacade.GameStateManager.Set(
                                        new LoadingLevelGS<SceneCompound>(delegate() { })
                                        );
                                    break;
                            }

                            GeneralFacade.GameStateManager.Set(new SpawningGS("main"));
                            GeneralFacade.GameStateManager.Set(new PlayingGS());
                        }

                    }
                    //else
                    //{
                    //    GeneralFacade.GameStateManager.Set(new ErrorScreenGS("Could not connect"));
                    //}

            };
            serverList.Add(testRow);


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
