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
        DeimosGame Game;
        MouseState CurrentMouseState;
        MouseState PreviousMouseState;
        Vector3 MouseRotationBuffer;

        Vector3 CameraOldPosition;

        public WeaponManager Inventory;

        public KeyboardState ks;

        float w_switch_timer = 0f;

        public LocalPlayer(DeimosGame game)
        {
            Game = game;
        }

        public void InitializeInventory()
        {
            Inventory = Game.WeaponManager;

            // Let's give them the default pistol ! :P
            Weapon Pistol = new Weapon(
                Game,
                "Pistol", 
                0, 0.2f, 7, 46, 14, 1.3f, 15, 30, 2f, 250f
            );

            Inventory.PickupWeapon(Pistol);

            Weapon Rifle = new Weapon(
                Game,
                "Rifle",
                2, 0.1f, 31, 150, 60, 2.2f, 25, 40, 2f, 500f
            );

            Inventory.PickupWeapon(Rifle);
            Inventory.SetCurrentWeapon(Pistol);
            Inventory.SetPreviousWeapon(Rifle);
        }

        private Vector3 GetMovementVector(float dt)
        {
            ks = Keyboard.GetState();

            // Let's update firing timer if necessary
            if (Game.ThisPlayer.CurrentWeapon.fireTimer < 
                Game.ThisPlayer.CurrentWeapon.FiringRate)
            {
                Game.ThisPlayer.CurrentWeapon.fireTimer += dt;
            }

            // Let's increment the reloading timer if reloading
            if (Game.ThisPlayer.CurrentWeapon.State ==
                Weapon.WeaponState.Reloading)
            { 
                Game.ThisPlayer.CurrentWeapon.reloadTimer += dt; 
            }

            // Let's increase weapon switch timer if switching
            if (Game.ThisPlayer.CurrentWeapon.State == Weapon.WeaponState.Switching)
            {
                if (w_switch_timer < Game.ThisPlayer.Inventory.GetSwitchTime(Game.ThisPlayer.PreviousWeapon))
                {
                    w_switch_timer += dt;
                }
                else
                {
                    Game.ThisPlayer.Inventory.QuickSwitch(
                        Game.ThisPlayer.CurrentWeapon,
                        Game.ThisPlayer.PreviousWeapon);
                    w_switch_timer = 0f;
                }
            }

            // Let's increment the sprint timer if sprinting,
            // and if not, let's cool stuff down
            if (Game.ThisPlayer.CurrentSpeedState ==
                SpeedState.Sprinting && ks.IsKeyDown(Game.Config.Forward))
            {
                Game.ThisPlayer.SprintTimer += dt;
            }
            else
            {
                // cooling down sprint if not already
                if (Game.ThisPlayer.CooldownTimer < Game.ThisPlayer.SprintCooldown)
                {
                    Game.ThisPlayer.CooldownTimer += dt;
                }

                // setting the sprint timer
                if (Game.ThisPlayer.SprintTimer > 0f)
                {
                    float cd = 0f;

                    if (Game.ThisPlayer.CurrentSpeedState == SpeedState.Running)
                    {
                        cd = dt / 2f;
                    }
                    else if (Game.ThisPlayer.CurrentSpeedState == SpeedState.Walking)
                    {
                        cd = dt;
                    }

                    Game.ThisPlayer.SprintTimer -= cd;
                }
            }

                // Let's handle walkstate resets
                if (ks.IsKeyUp(Game.Config.Walk) && 
                   (ks.IsKeyUp(Game.Config.Sprint)) &&
                   (ks.IsKeyUp(Game.Config.Crouch)))
                {
                    Game.ThisPlayer.CurrentSpeedState = SpeedState.Running;
                }

                // Let's handle walkstates and speed settings
                switch (Game.ThisPlayer.CurrentSpeedState)
                {
                    case SpeedState.Running:
                        Game.ThisPlayer.Speed = RunSpeed;
                        break;

                    case SpeedState.Sprinting:
                        if (ks.IsKeyDown(Game.Config.Forward))
                        {
                            if (Game.ThisPlayer.SprintTimer < Game.ThisPlayer.MaxSprintTime)
                            {
                                Game.ThisPlayer.Speed = SprintSpeed;
                            }
                            else
                            {
                                Game.ThisPlayer.CurrentSpeedState = SpeedState.Running;
                                Game.ThisPlayer.Speed = RunSpeed;
                                Game.ThisPlayer.SprintTimer = 0f;
                                Game.ThisPlayer.CooldownTimer = 0f;
                            }
                        }
                        else
                        {
                            Game.ThisPlayer.CurrentSpeedState = SpeedState.Running;
                            Game.ThisPlayer.Speed = RunSpeed;
                        }
                        break;

                    case SpeedState.Walking:
                        Game.ThisPlayer.Speed = WalkSpeed;
                        break;
                }

                // Let's handle input
                Vector3 moveVector = Vector3.Zero;
                if (ks.IsKeyDown(Game.Config.Forward))
                {
                    moveVector.Z = 1;
                }
                if (ks.IsKeyDown(Game.Config.Backward))
                {
                    moveVector.Z = -1;
                }

                if (ks.IsKeyDown(Game.Config.Left))
                {
                    moveVector.X = 1;
                }
                if (ks.IsKeyDown(Game.Config.Right))
                {
                    moveVector.X = -1;
                }

                // Jump
                if (ks.IsKeyDown(Game.Config.Jump))
                {
                    if (Game.CurrentPlayingState == DeimosGame.PlayingStates.NoClip)
                    {
                        moveVector.Y = 1;
                    }

                    else
                    {
                        if (Game.ThisPlayerPhysics.State ==
                        LocalPlayerPhysics.PhysicalState.Walking)
                        {
                            if (Game.ThisPlayer.CurrentSpeedState == SpeedState.Sprinting)
                            {
                                Game.ThisPlayer.CurrentSpeedState = SpeedState.Running;
                            }
                            Game.ThisPlayerPhysics.InitiateJump(4.3f);
                        }
                    }
                }

                // Crouch
                if (ks.IsKeyDown(Game.Config.Crouch))
                {
                    if (Game.CurrentPlayingState == DeimosGame.PlayingStates.NoClip)
                    {
                        moveVector.Y = -1;
                    }
                    else if (Game.ThisPlayerPhysics.State ==
                        LocalPlayerPhysics.PhysicalState.Walking &&
                    Game.ThisPlayer.CurrentSpeedState != SpeedState.Sprinting)
                    {
                        Game.ThisPlayer.CurrentSpeedState = SpeedState.Crouching;
                    }
                }

                // Initiate sprint and sprint timer
                if (ks.IsKeyDown(Game.Config.Sprint) &&
                    Game.ThisPlayerPhysics.State ==
                    LocalPlayerPhysics.PhysicalState.Walking &&
                    Game.ThisPlayer.CooldownTimer >= Game.ThisPlayer.SprintCooldown &&
                    ks.IsKeyDown(Game.Config.Forward))
                {
                    Game.ThisPlayer.CurrentSpeedState = SpeedState.Sprinting;
                }

                // Walk
                if (ks.IsKeyDown(Game.Config.Walk))
                {
                    Game.ThisPlayer.CurrentSpeedState = SpeedState.Walking;
                }

                // Fire Weapon
                if (CurrentMouseState.LeftButton == ButtonState.Pressed &&
                    Game.ThisPlayer.CurrentWeapon.State !=
                    Weapon.WeaponState.Reloading &&
                    Game.ThisPlayer.CurrentSpeedState != SpeedState.Sprinting)
                {
                    if (Game.ThisPlayer.CurrentWeapon.fireTimer >=
                        Game.ThisPlayer.CurrentWeapon.FiringRate)
                    {
                        Game.ThisPlayer.CurrentWeapon.Fire();

                        // reloading if no bullet left after shot
                        Game.ThisPlayer.Inventory.UpdateAmmo();
                    }
                }

                // Initiate Reload timer
                if (ks.IsKeyDown(Game.Config.Reload) &&
                    Game.ThisPlayer.CurrentWeapon.State ==
                    Weapon.WeaponState.AtEase &&
                    Game.ThisPlayer.CurrentWeapon.IsReloadable() &&
                    Game.ThisPlayer.CurrentSpeedState != SpeedState.Sprinting)
                {
                    Game.ThisPlayer.CurrentWeapon.State =
                        Weapon.WeaponState.Reloading;
                }

                // Reload when timer satisfied
                if (Game.ThisPlayer.CurrentWeapon.State ==
                    Weapon.WeaponState.Reloading &&
                    Game.ThisPlayer.CurrentWeapon.reloadTimer >=
                    Game.ThisPlayer.CurrentWeapon.timeToReload)
                {
                    Game.ThisPlayer.Inventory.Reload();
                }

                // Quick-Switch weapon: timer initiate
                if (ks.IsKeyDown(Game.Config.QuickSwitch) &&
                    Game.ThisPlayer.CurrentSpeedState != SpeedState.Sprinting &&
                    Game.ThisPlayer.CurrentWeapon.State == Weapon.WeaponState.AtEase)
                {
                    Game.ThisPlayer.CurrentWeapon.State = Weapon.WeaponState.Switching;
                }

                // Testing purposes: picking up ammo
                if (ks.IsKeyDown(Keys.O))
                {
                    Game.ThisPlayer.ammoPickup = 10;
                    Game.ThisPlayer.Inventory.PickupAmmo(Game.ThisPlayer.CurrentWeapon);

                    Game.ThisPlayer.Inventory.UpdateAmmo();
                }
            
                Game.Config.DebugScreen = true;

                if (ks.IsKeyDown(Game.Config.ShowDebug))
                {
                    Game.Config.DebugScreen = true;
                }

                if (Game.CurrentPlayingState != DeimosGame.PlayingStates.NoClip)
                {
                    moveVector.Y = Game.ThisPlayerPhysics.ApplyGravity(dt);
                }

                return moveVector;
            
        }

        public void GetMouseMovement(float dt)
        {
            CurrentMouseState = Mouse.GetState();

            // Handle mouse movement
            float deltaX;
            float deltaY;
            if (CurrentMouseState != PreviousMouseState)
            {
                // Cache mouse location
                // We devide by 2 because mouse will be in the center
                deltaX = CurrentMouseState.X
                    - (Game.GraphicsDevice.Viewport.Width / 2);
                deltaY = CurrentMouseState.Y
                    - (Game.GraphicsDevice.Viewport.Height / 2);

                MouseRotationBuffer.X -= Game.Config.MouseSensivity * deltaX * dt;
                MouseRotationBuffer.Y -= Game.Config.MouseSensivity * deltaY * dt;

                // Limit the user so he can't do an unlimited movement with 
                // his mouse (like a 7683°)
                if (MouseRotationBuffer.Y < MathHelper.ToRadians(-75.0f))
                {
                    MouseRotationBuffer.Y = MouseRotationBuffer.Y -
                        (MouseRotationBuffer.Y - MathHelper.ToRadians(-75.0f));
                }
                if (MouseRotationBuffer.Y > MathHelper.ToRadians(75.0f))
                {
                    MouseRotationBuffer.Y = MouseRotationBuffer.Y -
                        (MouseRotationBuffer.Y - MathHelper.ToRadians(75.0f));
                }

                float mouseInverted = (Game.Config.MouseInverted) ? 1 : -1;

                Game.Camera.Rotation = new Vector3(
                    mouseInverted * MathHelper.Clamp(
                        MouseRotationBuffer.Y,
                        MathHelper.ToRadians(-75.0f),
                        MathHelper.ToRadians(75.0f)
                    ),
                    MathHelper.WrapAngle(MouseRotationBuffer.X), // This is so 
                    // the camera isn't going really fast after some time 
                    // (as we are increasing the speed with time)
                    0
                );

                // Resetting them
                deltaX = 0;
                deltaY = 0;

            }

            // Putting the cursor in the middle of the screen
            Mouse.SetPosition(Game.GraphicsDevice.Viewport.Width / 2,
                Game.GraphicsDevice.Viewport.Height / 2);

            PreviousMouseState = CurrentMouseState;
        }

        // Set camera position and rotation
        public void MoveTo(Vector3 position, Vector3 rotation)
        {
            // Thanks to the properties set at the beginning, setting up these 
            // values will execute the code inside the property (i.e update our
            // vectors)
            CameraOldPosition = Position;

            Game.Camera.Position = position;
            Game.Camera.Rotation = rotation;
        }

        // Methods that simulate movement
        private Vector3 PreviewMove(Vector3 movement, float dt)
        {
            // Create a rotate matrix
            Matrix rotate = Matrix.CreateRotationY(Game.ThisPlayer.Rotation.Y);
            // Create a movement vector
            Vector3 movementGravity = new Vector3(0, movement.Y, 0);
            movement = Vector3.Transform(movement, rotate);
            movementGravity = Vector3.Transform(movementGravity, rotate);
            // Return the value of camera position + movement vector

            // Testing for the UPCOMING position
            if (Game.SceneManager.Collision.CheckCollision(Game.ThisPlayer.Position + movement))
            {
                if (Game.SceneManager.Collision.CheckCollision(Game.ThisPlayer.Position + movementGravity))
                {
                    // Hit floor or ceiling
                    Game.ThisPlayerPhysics.StopGravity();
                    movement.Y = 0;
                    if (!ks.IsKeyDown(Game.Config.Jump))
                    {
                        Game.ThisPlayerPhysics.BunnyhopCoeff = 1;
                    }
                }
                else if(Game.ThisPlayerPhysics.State == LocalPlayerPhysics.PhysicalState.Walking &&
                    !Game.SceneManager.Collision.CheckCollision(
                        Game.ThisPlayer.Position + new Vector3(movement.X, 2, movement.Z)))
                {
                    movement.Y = 2;
                }
                // Creating the new movement vector, which will make us 
                // able to have a smooth collision: being able to "slide" on 
                // the wall while colliding
                movement.X = Game.SceneManager.Collision.CheckCollision(
                                    Game.ThisPlayer.Position +
                                    new Vector3(movement.X, 0, 0)
                                ) ? 0 : movement.X;
                movement.Y = Game.SceneManager.Collision.CheckCollision(
                                    Game.ThisPlayer.Position +
                                    new Vector3(0, movement.Y, 0)
                                ) ? 0 : movement.Y;
                movement.Z = Game.SceneManager.Collision.CheckCollision(
                                    Game.ThisPlayer.Position +
                                    new Vector3(0, 0, movement.Z)
                             ) ? 0 : movement.Z;
                return Game.ThisPlayer.Position + movement;
            }
            else
            {
                // There isn't any collision, so we just move the user with 
                // the movement he wanted to do
                return Game.ThisPlayer.Position + movement;
            }
        }

        public void Move(Vector3 scale, float dt)
        {
            MoveTo(PreviewMove(scale, dt), Game.ThisPlayer.Rotation);
        }

        public void HandleInput(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f;

            GetMouseMovement(dt);

            Vector3 moveVector = GetMovementVector(dt);
            if (moveVector != Vector3.Zero)
            {
                float tempY = moveVector.Y;
                moveVector.Y = 0;
                // Normalize that vector so that we don't move faster diagonally
                if (moveVector != Vector3.Zero) moveVector.Normalize();
                // Now we add in move factor and speed
                moveVector *= dt * Speed * Game.ThisPlayerPhysics.BunnyhopCoeff;
                moveVector.Y = tempY;
                // Move camera!
                Move(moveVector, dt);
            }


        }
    }
}
