using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    public class Player
    {
        public Vector3 Position;
        

        public Vector3 Rotation;
        

        public Vector3 LookAt;
        

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

        public Teams Team;

        public LifeState CurrentLifeState;

        public SpeedState CurrentSpeedState = SpeedState.Walking;

        public Weapon CurrentWeapon;
        public Weapon PreviousWeapon; // for quick-switch purposes

        private uint health;
        public uint Health
        {
            get { return health; }
            set { health = value; }
        }

        public float MaxSprintTime = 5f;
        public float SprintTimer = 0f;

        public float SprintCooldown = 5f;
        public float CooldownTimer = 5f;

        public float WalkSpeed = 20f;
        public float RunSpeed = 50f;
        public float SprintSpeed = 90f;

        public float Speed;

        public int Score = 0;

        public uint ammoPickup = 0; // amount of ammo that is potentially picked up
    }
}
