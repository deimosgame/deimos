using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    public class Player
    {
        public string Name;

        public byte Model = 0x00;

        public byte WeaponModel;

        public string Instance;

        public Vector3 Position;

        public Vector3 Rotation;

        public Vector3 LookAt;

        public Vector3 Acceleration;

        public enum Spec
        {
            Overwatch,
            Agent,
            Soldier
        }

        public enum Teams
        {
            Humans,
            Aliens
        }

        public enum LifeState
        {
            Alive,
            Dead
        }

        public enum SpeedState
        {
            Crouching,
            Walking,
            Running,
            Sprinting
        }

        public bool Speedboosted = false;
        public bool Gravityboosted = false;

        public Spec Class = Spec.Soldier;

        public Teams Team = Teams.Humans;

        public LifeState CurrentLifeState = LifeState.Dead;

        public SpeedState CurrentSpeedState = SpeedState.Walking;

        public Weapon CurrentWeapon;
        public Weapon PreviousWeapon; // for quick-switch purposes

        private int health = 1;
        public int Health
        {
            get { return health; }
            set
            {
                if (value > m_health)
                { health = (int)m_health; return; }
                if (value < 0)
                { health = 0; return; }
                health = value;
            }
        }
        protected uint m_health;
        public uint M_Health
        {
            get { return m_health; }
            set { m_health = value; }
        }

        public float MaxSprintTime;
        private float sprintTimer = 0f;
        public float SprintTimer
        {
            get { return sprintTimer; }
            set
            {
                if (value > MaxSprintTime)
                { sprintTimer = MaxSprintTime; return; }
                if (value < 0)
                { sprintTimer = 0; return; }
                sprintTimer = value;
            }
        }

        public float SprintCooldown;
        private float cooldownTimer;
        public float CooldownTimer
        {
            get { return cooldownTimer; }
            set
            {
                if (value > SprintCooldown)
                { cooldownTimer = SprintCooldown; return; }
                if (value < 0)
                { cooldownTimer = 0; return; }
                cooldownTimer = value;
            }
        }
        
        public float Speed;

        public int Score = 0;

        public uint ammoPickup = 0; // amount of ammo that is potentially picked up

        public bool IsAlive()
        {
            if (Health > 0)
            {
                CurrentLifeState = LifeState.Alive;
                return true;
            }
            else
            {
                CurrentLifeState = LifeState.Dead;
                return false;
            }
        }

        public bool IsAtMaxHealth()
        {
            return (Health == (int)m_health);
        }

        public string GetWeaponName()
        {
            switch (WeaponModel)
            {
                case 0x00:
                    return "Carver";
                case 0x01:
                    return "Arbiter";
                case 0x02:
                    return "Pistol";
                case 0x03:
                    return "Assault Rifle";
                case 0x04:
                    return "Bazooka";
                default:
                    return null;
            }
        }

        public string GetModelName()
        {
            switch (Model)
            {
                case 0x00:
                    return "Models/Characters/Vanquish/vanquish";
                default:
                    return "Models/Characters/Vanquish/vanquish";
            }
        }
    }
}
