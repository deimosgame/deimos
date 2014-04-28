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
        SpriteFont Font;

        int FrameRate, FrameCounter, UpdateRate, UpdateCounter;
        TimeSpan ElapsedTime = TimeSpan.Zero;
        List<string> DebugConsole = new List<string>();

        public DebugScreen()
        {
            Font = GeneralFacade.Game.Content.Load<SpriteFont>("Fonts/debug");
            float coeff = 0.15f;
            float MapWidth = GeneralFacade.Game.Renderer.NormalRT.Width * coeff;
            float MapHeight = GeneralFacade.Game.Renderer.NormalRT.Height * coeff;
            GeneralFacade.Game.ScreenElementManager.AddImage(
                "NormalMap",
                0,
                0,
                coeff,
                0,
                (Texture2D)GeneralFacade.Game.Renderer.NormalRT
            );
            GeneralFacade.Game.ScreenElementManager.AddImage(
                "DepthMap",
                (int)MapWidth,
                0,
                coeff,
                0,
                (Texture2D)GeneralFacade.Game.Renderer.DepthRT
            );
            GeneralFacade.Game.ScreenElementManager.AddImage(
                "LightMap",
                (int)MapWidth * 2,
                0,
                coeff,
                0,
                (Texture2D)GeneralFacade.Game.Renderer.LightRT
            );
            GeneralFacade.Game.ScreenElementManager.AddImage(
                "SSAOMap",
                (int)MapWidth * 3,
                0,
                coeff,
                0,
                (Texture2D)GeneralFacade.Game.Renderer.SSAORT
            );

            GeneralFacade.Game.ScreenElementManager.AddText(
                "Location",
                0,
                (int)MapHeight + 10,
                0,
                Font,
                "Location: ",
                Color.LightBlue
            );
            GeneralFacade.Game.ScreenElementManager.AddText(
                "FPS",
                0,
                (int)MapHeight + 50,
                0,
                Font,
                "FPS: ",
                Color.White
            );
            GeneralFacade.Game.ScreenElementManager.AddText(
                "Ticks",
                0,
                (int)MapHeight + 70,
                0,
                Font,
                "Ticks per sec: ",
                Color.White
            );
            GeneralFacade.Game.ScreenElementManager.AddText(
                "JumpState",
                0,
                (int)MapHeight + 90,
                0,
                Font,
                "JumpState: ",
                Color.LightGreen
            );
            GeneralFacade.Game.ScreenElementManager.AddText(
                "WeaponState",
                0,
                (int)MapHeight + 110,
                0,
                Font,
                "Weapon State: ",
                Color.LightCoral
            );
            GeneralFacade.Game.ScreenElementManager.AddText(
                "CurrentWeapon",
                0,
                (int)MapHeight + 130,
                0,
                Font,
                "CurrentWeapon:",
                Color.LightCoral
            );
            GeneralFacade.Game.ScreenElementManager.AddText(
                "CurrentChamberAmmo",
                0,
                (int)MapHeight + 150,
                0,
                Font,
                "CurrentChamberAmmo:",
                Color.LightCoral
            );
            GeneralFacade.Game.ScreenElementManager.AddText(
                "CurrentReservoirAmmo",
                0,
                (int)MapHeight + 170,
                0,
                Font,
                "CurrentReservoirAmmo:",
                Color.LightCoral
            );
            GeneralFacade.Game.ScreenElementManager.AddText(
                "Rotation",
                0,
                (int)MapHeight + 30,
                0,
                Font,
                "Rotation: ",
                Color.LightBlue
            );
            GeneralFacade.Game.ScreenElementManager.AddText(
                "SpeedState",
                0,
                (int)MapHeight + 190,
                0,
                Font,
                "Speed State: ",
                Color.LightSalmon
            );
            GeneralFacade.Game.ScreenElementManager.AddText(
                "SprintTimer",
                0,
                (int)MapHeight + 210,
                0,
                Font,
                "Sprint Timer: ",
                Color.LightSalmon
            );
            GeneralFacade.Game.ScreenElementManager.AddText(
                "CooldownTimer",
                0,
                (int)MapHeight + 230,
                0,
                Font,
                "Cooldown Timer: ",
                Color.LightSalmon
            );
            GeneralFacade.Game.ScreenElementManager.AddText(
                "DebugConsole",
                (int)(MapWidth/coeff)-400,
                10,
                0,
                Font,
                "Console",
                Color.LightSalmon
            );
            GeneralFacade.Game.ScreenElementManager.AddText(
                "PlayingState",
                0,
                (int)MapHeight + 250,
                0,
                Font,
                "Playing State:",
                Color.DeepPink
            );
            GeneralFacade.Game.ScreenElementManager.AddText(
                "LifeState",
                0,
                (int)MapHeight + 270,
                0,
                Font,
                "Life State:",
                Color.DeepPink
            );
            GeneralFacade.Game.ScreenElementManager.AddText(
                "Health",
                0,
                (int)MapHeight + 290,
                0,
                Font,
                "Health:",
                Color.DeepPink
            );
            GeneralFacade.Game.ScreenElementManager.AddText(
                "Acceleration",
                0,
                (int)MapHeight + 310,
                0,
                Font,
                "Acceleration:",
                Color.Yellow
            );
            GeneralFacade.Game.ScreenElementManager.AddText(
                "G-Time",
                0,
                (int)MapHeight + 330,
                0,
                Font,
                "G-Time:",
                Color.Blue
            );
            GeneralFacade.Game.ScreenElementManager.AddText(
                "InitialVelocity",
                0,
                (int)MapHeight + 350,
                0,
                Font,
                "InitialVelocity:",
                Color.Blue
            );
            GeneralFacade.Game.ScreenElementManager.AddText(
                "Class",
                0,
                (int)MapHeight + 390,
                0,
                Font,
                "Player Class:",
                Color.Red
            );
            GeneralFacade.Game.ScreenElementManager.AddText(
                "Name",
                0,
                (int)MapHeight + 370,
                0,
                Font,
                "Player Name:",
                Color.Red
            );
            GeneralFacade.Game.ScreenElementManager.AddText(
                "Instance",
                0,
                (int)MapHeight + 410,
                0,
                Font,
                "Spawn Instance:",
                Color.Red
            );
            GeneralFacade.Game.ScreenElementManager.AddText(
                "View",
                0,
                (int)MapHeight + 430,
                0,
                Font,
                "Camera View:",
                Color.Yellow
            );
            GeneralFacade.Game.ScreenElementManager.AddText(
                "Momentum",
                0,
                (int)MapHeight + 450,
                0,
                Font,
                "Momentum:",
                Color.Yellow
            );
            GeneralFacade.Game.ScreenElementManager.AddText(
                "Scroll",
                0,
                (int)MapHeight + 470,
                0,
                Font,
                "Scroll:",
                Color.White
            );
            

        }

        private void Show()
        {
            GeneralFacade.Game.ScreenElementManager.GetImage("NormalMap").Show = true;
            GeneralFacade.Game.ScreenElementManager.GetImage("DepthMap").Show = true;
            GeneralFacade.Game.ScreenElementManager.GetImage("LightMap").Show = true;
            GeneralFacade.Game.ScreenElementManager.GetImage("SSAOMap").Show = true;
            GeneralFacade.Game.ScreenElementManager.GetText("Location").Show = true;
            GeneralFacade.Game.ScreenElementManager.GetText("FPS").Show = true;
            GeneralFacade.Game.ScreenElementManager.GetText("Ticks").Show = true;
            GeneralFacade.Game.ScreenElementManager.GetText("JumpState").Show = true;
            GeneralFacade.Game.ScreenElementManager.GetText("CurrentWeapon").Show = true;
            GeneralFacade.Game.ScreenElementManager.GetText("CurrentChamberAmmo").Show = true;
            GeneralFacade.Game.ScreenElementManager.GetText("CurrentReservoirAmmo").Show = true;
            GeneralFacade.Game.ScreenElementManager.GetText("Rotation").Show = true;
            GeneralFacade.Game.ScreenElementManager.GetText("SpeedState").Show = true;
            GeneralFacade.Game.ScreenElementManager.GetText("SprintTimer").Show = true;
            GeneralFacade.Game.ScreenElementManager.GetText("CooldownTimer").Show = true;
            GeneralFacade.Game.ScreenElementManager.GetText("PlayingState").Show = true;
            GeneralFacade.Game.ScreenElementManager.GetText("LifeState").Show = true;
            GeneralFacade.Game.ScreenElementManager.GetText("Health").Show = true;
            GeneralFacade.Game.ScreenElementManager.GetText("Acceleration").Show = true;
            GeneralFacade.Game.ScreenElementManager.GetText("G-Time").Show = true;
            GeneralFacade.Game.ScreenElementManager.GetText("InitialVelocity").Show = true;
            GeneralFacade.Game.ScreenElementManager.GetText("DebugConsole").Show = true;
            GeneralFacade.Game.ScreenElementManager.GetText("Class").Show = true;
            GeneralFacade.Game.ScreenElementManager.GetText("Name").Show = true;
            GeneralFacade.Game.ScreenElementManager.GetText("Instance").Show = true;
            GeneralFacade.Game.ScreenElementManager.GetText("View").Show = true;
            GeneralFacade.Game.ScreenElementManager.GetText("Momentum").Show = true;
            GeneralFacade.Game.ScreenElementManager.GetText("Scroll").Show = true;
            GeneralFacade.Game.ScreenElementManager.GetText("WeaponState").Show = true;
        }
        private void Hide()
        {
            GeneralFacade.Game.ScreenElementManager.GetImage("NormalMap").Show = false;
            GeneralFacade.Game.ScreenElementManager.GetImage("DepthMap").Show = false;
            GeneralFacade.Game.ScreenElementManager.GetImage("LightMap").Show = false;
            GeneralFacade.Game.ScreenElementManager.GetImage("SSAOMap").Show = false;
            GeneralFacade.Game.ScreenElementManager.GetText("Location").Show = false;
            GeneralFacade.Game.ScreenElementManager.GetText("FPS").Show = false;
            GeneralFacade.Game.ScreenElementManager.GetText("Ticks").Show = false;
            GeneralFacade.Game.ScreenElementManager.GetText("JumpState").Show = false;
            GeneralFacade.Game.ScreenElementManager.GetText("CurrentWeapon").Show = false;
            GeneralFacade.Game.ScreenElementManager.GetText("CurrentChamberAmmo").Show = false;
            GeneralFacade.Game.ScreenElementManager.GetText("CurrentReservoirAmmo").Show = false;
            GeneralFacade.Game.ScreenElementManager.GetText("Rotation").Show = false;
            GeneralFacade.Game.ScreenElementManager.GetText("SpeedState").Show = false;
            GeneralFacade.Game.ScreenElementManager.GetText("SprintTimer").Show = false;
            GeneralFacade.Game.ScreenElementManager.GetText("CooldownTimer").Show = false;
            GeneralFacade.Game.ScreenElementManager.GetText("PlayingState").Show = false;
            GeneralFacade.Game.ScreenElementManager.GetText("LifeState").Show = false;
            GeneralFacade.Game.ScreenElementManager.GetText("Health").Show = false;
            GeneralFacade.Game.ScreenElementManager.GetText("Acceleration").Show = false;
            GeneralFacade.Game.ScreenElementManager.GetText("G-Time").Show = false;
            GeneralFacade.Game.ScreenElementManager.GetText("InitialVelocity").Show = false;
            GeneralFacade.Game.ScreenElementManager.GetText("DebugConsole").Show = false;
            GeneralFacade.Game.ScreenElementManager.GetText("Class").Show = false;
            GeneralFacade.Game.ScreenElementManager.GetText("Name").Show = false;
            GeneralFacade.Game.ScreenElementManager.GetText("Instance").Show = false;
            GeneralFacade.Game.ScreenElementManager.GetText("View").Show = false;
            GeneralFacade.Game.ScreenElementManager.GetText("Momentum").Show = false;
            GeneralFacade.Game.ScreenElementManager.GetText("Scroll").Show = false;
            GeneralFacade.Game.ScreenElementManager.GetText("WeaponState").Show = false;
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
            if (!GeneralFacade.Game.Config.DebugScreen)
            {
                Hide();
                return;
            }
            Show();

            GeneralFacade.Game.ScreenElementManager.GetText("Location").Text = String.Format(
                "Location: x:{0}; y:{1}; z:{2}",
                (int)GeneralFacade.Game.ThisPlayer.Position.X,
                (int)GeneralFacade.Game.ThisPlayer.Position.Y,
                (int)GeneralFacade.Game.ThisPlayer.Position.Z
            );
            GeneralFacade.Game.ScreenElementManager.GetText("FPS").Text = String.Format(
                "FPS: {0}",
                FrameRate
            );
            GeneralFacade.Game.ScreenElementManager.GetText("Ticks").Text = String.Format(
                "Ticks per sec: {0}",
                UpdateRate
            );
            GeneralFacade.Game.ScreenElementManager.GetText("JumpState").Text = String.Format(
                "JumpState: {0}",
                GeneralFacade.Game.ThisPlayerPhysics.GravityState
            );
            GeneralFacade.Game.ScreenElementManager.GetText("CurrentWeapon").Text = String.Format(
                "CurrentWeapon: {0}",
                GeneralFacade.Game.ThisPlayer.CurrentWeapon.Name
            );
            GeneralFacade.Game.ScreenElementManager.GetText("CurrentChamberAmmo").Text = String.Format(
                "CurrentChamberAmmo: {0}",
                GeneralFacade.Game.ThisPlayer.CurrentWeapon.c_chamberAmmo
            );
            GeneralFacade.Game.ScreenElementManager.GetText("CurrentReservoirAmmo").Text = String.Format(
                "CurrentReservoirAmmo: {0}",
                GeneralFacade.Game.ThisPlayer.CurrentWeapon.c_reservoirAmmo
            );
            GeneralFacade.Game.ScreenElementManager.GetText("Rotation").Text = String.Format(
                "Rotation: x:{0}; y:{1}; z:{2}",
                (float)GeneralFacade.Game.ThisPlayer.Rotation.X,
                (float)GeneralFacade.Game.ThisPlayer.Rotation.Y,
                (float)GeneralFacade.Game.ThisPlayer.Rotation.Z
            );
            GeneralFacade.Game.ScreenElementManager.GetText("SpeedState").Text = String.Format(
                "Speed State: {0}",
                GeneralFacade.Game.ThisPlayer.CurrentSpeedState
            );
            GeneralFacade.Game.ScreenElementManager.GetText("SprintTimer").Text = String.Format(
                "Sprint Timer: {0}",
                GeneralFacade.Game.ThisPlayer.SprintTimer
            );
            GeneralFacade.Game.ScreenElementManager.GetText("CooldownTimer").Text = String.Format(
                "CooldownTimer: {0}",
                GeneralFacade.Game.ThisPlayer.CooldownTimer
            );
            GeneralFacade.Game.ScreenElementManager.GetText("PlayingState").Text = String.Format(
                "Playing State: {0}",
                GeneralFacade.Game.CurrentPlayingState
            );
            GeneralFacade.Game.ScreenElementManager.GetText("LifeState").Text = String.Format(
                "Life State: {0}",
                GeneralFacade.Game.ThisPlayer.CurrentLifeState
            );
            GeneralFacade.Game.ScreenElementManager.GetText("Health").Text = String.Format(
                "Health: {0}",
                GeneralFacade.Game.ThisPlayer.Health
            );
            GeneralFacade.Game.ScreenElementManager.GetText("Acceleration").Text = String.Format(
                "Acceleration: x:{0}; y:{1}; z:{2}",
                (float)GeneralFacade.Game.ThisPlayerPhysics.GetAcceleration().X,
                (float)GeneralFacade.Game.ThisPlayerPhysics.GetAcceleration().Y,
                (float)GeneralFacade.Game.ThisPlayerPhysics.GetAcceleration().Z
            );
            GeneralFacade.Game.ScreenElementManager.GetText("G-Time").Text = String.Format(
                "G-Time: {0}",
                GeneralFacade.Game.ThisPlayerPhysics.timer_gravity
            );
            GeneralFacade.Game.ScreenElementManager.GetText("InitialVelocity").Text = String.Format(
                "InitialVelocity: {0}",
                GeneralFacade.Game.ThisPlayerPhysics.initial_velocity
            );
            GeneralFacade.Game.ScreenElementManager.GetText("Class").Text = String.Format(
                "Player Class: {0}",
                GeneralFacade.Game.ThisPlayer.Class
            );
            GeneralFacade.Game.ScreenElementManager.GetText("Name").Text = String.Format(
                "Player Name: {0}",
                GeneralFacade.Game.ThisPlayer.Name
            );
            GeneralFacade.Game.ScreenElementManager.GetText("Instance").Text = String.Format(
                "Spawn Instance: {0}",
                GeneralFacade.Game.ThisPlayer.Instance
            );
            GeneralFacade.Game.ScreenElementManager.GetText("View").Text = String.Format(
                "Camera View x:{0}; y:{1}; z:{2}",
                (float)GeneralFacade.Game.Camera.ViewVector.X,
                (float)GeneralFacade.Game.Camera.ViewVector.Y,
                (float)GeneralFacade.Game.Camera.ViewVector.Z
            );
            GeneralFacade.Game.ScreenElementManager.GetText("Momentum").Text = String.Format(
                "Momentum: x:{0}; y:{1}; z:{2}",
                (float)GeneralFacade.Game.ThisPlayerPhysics.momentum.X,
                (float)GeneralFacade.Game.ThisPlayerPhysics.momentum.Y,
                (float)GeneralFacade.Game.ThisPlayerPhysics.momentum.Z
            );
            GeneralFacade.Game.ScreenElementManager.GetText("Scroll").Text = String.Format(
                "Scroll: {0}",
                GeneralFacade.Game.ThisPlayer.previousScrollValue
            );
            GeneralFacade.Game.ScreenElementManager.GetText("WeaponState").Text = String.Format(
                "Weapon State: {0}",
                GeneralFacade.Game.ThisPlayer.CurrentWeapon.State
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
            GeneralFacade.Game.ScreenElementManager.GetText("DebugConsole").Text = finalDebug;
        }
    }
}
