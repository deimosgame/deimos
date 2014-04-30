using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    class PlayingGS : GameStateObj
    {
        public override GameStates GameState
        {
            get { return GameStates.Playing; }
        }

        public override void PreSet()
        {
            GeneralFacade.Game.IsMouseVisible = false;

            Mouse.SetPosition(GeneralFacade.Game.GraphicsDevice.Viewport.Width / 2,
                    GeneralFacade.Game.GraphicsDevice.Viewport.Height / 2);
        }

        public override void PostUnset()
        {
            //
        }
    }
}
