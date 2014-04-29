using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    abstract class GameStateObj
    {
        public virtual GameStates GameState
        {
            get { return GameStates.Playing; }
        }

        public abstract void PreSet();
        public abstract void PostUnset();
    }
}
