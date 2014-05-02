using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    class IntroGS : GameStateObj
    {
        public override GameStates GameState
        {
            get { return GameStates.Intro;  }
        }

        public override void PreSet()
        {
            DisplayFacade.ScreenElementManager.AddImage("Intro", 0, 0, 1, 1, 1, null);
        }

        public override void PostUnset()
        {
            DisplayFacade.ScreenElementManager.RemoveImage("Intro");
        }
    }
}
