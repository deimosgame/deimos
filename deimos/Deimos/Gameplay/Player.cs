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

        public Spec Class = Spec.Soldier;

        public Teams Team = Teams.Humans;

        public LifeState CurrentLifeState = LifeState.Dead;

        public SpeedState CurrentSpeedState = SpeedState.Walking;

        public Weapon CurrentWeapon;
        public Weapon PreviousWeapon; // for quick-switch purposes

        private int health;
        public int Health
        {
            get { return health; }
            set
            {
                if (value > m_health)
                { health = m_health; return; }
                if (value < 0)
                { health = 0; return; }
                health = value;
            }
        }
        private int m_health;

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

        public void PlayerSpawn(Vector3 spawnpoint, Vector3 angle)
        {
            CurrentLifeState = LifeState.Alive;

            Position = spawnpoint;
            Rotation = angle;

            SetStats();
        }

        public void PlayerRespawn(Vector3 respawnlocation, Vector3 angle)
        {
            if (CurrentLifeState == LifeState.Dead)
            {
                CurrentLifeState = LifeState.Alive;

                Position = respawnlocation;
                Rotation = angle;

                SetStats();
            }
        }

        public void PlayerKill()
        {
            if (CurrentLifeState == LifeState.Alive)
            {
                Health = 0;
                CurrentLifeState = LifeState.Dead;

                Score--;
            }
        }

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

        public void SetStats()
        {
            switch (Class)
            {
                case Spec.Soldier:
                    {
                        m_health = 100;
                        Health = m_health;

                        Speed = 28.5f;
                        SprintCooldown = 3.5f;
                        CooldownTimer = 5f;
                        MaxSprintTime = 5f;
                    }
                    break;

                case Spec.Agent:
                    {
                        m_health = 80;
                        Health = m_health;

                        Speed = 35f;
                        SprintCooldown = 2.5f;
                        CooldownTimer = 7f;
                        MaxSprintTime = 7f;
                    }
                    break;

                case Spec.Overwatch:
                    {
                        m_health = 120;
                        Health = m_health;

                        Speed = 20.5f;
                        SprintCooldown = 5f;
                        CooldownTimer = 2f;
                        MaxSprintTime = 2f;
                    }
                    break;

                default:
                    break;
            }
        }
    }
}
