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
            get { return GameStates.Pause; }
        }

        public override void PreSet()
        {
            GeneralFacade.Game.IsMouseVisible = true;
            DisplayFacade.ScreenElementManager.HandleMouse();

            DisplayFacade.ScreenElementManager.AddRectangle(
                "PauseMenuPlay",
                50,
                200,
                1,
                30,
                200,
                Color.Red,
                delegate(ScreenElement el, DeimosGame game)
                {
                    GeneralFacade.GameStateManager.Set(new PlayingGS());
                }
            );
            DisplayFacade.ScreenElementManager.AddRectangle(
                "PauseMenuExit",
                50,
                250,
                1,
                30,
                200,
                Color.Green,
                delegate(ScreenElement el, DeimosGame game)
                {
                    GeneralFacade.Game.Exit();
                }
            );
        }

        public override void PostUnset()
        {
            DisplayFacade.ScreenElementManager.RemoveRectangle("PauseMenuPlay");
            DisplayFacade.ScreenElementManager.RemoveRectangle("PauseMenuExit");
        }
    }
}
