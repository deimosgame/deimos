using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    class StartMenuGS : GameStateObj
    {
        public override GameStates GameState
        {
            get { return GameStates.StartMenu; }
        }

        public override void PreSet()
        {
            GeneralFacade.Game.IsMouseVisible = false;
            DisplayFacade.ScreenElementManager.HandleMouse();

            DisplayFacade.ScreenElementManager.RemoveImage("Intro");
            DisplayFacade.MenuManager.Set("Start");
        }

        public override void PostUnset()
        {
            //
        }
    }
}
