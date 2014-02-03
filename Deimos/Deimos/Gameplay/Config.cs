using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    class Config
    {
        public string PlayerName = "Unknown Soldier";

        public Keys Forward = Keys.W;
        public Keys Backward = Keys.S;
        public Keys Left = Keys.A;
        public Keys Right = Keys.D;

        public Keys Jump = Keys.Space;
        public Keys Crouch = Keys.LeftControl;

        public Keys ShowDebug = Keys.Tab;

        public float MouseSensivity = 0.15f;
        public bool MouseInverted = false;

        public bool DebugScreen = false;
    }
}
