﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    class IntroStartingGS : GameStateObj
    {
        public override GameStates GameState
        {
            get { return GameStates.IntroStarting; }
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
