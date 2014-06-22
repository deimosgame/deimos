using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    class ConfigMenuGS : GameStateObj
    {
        public override GameStates GameState
        {
            get { return GameStates.ServerListMenu; }
        }


        public override void PreSet()
        {
            GeneralFacade.SceneManager.SetScene<SceneStartMenu>();

            GeneralFacade.Game.IsMouseVisible = true;
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
                    GeneralFacade.GameStateManager.Set(new StartMenuGS());
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
            DisplayFacade.ScreenElementManager.AddImage(
                "ConfigMenuForward",
                (int)((540 + 20) * coeffX),
                130,
                0.5f,
                0.5f,
                1,
                DisplayFacade.ButtonsImages["DeadScreenGood"],
                delegate(ScreenElement el, DeimosGame game)
                {
                    GameplayFacade.ThisPlayer.Class = Player.Spec.Soldier;
                    GeneralFacade.GameStateManager.Set(new RespawnGS(GameplayFacade.ThisPlayer.NextInstance, false));
                    GeneralFacade.GameStateManager.Set(new PlayingGS());
                },
                delegate(ScreenElement el, DeimosGame game)
                {
                    ScreenImage t = DisplayFacade.ScreenElementManager.GetImage("DeadScreenGood");
                    t.Image = DisplayFacade.ButtonsImages["DeadScreenGoodHover"];
                },
                delegate(ScreenElement el, DeimosGame game)
                {
                    ScreenImage t = DisplayFacade.ScreenElementManager.GetImage("DeadScreenGood");
                    t.Image = DisplayFacade.ButtonsImages["DeadScreenGood"];
                }
            );
        }

        public override void PostUnset()
        {
            DisplayFacade.ScreenElementManager.RemoveText("ConfigMenuTitle");
            DisplayFacade.ScreenElementManager.RemoveText("ConfigMenuSubTitle");
            DisplayFacade.ScreenElementManager.RemoveImage("ConfigMenuBackground");
            DisplayFacade.ScreenElementManager.RemoveImage("ConfigMenuMain");
        }
    }
}
