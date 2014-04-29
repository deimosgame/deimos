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
            GeneralFacade.Game.IsMouseVisible = false;
            DisplayFacade.ScreenElementManager.HandleMouse();

            DisplayFacade.MenuManager.Set("Pause");
        }

        public override void PostUnset()
        {
            //
        }
    }
}
