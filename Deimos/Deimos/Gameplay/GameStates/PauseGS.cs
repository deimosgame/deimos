﻿using Microsoft.Xna.Framework;
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
            get { return GameStates.Pause; }
        }

        public override void PreSet()
        {
            GeneralFacade.Game.IsMouseVisible = true;
            float coeffX = GeneralFacade.Game.GraphicsDevice.Viewport.Width / DisplayFacade.BackgroundMenu.Width;
            float coeffY = GeneralFacade.Game.GraphicsDevice.Viewport.Height / DisplayFacade.BackgroundMenu.Height;
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
                1,
                1,
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
                1,
                1,
                1,
                DisplayFacade.MenuImages["PauseMenuMain"],
                delegate(ScreenElement el, DeimosGame game)
                {
                    if (!NetworkFacade.IsMultiplayer)
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
