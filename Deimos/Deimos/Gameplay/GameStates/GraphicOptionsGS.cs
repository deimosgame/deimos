using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    class GraphicOptionsGS : GameStateObj
    {
        public override GameStates GameState
        {
            get { return GameStates.GraphicOptions; }
        }

        public override void PreSet()
        {
            //
        }

        public override void PostUnset()
        {
            //
        }
    }
}
