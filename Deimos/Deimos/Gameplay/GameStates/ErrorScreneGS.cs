using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    class ErrorScreneGS
    {
        public override GameStates GameState
        {
            get { return GameStates.Pause; }
        }

        public override void PreSet()
        {
            GeneralFacade.Game.IsMouseVisible = true;
            float coeffX = (float)GeneralFacade.Game.GraphicsDevice.Viewport.Width / (float)DisplayFacade.BackgroundMenu.Width;
            float coeffY = (float)GeneralFacade.Game.GraphicsDevice.Viewport.Height / (float)DisplayFacade.BackgroundMenu.Height;
            int imageWidth = DisplayFacade.MenuImages["StartMenuPlay"].Width;

        }
    }
}
