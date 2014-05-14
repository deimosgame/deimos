using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    class Minigame
    {
        public enum MinigameType
        {
            Knife
        }

        // General properties
        public MinigameType Type;
        public string Name;
        public string Map;
        public float TimeLimit;
        
        // Physics properties
        public float Gravity;
        public float SpeedRate;

        // Gameplay properties
        public bool Falldamage;
        public bool LinearJump;

        // General Methods
        public void Load()
        {

        }
        
        public void Terminate()
        {

        }
    }
}
