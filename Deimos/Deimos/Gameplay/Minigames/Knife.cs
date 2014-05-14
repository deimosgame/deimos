using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    class Knife : Minigame
    {
        public Knife()
        {
            Type = MinigameType.Knife;
            Name = "knife";
            Map = "";
            TimeLimit = 180000;
            LinearJump = true;
            Falldamage = false;
            Gravity = 9.8f;
            SpeedRate = 1;
        }
    }
}
