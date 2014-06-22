using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    public class Constants
    {
        // Class-related constants
          // Health
        public uint HealthSoldier = 100;
        public uint HealthOverwatch = 120;
        public uint HealthAgent = 80;
        public uint HealthCT = 200;
        public uint HealthVictim = 50;
          // Sprinting
        public float MaxSprintSoldier = 5;
        public float SprintCooldownSoldier = 3.5f;
        public float MaxSprintOverwatch = 2;
        public float SprintCooldownOverwatch = 5;
        public float MaxSprintAgent = 7;
        public float SprintCooldownAgent = 2.5f;
        public float MaxSprintCT = 3;
        public float SprintCooldownCT = 2;
        public float MaxSprintVictim = 0;
        public float SprintCooldownVictim = 10;
          // Speed
        public float SpeedSoldier = 28.5f;
        public float SpeedOverwatch = 20.5f;
        public float SpeedAgent = 30f;
        public float SpeedCT = 35;
        public float SpeedVictim = 25;
          // Propulsion Power
        public float JumpSoldier = 4.5f;
        public float JumpOverwatch = 3f;
        public float JumpAgent = 5.3f;
        public float JumpCT = 5;
        public float JumpVictim = 4.5f;
    }
}
