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

            Name = GeneralFacade.Config.PlayerName;
            Instance = "main";
        }

        public void InitializeInventory(Player.Spec spec)
        {
            Inventory = new WeaponManager();
            GameplayFacade.ThisPlayer.CurrentWeapon = null;
            GameplayFacade.ThisPlayer.PreviousWeapon = null;

            switch (spec)
            {
                case Spec.Soldier:
                    {
                        Inventory.PickupWeapon(GameplayFacade.Weapons.GetWeapon("Pistol"));
                        ammoPickup = CurrentWeapon.m_reservoirAmmo;
                        Inventory.PickupAmmo("Pistol");
                        Inventory.PickupWeapon(GameplayFacade.Weapons.GetWeapon("Assault Rifle"));
                        ammoPickup = CurrentWeapon.m_reservoirAmmo;
                        Inventory.PickupAmmo("Assault Rifle");
                        Inventory.PickupWeapon(GameplayFacade.Weapons.GetWeapon("Bazooka"));
                        ammoPickup = CurrentWeapon.m_reservoirAmmo;
                        Inventory.PickupAmmo("Bazooka");
                    }
                    break;

                case Spec.Agent:
                    {
                        Inventory.PickupWeapon(GameplayFacade.Weapons.GetWeapon("Pistol"));
                        ammoPickup = CurrentWeapon.m_reservoirAmmo;
                        Inventory.PickupAmmo("Pistol");
                        Inventory.PickupWeapon(GameplayFacade.Weapons.GetWeapon("Assault Rifle"));
                        ammoPickup = CurrentWeapon.m_reservoirAmmo;
                        Inventory.PickupAmmo("Assault Rifle");
                        Inventory.PickupWeapon(GameplayFacade.Weapons.GetWeapon("Carver"));
                    }
                    break;

                case Spec.Overwatch:
                    {
                        Inventory.PickupWeapon(GameplayFacade.Weapons.GetWeapon("Assault Rifle"));
                        ammoPickup = CurrentWeapon.m_reservoirAmmo;
                        Inventory.PickupAmmo("Assault Rifle");
                        Inventory.PickupWeapon(GameplayFacade.Weapons.GetWeapon("Arbiter"));
                        Inventory.PickupWeapon(GameplayFacade.Weapons.GetWeapon("Bazooka"));
                        ammoPickup = CurrentWeapon.m_reservoirAmmo;
                        Inventory.PickupAmmo("Bazooka");
                    }
                    break;

                default:
                    break;
            }

            ammoPickup = CurrentWeapon.m_reservoirAmmo;
            Inventory.PickupAmmo(CurrentWeapon.Name);

            GameplayFacade.ThisPlayerDisplay.LoadCurrentWeaponModel();
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
                Name = GeneralFacade.Config.PlayerName;
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

                GameplayFacade.ThisPlayerPhysics.acceleration = Vector3.Zero;
                GameplayFacade.ThisPlayerPhysics.GravityState = LocalPlayerPhysics.PhysicalState.Walking;
                GameplayFacade.ThisPlayerPhysics.Accelerestate = LocalPlayerPhysics.AccelerationState.Still;
                GameplayFacade.ThisPlayerPhysics.timer_gravity = 0f;
                GameplayFacade.ThisPlayerPhysics.initial_velocity = 0f;

                GameplayFacade.ThisPlayerDisplay.UnloadAllWeapons();

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

                        GameplayFacade.ThisPlayerPhysics.JumpVelocity = GeneralFacade.Game.Constants.JumpSoldier;
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

                        GameplayFacade.ThisPlayerPhysics.JumpVelocity = 
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

                        GameplayFacade.ThisPlayerPhysics.JumpVelocity = GeneralFacade.Game.Constants.JumpAgent;
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
                GameplayFacade.ThisPlayer.ammoPickup = 10;
                GameplayFacade.ThisPlayer.Inventory.PickupAmmo(GameplayFacade.ThisPlayer.CurrentWeapon.Name);

                GameplayFacade.ThisPlayer.Inventory.UpdateAmmo();
            }

            if (ks.IsKeyDown(GeneralFacade.Config.ShowDebug))
            {
                GeneralFacade.Config.DebugScreen = !GeneralFacade.Config.DebugScreen;
            }

        }

        
        
        public void HandleInput(GameTime gameTime)
        {
            dt = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f;

            PlayerMovement.HandleMovement(dt);
            PlayerActions.HandleActions(dt);
            PlayerCombat.HandleCombat(dt);
            Stuff(dt);
            GameplayFacade.ThisPlayerDisplay.DisplayCurrentWeapon(GameplayFacade.ThisPlayer.CurrentWeapon);

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
