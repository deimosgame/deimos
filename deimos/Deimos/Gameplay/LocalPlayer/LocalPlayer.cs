using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Deimos
{
    public class LocalPlayer : Player
    {
        public WeaponManager Inventory;

        public MouseState CurrentMouseState;
        public MouseState PreviousMouseState;
        public float previousScrollValue;
        public Vector3 MouseRotationBuffer;

        public Vector3 CameraOldPosition;

        public KeyboardState ks;

        LocalPlayerActions PlayerActions;
        LocalPlayerMovement PlayerMovement;
        LocalPlayerCombat PlayerCombat;

        public float dt;

        public LocalPlayer()
        {
            PlayerMovement = new LocalPlayerMovement();
            PlayerActions = new LocalPlayerActions();
            PlayerCombat = new LocalPlayerCombat();

            Name = GeneralFacade.Game.Config.PlayerName;
            Instance = "main";
        }

        public void InitializeInventory(Player.Spec spec)
        {
            Inventory = new WeaponManager();
            GeneralFacade.Game.ThisPlayer.CurrentWeapon = null;
            GeneralFacade.Game.ThisPlayer.PreviousWeapon = null;

            switch (spec)
            {
                case Spec.Soldier:
                    {
                        Inventory.PickupWeapon(GeneralFacade.Game.Weapons.GetWeapon("Pistol"));
                        ammoPickup = CurrentWeapon.m_reservoirAmmo;
                        Inventory.PickupAmmo("Pistol");
                        Inventory.PickupWeapon(GeneralFacade.Game.Weapons.GetWeapon("Assault Rifle"));
                        ammoPickup = CurrentWeapon.m_reservoirAmmo;
                        Inventory.PickupAmmo("Assault Rifle");
                        Inventory.PickupWeapon(GeneralFacade.Game.Weapons.GetWeapon("Bazooka"));
                        ammoPickup = CurrentWeapon.m_reservoirAmmo;
                        Inventory.PickupAmmo("Bazooka");
                    }
                    break;

                case Spec.Agent:
                    {
                        Inventory.PickupWeapon(GeneralFacade.Game.Weapons.GetWeapon("Pistol"));
                        ammoPickup = CurrentWeapon.m_reservoirAmmo;
                        Inventory.PickupAmmo("Pistol");
                        Inventory.PickupWeapon(GeneralFacade.Game.Weapons.GetWeapon("Assault Rifle"));
                        ammoPickup = CurrentWeapon.m_reservoirAmmo;
                        Inventory.PickupAmmo("Assault Rifle");
                        Inventory.PickupWeapon(GeneralFacade.Game.Weapons.GetWeapon("Carver"));
                    }
                    break;

                case Spec.Overwatch:
                    {
                        Inventory.PickupWeapon(GeneralFacade.Game.Weapons.GetWeapon("Assault Rifle"));
                        ammoPickup = CurrentWeapon.m_reservoirAmmo;
                        Inventory.PickupAmmo("Assault Rifle");
                        Inventory.PickupWeapon(GeneralFacade.Game.Weapons.GetWeapon("Arbiter"));
                        Inventory.PickupWeapon(GeneralFacade.Game.Weapons.GetWeapon("Bazooka"));
                        ammoPickup = CurrentWeapon.m_reservoirAmmo;
                        Inventory.PickupAmmo("Bazooka");
                    }
                    break;

                default:
                    break;
            }

            ammoPickup = CurrentWeapon.m_reservoirAmmo;
            Inventory.PickupAmmo(CurrentWeapon.Name);

            GeneralFacade.Game.ThisPlayerDisplay.LoadCurrentWeaponModel();
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
                Name = GeneralFacade.Game.Config.PlayerName;
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

                GeneralFacade.Game.ThisPlayerPhysics.acceleration = Vector3.Zero;
                GeneralFacade.Game.ThisPlayerPhysics.GravityState = LocalPlayerPhysics.PhysicalState.Walking;
                GeneralFacade.Game.ThisPlayerPhysics.Accelerestate = LocalPlayerPhysics.AccelerationState.Still;
                GeneralFacade.Game.ThisPlayerPhysics.timer_gravity = 0f;
                GeneralFacade.Game.ThisPlayerPhysics.initial_velocity = 0f;

                GeneralFacade.Game.ThisPlayerDisplay.UnloadAllWeapons();

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
                        Name = "Unknown Soldier";
                        m_health = GeneralFacade.Game.Constants.HealthSoldier;
                        Health = (int)m_health;

                        Speed = GeneralFacade.Game.Constants.SpeedSoldier;
                        SprintCooldown = GeneralFacade.Game.Constants.SprintCooldownSoldier;
                        MaxSprintTime = GeneralFacade.Game.Constants.MaxSprintSoldier;

                        GeneralFacade.Game.ThisPlayerPhysics.JumpVelocity = GeneralFacade.Game.Constants.JumpSoldier;
                    }
                    break;

                case Spec.Overwatch:
                    {
                        Name = "Gabe";
                        m_health = GeneralFacade.Game.Constants.HealthOverwatch;
                        Health = (int)m_health;

                        Speed = GeneralFacade.Game.Constants.SpeedOverwatch;
                        SprintCooldown = GeneralFacade.Game.Constants.SprintCooldownOverwatch;
                        MaxSprintTime = GeneralFacade.Game.Constants.MaxSprintOverwatch;

                        GeneralFacade.Game.ThisPlayerPhysics.JumpVelocity = 
                            GeneralFacade.Game.Constants.JumpOverwatch;
                    }
                    break;

                case Spec.Agent:
                    {
                        Name = "Varidar";
                        m_health = GeneralFacade.Game.Constants.HealthAgent;
                        Health = (int)m_health;

                        Speed = GeneralFacade.Game.Constants.SpeedAgent;
                        SprintCooldown = GeneralFacade.Game.Constants.SprintCooldownAgent;
                        MaxSprintTime = GeneralFacade.Game.Constants.MaxSprintAgent;

                        GeneralFacade.Game.ThisPlayerPhysics.JumpVelocity = GeneralFacade.Game.Constants.JumpAgent;
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
                GeneralFacade.Game.ThisPlayer.ammoPickup = 10;
                GeneralFacade.Game.ThisPlayer.Inventory.PickupAmmo(GeneralFacade.Game.ThisPlayer.CurrentWeapon.Name);

                GeneralFacade.Game.ThisPlayer.Inventory.UpdateAmmo();
            }

            if (ks.IsKeyDown(GeneralFacade.Game.Config.ShowDebug))
            {
                GeneralFacade.Game.Config.DebugScreen = !GeneralFacade.Game.Config.DebugScreen;
            }

        }

        
        
        public void HandleInput(GameTime gameTime)
        {
            dt = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f;

            PlayerMovement.HandleMovement(dt);
            PlayerActions.HandleActions(dt);
            PlayerCombat.HandleCombat(dt);
            Stuff(dt);
            GeneralFacade.Game.ThisPlayerDisplay.DisplayCurrentWeapon(GeneralFacade.Game.ThisPlayer.CurrentWeapon);

            previousScrollValue = CurrentMouseState.ScrollWheelValue;
       }

        public bool FireSprint()
        {
            return PlayerCombat.firesprint;
        }

        public string GetTargetWeapon()
        {
            return PlayerCombat.target_weapon;
        }

        public void SetTargetWeapon(string name)
        {
            PlayerCombat.target_weapon = name;
        }
   }
}
