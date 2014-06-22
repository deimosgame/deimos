using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    class ConfigMenuGS : GameStateObj
    {
        GameStateObj Back;

        public override GameStates GameState
        {
            get { return GameStates.ConfigMenu; }
        }

        public ConfigMenuGS(GameStateObj back)
        {
            Back = back;
        }


        public override void PreSet()
        {
            DisplayFacade.ScreenElementManager.IsMouseVisible = true;
            float coeffX = (float)GeneralFacade.Game.GraphicsDevice.Viewport.Width / (float)DisplayFacade.BackgroundMenu.Width;
            float coeffY = (float)GeneralFacade.Game.GraphicsDevice.Viewport.Height / (float)DisplayFacade.BackgroundMenu.Height;
            int imageWidth = DisplayFacade.MenuImages["StartMenuPlay"].Width;

            DisplayFacade.ScreenElementManager.AddText(
                "ConfigMenuTitle",
                (int)((540 + 20) * coeffX),
                100,
                1,
                DisplayFacade.TitleFont,
                "Config",
                Color.White
            );
            DisplayFacade.ScreenElementManager.AddText(
                "ConfigMenuSubTitle",
                (int)((540 + 20) * coeffX),
                130,
                1,
                DisplayFacade.TableFont,
                "Hover over the key you want to bind and press a key.",
                Color.White
            );

            DisplayFacade.ScreenElementManager.AddImage(
                "ConfigMenuBackground",
                0,
                0,
                coeffX,
                coeffY,
                0,
                DisplayFacade.BackgroundMenu,
                null, null, null
            );

            DisplayFacade.ScreenElementManager.AddImage(
                "ConfigMenuMain",
                (int)((510 - imageWidth) * coeffX),
                390,
                coeffX,
                coeffY,
                1,
                DisplayFacade.MenuImages["PauseMenuMain"],
                delegate(ScreenElement el, DeimosGame game)
                {
                    GeneralFacade.GameStateManager.Set(Back);
                },
                delegate(ScreenElement el, DeimosGame game)
                {
                    ScreenImage t = DisplayFacade.ScreenElementManager.GetImage("ConfigMenuMain");
                    t.Image = DisplayFacade.MenuImages["PauseMenuMainHover"];
                },
                delegate(ScreenElement el, DeimosGame game)
                {
                    ScreenImage t = DisplayFacade.ScreenElementManager.GetImage("ConfigMenuMain");
                    t.Image = DisplayFacade.MenuImages["PauseMenuMain"];
                }
            );




            // Actual key binding:
            #region "Foward"
            DisplayFacade.ScreenElementManager.AddImage(
                "ConfigMenuForward",
                (int)((540 + 20) * coeffX),
                180,
                0.3f,
                0.3f,
                1,
                DisplayFacade.ButtonsImages["ConfigForward"],
                delegate(ScreenElement el, DeimosGame game)
                {
                    //
                },
                delegate(ScreenElement el, DeimosGame game)
                {
                    ScreenImage t = DisplayFacade.ScreenElementManager.GetImage("ConfigMenuForward");
                    t.Image = DisplayFacade.ButtonsImages["ConfigForwardHover"];
                },
                delegate(ScreenElement el, DeimosGame game)
                {
                    ScreenImage t = DisplayFacade.ScreenElementManager.GetImage("ConfigMenuForward");
                    t.Image = DisplayFacade.ButtonsImages["ConfigForward"];
                },
                delegate(ScreenElement el, DeimosGame game, Keys k)
                {
                    GeneralFacade.Config.Forward = k;
                    DisplayFacade.ScreenElementManager.GetText("ConfigMenuForwardText").Text =
                        "Binded to: " + k.ToString();
                }
            );
            DisplayFacade.ScreenElementManager.AddText(
                "ConfigMenuForwardText",
                (int)((540 + 20) * coeffX) + 200,
                180,
                1,
                DisplayFacade.TableFont,
                "Binded to: " + GeneralFacade.Config.Forward.ToString(),
                Color.White
            );
            #endregion
            #region "Backward"
            DisplayFacade.ScreenElementManager.AddImage(
                "ConfigMenuBackward",
                (int)((540 + 20) * coeffX),
                230,
                0.3f,
                0.3f,
                1,
                DisplayFacade.ButtonsImages["ConfigBackward"],
                delegate(ScreenElement el, DeimosGame game)
                {
                    //
                },
                delegate(ScreenElement el, DeimosGame game)
                {
                    ScreenImage t = DisplayFacade.ScreenElementManager.GetImage("ConfigMenuBackward");
                    t.Image = DisplayFacade.ButtonsImages["ConfigBackwardHover"];
                },
                delegate(ScreenElement el, DeimosGame game)
                {
                    ScreenImage t = DisplayFacade.ScreenElementManager.GetImage("ConfigMenuBackward");
                    t.Image = DisplayFacade.ButtonsImages["ConfigBackward"];
                },
                delegate(ScreenElement el, DeimosGame game, Keys k)
                {
                    GeneralFacade.Config.Backward = k;
                    DisplayFacade.ScreenElementManager.GetText("ConfigMenuBackwardText").Text =
                        "Binded to: " + k.ToString();
                }
            );
            DisplayFacade.ScreenElementManager.AddText(
                "ConfigMenuBackwardText",
                (int)((540 + 20) * coeffX) + 200,
                230,
                1,
                DisplayFacade.TableFont,
                "Binded to: " + GeneralFacade.Config.Backward.ToString(),
                Color.White
            );
            #endregion
            #region "left"
            DisplayFacade.ScreenElementManager.AddImage(
                "ConfigMenuLeft",
                (int)((540 + 20) * coeffX),
                280,
                0.3f,
                0.3f,
                1,
                DisplayFacade.ButtonsImages["ConfigLeft"],
                delegate(ScreenElement el, DeimosGame game)
                {
                    //
                },
                delegate(ScreenElement el, DeimosGame game)
                {
                    ScreenImage t = DisplayFacade.ScreenElementManager.GetImage("ConfigMenuLeft");
                    t.Image = DisplayFacade.ButtonsImages["ConfigLeftHover"];
                },
                delegate(ScreenElement el, DeimosGame game)
                {
                    ScreenImage t = DisplayFacade.ScreenElementManager.GetImage("ConfigMenuLeft");
                    t.Image = DisplayFacade.ButtonsImages["ConfigLeft"];
                },
                delegate(ScreenElement el, DeimosGame game, Keys k)
                {
                    GeneralFacade.Config.Left = k;
                    DisplayFacade.ScreenElementManager.GetText("ConfigMenuLeftText").Text =
                        "Binded to: " + k.ToString();
                }
            );
            DisplayFacade.ScreenElementManager.AddText(
                "ConfigMenuLeftText",
                (int)((540 + 20) * coeffX) + 200,
                280,
                1,
                DisplayFacade.TableFont,
                "Binded to: " + GeneralFacade.Config.Left.ToString(),
                Color.White
            );
            #endregion
            #region "right"
            DisplayFacade.ScreenElementManager.AddImage(
                "ConfigMenuRight",
                (int)((540 + 20) * coeffX),
                330,
                0.3f,
                0.3f,
                1,
                DisplayFacade.ButtonsImages["ConfigRight"],
                delegate(ScreenElement el, DeimosGame game)
                {
                    //
                },
                delegate(ScreenElement el, DeimosGame game)
                {
                    ScreenImage t = DisplayFacade.ScreenElementManager.GetImage("ConfigMenuRight");
                    t.Image = DisplayFacade.ButtonsImages["ConfigRightHover"];
                },
                delegate(ScreenElement el, DeimosGame game)
                {
                    ScreenImage t = DisplayFacade.ScreenElementManager.GetImage("ConfigMenuRight");
                    t.Image = DisplayFacade.ButtonsImages["ConfigRight"];
                },
                delegate(ScreenElement el, DeimosGame game, Keys k)
                {
                    GeneralFacade.Config.Right = k;
                    DisplayFacade.ScreenElementManager.GetText("ConfigMenuRightText").Text =
                        "Binded to: " + k.ToString();
                }
            );
            DisplayFacade.ScreenElementManager.AddText(
                "ConfigMenuRightText",
                (int)((540 + 20) * coeffX) + 200,
                330,
                1,
                DisplayFacade.TableFont,
                "Binded to: " + GeneralFacade.Config.Right.ToString(),
                Color.White
            );
            #endregion

            #region "fullscreen"
            DisplayFacade.ScreenElementManager.AddImage(
                "ConfigMenuFullscreen",
                (int)((540 + 20) * coeffX),
                430,
                0.3f,
                0.3f,
                1,
                DisplayFacade.ButtonsImages["ConfigFullscreen"],
                delegate(ScreenElement el, DeimosGame game)
                {
                    GeneralFacade.Game.Graphics.IsFullScreen = !GeneralFacade.Game.Graphics.IsFullScreen;
                    GeneralFacade.Game.Graphics.ApplyChanges();
                },
                delegate(ScreenElement el, DeimosGame game)
                {
                    ScreenImage t = DisplayFacade.ScreenElementManager.GetImage("ConfigMenuFullscreen");
                    t.Image = DisplayFacade.ButtonsImages["ConfigFullscreenHover"];
                },
                delegate(ScreenElement el, DeimosGame game)
                {
                    ScreenImage t = DisplayFacade.ScreenElementManager.GetImage("ConfigMenuFullscreen");
                    t.Image = DisplayFacade.ButtonsImages["ConfigFullscreen"];
                }
            );
            #endregion
        }

        public override void PostUnset()
        {
            DisplayFacade.ScreenElementManager.RemoveText("ConfigMenuTitle");
            DisplayFacade.ScreenElementManager.RemoveText("ConfigMenuSubTitle");
            DisplayFacade.ScreenElementManager.RemoveImage("ConfigMenuBackground");
            DisplayFacade.ScreenElementManager.RemoveImage("ConfigMenuMain");

            DisplayFacade.ScreenElementManager.RemoveImage("ConfigMenuForward");
            DisplayFacade.ScreenElementManager.RemoveText("ConfigMenuForwardText");
            DisplayFacade.ScreenElementManager.RemoveImage("ConfigMenuBackward");
            DisplayFacade.ScreenElementManager.RemoveText("ConfigMenuBackwardText");
            DisplayFacade.ScreenElementManager.RemoveImage("ConfigMenuLeft");
            DisplayFacade.ScreenElementManager.RemoveText("ConfigMenuLeftText");
            DisplayFacade.ScreenElementManager.RemoveImage("ConfigMenuRight");
            DisplayFacade.ScreenElementManager.RemoveText("ConfigMenuRightText");
            DisplayFacade.ScreenElementManager.RemoveImage("ConfigMenuFullscreen");
        }
    }
}
