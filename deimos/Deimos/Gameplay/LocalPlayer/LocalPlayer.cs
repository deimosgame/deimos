﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Deimos
{
    public class LocalPlayer : Player
    {
        DeimosGame Game;

        public WeaponManager Inventory;

        public MouseState CurrentMouseState;
        public MouseState PreviousMouseState;
        public Vector3 MouseRotationBuffer;

        public Vector3 CameraOldPosition;

        public KeyboardState ks;

        LocalPlayerActions PlayerActions;
        LocalPlayerMovement PlayerMovement;
        LocalPlayerCombat PlayerCombat;

        public float dt;

        public LocalPlayer(DeimosGame game)
        {
            Game = game;

            PlayerMovement = new LocalPlayerMovement(game);
            PlayerActions = new LocalPlayerActions(game);
            PlayerCombat = new LocalPlayerCombat(game);

            Name = Game.Config.PlayerName;
            Instance = "main";
        }

        public void InitializeInventory(Player.Spec spec)
        {
            Inventory = new WeaponManager(Game);
            Game.ThisPlayer.CurrentWeapon = null;
            Game.ThisPlayer.PreviousWeapon = null;

            switch (spec)
            {
                case Spec.Soldier:
                    {
                        Inventory.PickupWeapon(Game.Weapons.GetWeapon("Pistol"));
                        ammoPickup = CurrentWeapon.m_reservoirAmmo;
                        Inventory.PickupAmmo(CurrentWeapon);
                        Inventory.PickupWeapon(Game.Weapons.GetWeapon("Assault Rifle"));
                        ammoPickup = CurrentWeapon.m_reservoirAmmo;
                        Inventory.PickupAmmo(CurrentWeapon);
                        Inventory.PickupWeapon(Game.Weapons.GetWeapon("Bazooka"));
                        ammoPickup = CurrentWeapon.m_reservoirAmmo;
                        Inventory.PickupAmmo(CurrentWeapon);
                    }
                    break;

                case Spec.Agent:
                    {
                        Inventory.PickupWeapon(Game.Weapons.GetWeapon("Pistol"));
                        ammoPickup = CurrentWeapon.m_reservoirAmmo;
                        Inventory.PickupAmmo(CurrentWeapon);
                        Inventory.PickupWeapon(Game.Weapons.GetWeapon("Assault Rifle"));
                        ammoPickup = CurrentWeapon.m_reservoirAmmo;
                        Inventory.PickupAmmo(CurrentWeapon);
                    }
                    break;

                case Spec.Overwatch:
                    {
                        Inventory.PickupWeapon(Game.Weapons.GetWeapon("Assault Rifle"));
                        ammoPickup = CurrentWeapon.m_reservoirAmmo;
                        Inventory.PickupAmmo(CurrentWeapon);
                        Inventory.PickupWeapon(Game.Weapons.GetWeapon("Bazooka"));
                        ammoPickup = CurrentWeapon.m_reservoirAmmo;
                        Inventory.PickupAmmo(CurrentWeapon);
                    }
                    break;

                default:
                    break;
            }

            ammoPickup = CurrentWeapon.m_reservoirAmmo;
            Inventory.PickupAmmo(CurrentWeapon);

            Game.ThisPlayerDisplay.LoadCurrentWeaponModel();
        }

        public void PlayerSpawn(Vector3 spawnpoint, Vector3 angle)
        {
            CurrentLifeState = LifeState.Alive;

            Position = spawnpoint;
            Rotation = angle;

            CooldownTimer = SprintCooldown;

            SetStats();
        }

        public void PlayerRespawn(Vector3 respawnlocation, Vector3 angle, string instance)
        {
            if (CurrentLifeState == LifeState.Dead)
            {
                Name = Game.Config.PlayerName;
                Instance = instance;

                CurrentLifeState = LifeState.Alive;

                Position = respawnlocation;
                Rotation = angle;

                CooldownTimer = SprintCooldown;

                SetStats();
            }
        }

        public void PlayerKill()
        {
            if (CurrentLifeState == LifeState.Alive)
            {
                Health = 0;

                Game.ThisPlayerPhysics.acceleration = Vector3.Zero;
                Game.ThisPlayerPhysics.GravityState = LocalPlayerPhysics.PhysicalState.Walking;
                Game.ThisPlayerPhysics.Accelerestate = LocalPlayerPhysics.AccelerationState.Still;
                Game.ThisPlayerPhysics.timer_gravity = 0f;
                Game.ThisPlayerPhysics.initial_velocity = 0f;

                Game.ThisPlayerDisplay.UnloadAllWeapons();

                SprintTimer = 0f;

                CurrentLifeState = LifeState.Dead;

                Score--;
            }
        }

        public void SetStats()
        {
            switch (Class)
            {
                case Spec.Soldier:
                    {
                        m_health = Game.Constants.HealthSoldier;
                        Health = (int)m_health;

                        Speed = Game.Constants.SpeedSoldier;
                        SprintCooldown = Game.Constants.SprintCooldownSoldier;
                        MaxSprintTime = Game.Constants.MaxSprintSoldier;
                    }
                    break;

                case Spec.Overwatch:
                    {
                        m_health = Game.Constants.HealthOverwatch;
                        Health = (int)m_health;

                        Speed = Game.Constants.SpeedOverwatch;
                        SprintCooldown = Game.Constants.SprintCooldownOverwatch;
                        MaxSprintTime = Game.Constants.MaxSprintOverwatch;
                    }
                    break;

                case Spec.Agent:
                    {
                        m_health = Game.Constants.HealthAgent;
                        Health = (int)m_health;

                        Speed = Game.Constants.SpeedAgent;
                        SprintCooldown = Game.Constants.SprintCooldownAgent;
                        MaxSprintTime = Game.Constants.MaxSprintAgent;
                    }
                    break;

                default:
                    break;
            }

            CooldownTimer = MaxSprintTime;
        }

        private void Stuff(float dt)
        {
            // Testing purposes: picking up ammo
            if (ks.IsKeyDown(Keys.O))
            {
                Game.ThisPlayer.ammoPickup = 10;
                Game.ThisPlayer.Inventory.PickupAmmo(Game.ThisPlayer.CurrentWeapon);

                Game.ThisPlayer.Inventory.UpdateAmmo();
            }

            if (ks.IsKeyDown(Game.Config.ShowDebug))
            {
                Game.Config.DebugScreen = !Game.Config.DebugScreen;
            }

        }

        
        
        public void HandleInput(GameTime gameTime)
        {
            dt = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f;

            PlayerMovement.HandleMovement(dt);
            PlayerActions.HandleActions(dt);
            PlayerCombat.HandleCombat(dt);
            Stuff(dt);
            Game.ThisPlayerDisplay.DisplayCurrentWeapon(Game.ThisPlayer.CurrentWeapon);
       }

        public bool FireSprint()
        {
            return PlayerCombat.firesprint;
        }
   }
}
