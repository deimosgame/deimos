using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    class DeadScreenGS : GameStateObj
    {
        public override GameStates GameState
        {
            get { return GameStates.DeadScreen; }
        }

        public override void PreSet()
        {
            throw new NotImplementedException();
        }

        public override void PostUnset()
        {
            throw new NotImplementedException();
        }
    }
}
