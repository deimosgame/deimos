using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    class PauseGS : GameStateObj
    {
        public override GameStates GameState
        {
            get { return GameStates.PauseMenu; }
        }

        public override void PreSet()
        {
            GeneralFacade.Config.DebugScreen = false;
            DisplayFacade.ScreenElementManager.IsMouseVisible = true;
            float coeffX = (float)GeneralFacade.Game.GraphicsDevice.Viewport.Width / (float)DisplayFacade.BackgroundMenu.Width;
            float coeffY = (float)GeneralFacade.Game.GraphicsDevice.Viewport.Height / (float)DisplayFacade.BackgroundMenu.Height;
            int imageWidth = DisplayFacade.MenuImages["StartMenuPlay"].Width;

            DisplayFacade.ScreenElementManager.AddImage(
                "PauseMenuBackground",
                0,
                0,
                coeffX,
                coeffY,
                0,
                DisplayFacade.BackgroundMenu,
                null, null, null
            );

            DisplayFacade.ScreenElementManager.AddImage(
                "PauseMenuResume",
                (int)((520 - imageWidth) * coeffX),
                320,
                coeffX,
                coeffY,
                1,
                DisplayFacade.MenuImages["PauseMenuResume"],
                delegate(ScreenElement el, DeimosGame game)
                {
                    GeneralFacade.GameStateManager.Set(new PlayingGS());
                },
                delegate(ScreenElement el, DeimosGame game)
                {
                    ScreenImage t = DisplayFacade.ScreenElementManager.GetImage("PauseMenuResume");
                    t.Image = DisplayFacade.MenuImages["PauseMenuResumeHover"];
                },
                delegate(ScreenElement el, DeimosGame game)
                {
                    ScreenImage t = DisplayFacade.ScreenElementManager.GetImage("PauseMenuResume");
                    t.Image = DisplayFacade.MenuImages["PauseMenuResume"];
                }
            );
            DisplayFacade.ScreenElementManager.AddImage(
                "PauseMenuMain",
                (int)((510 - imageWidth) * coeffX),
                390,
                coeffX,
                coeffY,
                1,
                DisplayFacade.MenuImages["PauseMenuMain"],
                delegate(ScreenElement el, DeimosGame game)
                {
                    if (NetworkFacade.Local)
                    {
                        GeneralFacade.GameStateManager.Set(new StartMenuGS());
                    }
                    else
                    {
                        NetworkFacade.MainHandling.Disconnections.Disco();
                        GeneralFacade.GameStateManager.Set(new StartMenuGS());
                    }
                },
                delegate(ScreenElement el, DeimosGame game)
                {
                    ScreenImage t = DisplayFacade.ScreenElementManager.GetImage("PauseMenuMain");
                    t.Image = DisplayFacade.MenuImages["PauseMenuMainHover"];
                },
                delegate(ScreenElement el, DeimosGame game)
                {
                    ScreenImage t = DisplayFacade.ScreenElementManager.GetImage("PauseMenuMain");
                    t.Image = DisplayFacade.MenuImages["PauseMenuMain"];
                }
            );
        }

        public override void PostUnset()
        {
            DisplayFacade.ScreenElementManager.RemoveImage("PauseMenuBackground");
            DisplayFacade.ScreenElementManager.RemoveImage("PauseMenuResume");
            DisplayFacade.ScreenElementManager.RemoveImage("PauseMenuMain");
        }
    }
}
