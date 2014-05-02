using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Deimos
{
    class DebugScreen
    {
        public SpriteFont Font;

        int FrameRate, FrameCounter, UpdateRate, UpdateCounter;
        TimeSpan ElapsedTime = TimeSpan.Zero;
        List<string> DebugConsole = new List<string>();

        public DebugScreen()
        {
            Font = GeneralFacade.Game.Content.Load<SpriteFont>("Fonts/debug");
            float coeff = 0.15f;
            float MapWidth = GeneralFacade.Game.Renderer.NormalRT.Width * coeff;
            float MapHeight = GeneralFacade.Game.Renderer.NormalRT.Height * coeff;
            DisplayFacade.ScreenElementManager.AddImage(
                "NormalMap",
                0,
                0,
                coeff,
                0,
                (Texture2D)GeneralFacade.Game.Renderer.NormalRT
            );
            DisplayFacade.ScreenElementManager.AddImage(
                "DepthMap",
                (int)MapWidth,
                0,
                coeff,
                0,
                (Texture2D)GeneralFacade.Game.Renderer.DepthRT
            );
            DisplayFacade.ScreenElementManager.AddImage(
                "LightMap",
                (int)MapWidth * 2,
                0,
                coeff,
                0,
                (Texture2D)GeneralFacade.Game.Renderer.LightRT
            );
            DisplayFacade.ScreenElementManager.AddImage(
                "SSAOMap",
                (int)MapWidth * 3,
                0,
                coeff,
                0,
                (Texture2D)GeneralFacade.Game.Renderer.SSAORT
            );

            DisplayFacade.ScreenElementManager.AddText(
                "Location",
                0,
                (int)MapHeight + 10,
                0,
                Font,
                "Location: ",
                Color.LightBlue
            );
            DisplayFacade.ScreenElementManager.AddText(
                "FPS",
                0,
                (int)MapHeight + 50,
                0,
                Font,
                "FPS: ",
                Color.White
            );
            DisplayFacade.ScreenElementManager.AddText(
                "Ticks",
                0,
                (int)MapHeight + 70,
                0,
                Font,
                "Ticks per sec: ",
                Color.White
            );
            DisplayFacade.ScreenElementManager.AddText(
                "JumpState",
                0,
                (int)MapHeight + 90,
                0,
                Font,
                "JumpState: ",
                Color.LightGreen
            );
            DisplayFacade.ScreenElementManager.AddText(
                "WeaponState",
                0,
                (int)MapHeight + 110,
                0,
                Font,
                "Weapon State: ",
                Color.LightCoral
            );
            DisplayFacade.ScreenElementManager.AddText(
                "CurrentWeapon",
                0,
                (int)MapHeight + 130,
                0,
                Font,
                "CurrentWeapon:",
                Color.LightCoral
            );
            DisplayFacade.ScreenElementManager.AddText(
                "CurrentChamberAmmo",
                0,
                (int)MapHeight + 150,
                0,
                Font,
                "CurrentChamberAmmo:",
                Color.LightCoral
            );
            DisplayFacade.ScreenElementManager.AddText(
                "CurrentReservoirAmmo",
                0,
                (int)MapHeight + 170,
                0,
                Font,
                "CurrentReservoirAmmo:",
                Color.LightCoral
            );
            DisplayFacade.ScreenElementManager.AddText(
                "Rotation",
                0,
                (int)MapHeight + 30,
                0,
                Font,
                "Rotation: ",
                Color.LightBlue
            );
            DisplayFacade.ScreenElementManager.AddText(
                "SpeedState",
                0,
                (int)MapHeight + 190,
                0,
                Font,
                "Speed State: ",
                Color.LightSalmon
            );
            DisplayFacade.ScreenElementManager.AddText(
                "SprintTimer",
                0,
                (int)MapHeight + 210,
                0,
                Font,
                "Sprint Timer: ",
                Color.LightSalmon
            );
            DisplayFacade.ScreenElementManager.AddText(
                "CooldownTimer",
                0,
                (int)MapHeight + 230,
                0,
                Font,
                "Cooldown Timer: ",
                Color.LightSalmon
            );
            DisplayFacade.ScreenElementManager.AddText(
                "DebugConsole",
                (int)(MapWidth/coeff)-400,
                10,
                0,
                Font,
                "Console",
                Color.LightSalmon
            );
            DisplayFacade.ScreenElementManager.AddText(
                "PlayingState",
                0,
                (int)MapHeight + 250,
                0,
                Font,
                "Playing State:",
                Color.DeepPink
            );
            DisplayFacade.ScreenElementManager.AddText(
                "LifeState",
                0,
                (int)MapHeight + 270,
                0,
                Font,
                "Life State:",
                Color.DeepPink
            );
            DisplayFacade.ScreenElementManager.AddText(
                "Health",
                0,
                (int)MapHeight + 290,
                0,
                Font,
                "Health:",
                Color.DeepPink
            );
            DisplayFacade.ScreenElementManager.AddText(
                "Acceleration",
                0,
                (int)MapHeight + 310,
                0,
                Font,
                "Acceleration:",
                Color.Yellow
            );
            DisplayFacade.ScreenElementManager.AddText(
                "G-Time",
                0,
                (int)MapHeight + 330,
                0,
                Font,
                "G-Time:",
                Color.Blue
            );
            DisplayFacade.ScreenElementManager.AddText(
                "InitialVelocity",
                0,
                (int)MapHeight + 350,
                0,
                Font,
                "InitialVelocity:",
                Color.Blue
            );
            DisplayFacade.ScreenElementManager.AddText(
                "Class",
                0,
                (int)MapHeight + 390,
                0,
                Font,
                "Player Class:",
                Color.Red
            );
            DisplayFacade.ScreenElementManager.AddText(
                "Name",
                0,
                (int)MapHeight + 370,
                0,
                Font,
                "Player Name:",
                Color.Red
            );
            DisplayFacade.ScreenElementManager.AddText(
                "Instance",
                0,
                (int)MapHeight + 410,
                0,
                Font,
                "Spawn Instance:",
                Color.Red
            );
            DisplayFacade.ScreenElementManager.AddText(
                "View",
                0,
                (int)MapHeight + 430,
                0,
                Font,
                "Camera View:",
                Color.Yellow
            );
            DisplayFacade.ScreenElementManager.AddText(
                "Momentum",
                0,
                (int)MapHeight + 450,
                0,
                Font,
                "Momentum:",
                Color.Yellow
            );
            DisplayFacade.ScreenElementManager.AddText(
                "Scroll",
                0,
                (int)MapHeight + 470,
                0,
                Font,
                "Scroll:",
                Color.White
            );
            DisplayFacade.ScreenElementManager.AddText(
                "NumberParticles",
                0,
                (int)MapHeight + 490,
                0,
                Font,
                "Particles: ",
                Color.White
            );
            DisplayFacade.ScreenElementManager.AddText(
                "CollisionState",
                0,
                (int)MapHeight + 510,
                0,
                Font,
                "Collision: ",
                Color.White
            );
            DisplayFacade.ScreenElementManager.AddText(
                "MultiUser",
                0,
                (int)MapHeight + 530,
                0,
                Font,
                "User: " + Program.PlayerEmail + " " + Program.PlayerToken,
                Color.White
            );

        }

        private void Show()
        {
            DisplayFacade.ScreenElementManager.GetImage("NormalMap").Visible = true;
            DisplayFacade.ScreenElementManager.GetImage("DepthMap").Visible = true;
            DisplayFacade.ScreenElementManager.GetImage("LightMap").Visible = true;
            DisplayFacade.ScreenElementManager.GetImage("SSAOMap").Visible = true;
            DisplayFacade.ScreenElementManager.GetText("Location").Visible = true;
            DisplayFacade.ScreenElementManager.GetText("FPS").Visible = true;
            DisplayFacade.ScreenElementManager.GetText("Ticks").Visible = true;
            DisplayFacade.ScreenElementManager.GetText("JumpState").Visible = true;
            DisplayFacade.ScreenElementManager.GetText("CurrentWeapon").Visible = true;
            DisplayFacade.ScreenElementManager.GetText("CurrentChamberAmmo").Visible = true;
            DisplayFacade.ScreenElementManager.GetText("CurrentReservoirAmmo").Visible = true;
            DisplayFacade.ScreenElementManager.GetText("Rotation").Visible = true;
            DisplayFacade.ScreenElementManager.GetText("SpeedState").Visible = true;
            DisplayFacade.ScreenElementManager.GetText("SprintTimer").Visible = true;
            DisplayFacade.ScreenElementManager.GetText("CooldownTimer").Visible = true;
            DisplayFacade.ScreenElementManager.GetText("PlayingState").Visible = true;
            DisplayFacade.ScreenElementManager.GetText("LifeState").Visible = true;
            DisplayFacade.ScreenElementManager.GetText("Health").Visible = true;
            DisplayFacade.ScreenElementManager.GetText("Acceleration").Visible = true;
            DisplayFacade.ScreenElementManager.GetText("G-Time").Visible = true;
            DisplayFacade.ScreenElementManager.GetText("InitialVelocity").Visible = true;
            DisplayFacade.ScreenElementManager.GetText("DebugConsole").Visible = true;
            DisplayFacade.ScreenElementManager.GetText("Class").Visible = true;
            DisplayFacade.ScreenElementManager.GetText("Name").Visible = true;
            DisplayFacade.ScreenElementManager.GetText("Instance").Visible = true;
            DisplayFacade.ScreenElementManager.GetText("View").Visible = true;
            DisplayFacade.ScreenElementManager.GetText("Momentum").Visible = true;
            DisplayFacade.ScreenElementManager.GetText("Scroll").Visible = true;
            DisplayFacade.ScreenElementManager.GetText("WeaponState").Visible = true;
            DisplayFacade.ScreenElementManager.GetText("NumberParticles").Visible = true;
            DisplayFacade.ScreenElementManager.GetText("CollisionState").Visible = true;
            DisplayFacade.ScreenElementManager.GetText("MultiUser").Visible = true;
        }
        private void Hide()
        {
            DisplayFacade.ScreenElementManager.GetImage("NormalMap").Visible = false;
            DisplayFacade.ScreenElementManager.GetImage("DepthMap").Visible = false;
            DisplayFacade.ScreenElementManager.GetImage("LightMap").Visible = false;
            DisplayFacade.ScreenElementManager.GetImage("SSAOMap").Visible = false;
            DisplayFacade.ScreenElementManager.GetText("Location").Visible = false;
            DisplayFacade.ScreenElementManager.GetText("FPS").Visible = false;
            DisplayFacade.ScreenElementManager.GetText("Ticks").Visible = false;
            DisplayFacade.ScreenElementManager.GetText("JumpState").Visible = false;
            DisplayFacade.ScreenElementManager.GetText("CurrentWeapon").Visible = false;
            DisplayFacade.ScreenElementManager.GetText("CurrentChamberAmmo").Visible = false;
            DisplayFacade.ScreenElementManager.GetText("CurrentReservoirAmmo").Visible = false;
            DisplayFacade.ScreenElementManager.GetText("Rotation").Visible = false;
            DisplayFacade.ScreenElementManager.GetText("SpeedState").Visible = false;
            DisplayFacade.ScreenElementManager.GetText("SprintTimer").Visible = false;
            DisplayFacade.ScreenElementManager.GetText("CooldownTimer").Visible = false;
            DisplayFacade.ScreenElementManager.GetText("PlayingState").Visible = false;
            DisplayFacade.ScreenElementManager.GetText("LifeState").Visible = false;
            DisplayFacade.ScreenElementManager.GetText("Health").Visible = false;
            DisplayFacade.ScreenElementManager.GetText("Acceleration").Visible = false;
            DisplayFacade.ScreenElementManager.GetText("G-Time").Visible = false;
            DisplayFacade.ScreenElementManager.GetText("InitialVelocity").Visible = false;
            DisplayFacade.ScreenElementManager.GetText("DebugConsole").Visible = false;
            DisplayFacade.ScreenElementManager.GetText("Class").Visible = false;
            DisplayFacade.ScreenElementManager.GetText("Name").Visible = false;
            DisplayFacade.ScreenElementManager.GetText("Instance").Visible = false;
            DisplayFacade.ScreenElementManager.GetText("View").Visible = false;
            DisplayFacade.ScreenElementManager.GetText("Momentum").Visible = false;
            DisplayFacade.ScreenElementManager.GetText("Scroll").Visible = false;
            DisplayFacade.ScreenElementManager.GetText("WeaponState").Visible = false;
            DisplayFacade.ScreenElementManager.GetText("NumberParticles").Visible = false;
            DisplayFacade.ScreenElementManager.GetText("CollisionState").Visible = false;
            DisplayFacade.ScreenElementManager.GetText("MultiUser").Visible = false;
        }

        public void Debug(string text)
        {
            DebugConsole.Add(text);
        }

        public void Update(GameTime gameTime)
        {
            UpdateCounter++;
            ElapsedTime += gameTime.ElapsedGameTime;

            if (ElapsedTime > TimeSpan.FromSeconds(1))
            {
                ElapsedTime -= TimeSpan.FromSeconds(1);
                FrameRate = FrameCounter;
                UpdateRate = UpdateCounter;
                UpdateCounter = 0;
                FrameCounter = 0;
            }
        }

        public void Draw(GameTime gameTime)
        {
            FrameCounter++;
            if (!GeneralFacade.Config.DebugScreen)
            {
                Hide();
                return;
            }
            Show();

            DisplayFacade.ScreenElementManager.GetText("Location").Text = String.Format(
                "Location: x:{0}; y:{1}; z:{2}",
                (int)GameplayFacade.ThisPlayer.Position.X,
                (int)GameplayFacade.ThisPlayer.Position.Y,
                (int)GameplayFacade.ThisPlayer.Position.Z
            );
            DisplayFacade.ScreenElementManager.GetText("FPS").Text = String.Format(
                "FPS: {0}",
                FrameRate
            );
            DisplayFacade.ScreenElementManager.GetText("Ticks").Text = String.Format(
                "Ticks per sec: {0}",
                UpdateRate
            );
            DisplayFacade.ScreenElementManager.GetText("JumpState").Text = String.Format(
                "JumpState: {0}",
                GameplayFacade.ThisPlayerPhysics.GravityState
            );
            DisplayFacade.ScreenElementManager.GetText("CurrentWeapon").Text = String.Format(
                "CurrentWeapon: {0}",
                GameplayFacade.ThisPlayer.CurrentWeapon.Name
            );
            DisplayFacade.ScreenElementManager.GetText("CurrentChamberAmmo").Text = String.Format(
                "CurrentChamberAmmo: {0}",
                GameplayFacade.ThisPlayer.CurrentWeapon.c_chamberAmmo
            );
            DisplayFacade.ScreenElementManager.GetText("CurrentReservoirAmmo").Text = String.Format(
                "CurrentReservoirAmmo: {0}",
                GameplayFacade.ThisPlayer.CurrentWeapon.c_reservoirAmmo
            );
            DisplayFacade.ScreenElementManager.GetText("Rotation").Text = String.Format(
                "Rotation: x:{0}; y:{1}; z:{2}",
                (float)GameplayFacade.ThisPlayer.Rotation.X,
                (float)GameplayFacade.ThisPlayer.Rotation.Y,
                (float)GameplayFacade.ThisPlayer.Rotation.Z
            );
            DisplayFacade.ScreenElementManager.GetText("SpeedState").Text = String.Format(
                "Speed State: {0}",
                GameplayFacade.ThisPlayer.CurrentSpeedState
            );
            DisplayFacade.ScreenElementManager.GetText("SprintTimer").Text = String.Format(
                "Sprint Timer: {0}",
                GameplayFacade.ThisPlayer.SprintTimer
            );
            DisplayFacade.ScreenElementManager.GetText("CooldownTimer").Text = String.Format(
                "CooldownTimer: {0}",
                GameplayFacade.ThisPlayer.CooldownTimer
            );
            DisplayFacade.ScreenElementManager.GetText("PlayingState").Text = String.Format(
                "Playing State: {0}",
                GeneralFacade.Game.CurrentPlayingState
            );
            DisplayFacade.ScreenElementManager.GetText("LifeState").Text = String.Format(
                "Life State: {0}",
                GameplayFacade.ThisPlayer.CurrentLifeState
            );
            DisplayFacade.ScreenElementManager.GetText("Health").Text = String.Format(
                "Health: {0}",
                GameplayFacade.ThisPlayer.Health
            );
            DisplayFacade.ScreenElementManager.GetText("Acceleration").Text = String.Format(
                "Acceleration: x:{0}; y:{1}; z:{2}",
                (float)GameplayFacade.ThisPlayerPhysics.GetAcceleration().X,
                (float)GameplayFacade.ThisPlayerPhysics.GetAcceleration().Y,
                (float)GameplayFacade.ThisPlayerPhysics.GetAcceleration().Z
            );
            DisplayFacade.ScreenElementManager.GetText("G-Time").Text = String.Format(
                "G-Time: {0}",
                GameplayFacade.ThisPlayerPhysics.timer_gravity
            );
            DisplayFacade.ScreenElementManager.GetText("InitialVelocity").Text = String.Format(
                "InitialVelocity: {0}",
                GameplayFacade.ThisPlayerPhysics.initial_velocity
            );
            DisplayFacade.ScreenElementManager.GetText("Class").Text = String.Format(
                "Player Class: {0}",
                GameplayFacade.ThisPlayer.Class
            );
            DisplayFacade.ScreenElementManager.GetText("Name").Text = String.Format(
                "Player Name: {0}",
                GameplayFacade.ThisPlayer.Name
            );
            DisplayFacade.ScreenElementManager.GetText("Instance").Text = String.Format(
                "Spawn Instance: {0}",
                GameplayFacade.ThisPlayer.Instance
            );
            DisplayFacade.ScreenElementManager.GetText("View").Text = String.Format(
                "Camera View x:{0}; y:{1}; z:{2}",
                (float)DisplayFacade.Camera.ViewVector.X,
                (float)DisplayFacade.Camera.ViewVector.Y,
                (float)DisplayFacade.Camera.ViewVector.Z
            );
            DisplayFacade.ScreenElementManager.GetText("Momentum").Text = String.Format(
                "Momentum: x:{0}; y:{1}; z:{2}",
                (float)GameplayFacade.ThisPlayerPhysics.momentum.X,
                (float)GameplayFacade.ThisPlayerPhysics.momentum.Y,
                (float)GameplayFacade.ThisPlayerPhysics.momentum.Z
            );
            DisplayFacade.ScreenElementManager.GetText("Scroll").Text = String.Format(
                "Scroll: {0}",
                GameplayFacade.ThisPlayer.previousScrollValue
            );
            DisplayFacade.ScreenElementManager.GetText("WeaponState").Text = String.Format(
                "Weapon State: {0}",
                GameplayFacade.ThisPlayer.CurrentWeapon.State
            );
            DisplayFacade.ScreenElementManager.GetText("NumberParticles").Text = String.Format(
                "Particles: {0}",
                DisplayFacade.ParticleManager.ParticleCount
            );
            DisplayFacade.ScreenElementManager.GetText("CollisionState").Text = String.Format(
                "Collision: {0}",
                GeneralFacade.SceneManager.PlayerCollision.CheckCollision(GameplayFacade.ThisPlayer.Position)
            );

            string finalDebug = "";
            int southLimit = DebugConsole.Count - 10;
            if (southLimit < 0)
            {
                southLimit = 0;
            }
            for (int i = DebugConsole.Count - 1; i > southLimit; i--)
            {
                finalDebug += DebugConsole[i] + "\n";
            }
            DisplayFacade.ScreenElementManager.GetText("DebugConsole").Text = finalDebug;
        }
    }
}
