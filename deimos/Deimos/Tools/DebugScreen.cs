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
        DeimosGame Game;
        SpriteFont Font;

        int FrameRate, FrameCounter, UpdateRate, UpdateCounter;
        TimeSpan ElapsedTime = TimeSpan.Zero;
        List<string> DebugConsole = new List<string>();

        public DebugScreen(DeimosGame game)
        {
            Game = game;
            Font = Game.Content.Load<SpriteFont>("Fonts/debug");
            float coeff = 0.15f;
            float MapWidth = Game.Renderer.NormalRT.Width * coeff;
            float MapHeight = Game.Renderer.NormalRT.Height * coeff;
            Game.ScreenElementManager.AddImage(
                "NormalMap",
                0,
                0,
                coeff,
                0,
                (Texture2D)Game.Renderer.NormalRT
            );
            Game.ScreenElementManager.AddImage(
                "DepthMap",
                (int)MapWidth,
                0,
                coeff,
                0,
                (Texture2D)Game.Renderer.DepthRT
            );
            Game.ScreenElementManager.AddImage(
                "LightMap",
                (int)MapWidth * 2,
                0,
                coeff,
                0,
                (Texture2D)Game.Renderer.LightRT
            );
            Game.ScreenElementManager.AddImage(
                "SSAOMap",
                (int)MapWidth * 3,
                0,
                coeff,
                0,
                (Texture2D)Game.Renderer.SSAORT
            );

            Game.ScreenElementManager.AddText(
                "Location",
                0,
                (int)MapHeight + 10,
                0,
                Font,
                "Location: ",
                Color.LightBlue
            );
            Game.ScreenElementManager.AddText(
                "FPS",
                0,
                (int)MapHeight + 50,
                0,
                Font,
                "FPS: ",
                Color.White
            );
            Game.ScreenElementManager.AddText(
                "Ticks",
                0,
                (int)MapHeight + 70,
                0,
                Font,
                "Ticks per sec: ",
                Color.White
            );
            Game.ScreenElementManager.AddText(
                "JumpState",
                0,
                (int)MapHeight + 90,
                0,
                Font,
                "JumpState: ",
                Color.LightGreen
            );
            Game.ScreenElementManager.AddText(
                "CurrentWeapon",
                0,
                (int)MapHeight + 130,
                0,
                Font,
                "CurrentWeapon:",
                Color.LightCoral
            );
            Game.ScreenElementManager.AddText(
                "CurrentChamberAmmo",
                0,
                (int)MapHeight + 150,
                0,
                Font,
                "CurrentChamberAmmo:",
                Color.LightCoral
            );
            Game.ScreenElementManager.AddText(
                "CurrentReservoirAmmo",
                0,
                (int)MapHeight + 170,
                0,
                Font,
                "CurrentReservoirAmmo:",
                Color.LightCoral
            );
            Game.ScreenElementManager.AddText(
                "Rotation",
                0,
                (int)MapHeight + 30,
                0,
                Font,
                "Rotation: ",
                Color.LightBlue
            );
            Game.ScreenElementManager.AddText(
                "SpeedState",
                0,
                (int)MapHeight + 190,
                0,
                Font,
                "Speed State: ",
                Color.LightSalmon
            );
            Game.ScreenElementManager.AddText(
                "SprintTimer",
                0,
                (int)MapHeight + 210,
                0,
                Font,
                "Sprint Timer: ",
                Color.LightSalmon
            );
            Game.ScreenElementManager.AddText(
                "CooldownTimer",
                0,
                (int)MapHeight + 230,
                0,
                Font,
                "Cooldown Timer: ",
                Color.LightSalmon
            );
            Game.ScreenElementManager.AddText(
                "DebugConsole",
                (int)(MapWidth/coeff)-400,
                10,
                0,
                Font,
                "Console",
                Color.LightSalmon
            );
            Game.ScreenElementManager.AddText(
                "PlayingState",
                0,
                (int)MapHeight + 250,
                0,
                Font,
                "Playing State:",
                Color.DeepPink
            );
            Game.ScreenElementManager.AddText(
                "LifeState",
                0,
                (int)MapHeight + 270,
                0,
                Font,
                "Life State:",
                Color.DeepPink
            );
            Game.ScreenElementManager.AddText(
                "Health",
                0,
                (int)MapHeight + 290,
                0,
                Font,
                "Health:",
                Color.DeepPink
            );
            Game.ScreenElementManager.AddText(
                "Acceleration",
                0,
                (int)MapHeight + 310,
                0,
                Font,
                "Acceleration:",
                Color.Yellow
            );
            Game.ScreenElementManager.AddText(
                "G-Time",
                0,
                (int)MapHeight + 330,
                0,
                Font,
                "G-Time:",
                Color.Blue
            );
            Game.ScreenElementManager.AddText(
                "InitialVelocity",
                0,
                (int)MapHeight + 350,
                0,
                Font,
                "InitialVelocity:",
                Color.Blue
            );
            

        }

        private void Show()
        {
            Game.ScreenElementManager.GetImage("NormalMap").Show = true;
            Game.ScreenElementManager.GetImage("DepthMap").Show = true;
            Game.ScreenElementManager.GetImage("LightMap").Show = true;
            Game.ScreenElementManager.GetImage("SSAOMap").Show = true;
            Game.ScreenElementManager.GetText("Location").Show = true;
            Game.ScreenElementManager.GetText("FPS").Show = true;
            Game.ScreenElementManager.GetText("Ticks").Show = true;
            Game.ScreenElementManager.GetText("JumpState").Show = true;
            Game.ScreenElementManager.GetText("CurrentWeapon").Show = true;
            Game.ScreenElementManager.GetText("CurrentChamberAmmo").Show = true;
            Game.ScreenElementManager.GetText("CurrentReservoirAmmo").Show = true;
            Game.ScreenElementManager.GetText("Rotation").Show = true;
            Game.ScreenElementManager.GetText("SpeedState").Show = true;
            Game.ScreenElementManager.GetText("SprintTimer").Show = true;
            Game.ScreenElementManager.GetText("CooldownTimer").Show = true;
            Game.ScreenElementManager.GetText("PlayingState").Show = true;
            Game.ScreenElementManager.GetText("LifeState").Show = true;
            Game.ScreenElementManager.GetText("Health").Show = true;
            Game.ScreenElementManager.GetText("Acceleration").Show = true;
            Game.ScreenElementManager.GetText("G-Time").Show = true;
            Game.ScreenElementManager.GetText("InitialVelocity").Show = true;
            Game.ScreenElementManager.GetText("DebugConsole").Show = true;
        }
        private void Hide()
        {
            Game.ScreenElementManager.GetImage("NormalMap").Show = false;
            Game.ScreenElementManager.GetImage("DepthMap").Show = false;
            Game.ScreenElementManager.GetImage("LightMap").Show = false;
            Game.ScreenElementManager.GetImage("SSAOMap").Show = false;
            Game.ScreenElementManager.GetText("Location").Show = false;
            Game.ScreenElementManager.GetText("FPS").Show = false;
            Game.ScreenElementManager.GetText("Ticks").Show = false;
            Game.ScreenElementManager.GetText("JumpState").Show = false;
            Game.ScreenElementManager.GetText("CurrentWeapon").Show = false;
            Game.ScreenElementManager.GetText("CurrentChamberAmmo").Show = false;
            Game.ScreenElementManager.GetText("CurrentReservoirAmmo").Show = false;
            Game.ScreenElementManager.GetText("Rotation").Show = false;
            Game.ScreenElementManager.GetText("SpeedState").Show = false;
            Game.ScreenElementManager.GetText("SprintTimer").Show = false;
            Game.ScreenElementManager.GetText("CooldownTimer").Show = false;
            Game.ScreenElementManager.GetText("PlayingState").Show = false;
            Game.ScreenElementManager.GetText("LifeState").Show = false;
            Game.ScreenElementManager.GetText("Health").Show = false;
            Game.ScreenElementManager.GetText("Acceleration").Show = false;
            Game.ScreenElementManager.GetText("G-Time").Show = false;
            Game.ScreenElementManager.GetText("InitialVelocity").Show = false;
            Game.ScreenElementManager.GetText("DebugConsole").Show = false;
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
            if (!Game.Config.DebugScreen)
            {
                Hide();
                return;
            }
            Show();

            Game.ScreenElementManager.GetText("Location").Text = String.Format(
                "Location: x:{0}; y:{1}; z:{2}",
                (int)Game.ThisPlayer.Position.X,
                (int)Game.ThisPlayer.Position.Y,
                (int)Game.ThisPlayer.Position.Z
            );
            Game.ScreenElementManager.GetText("FPS").Text = String.Format(
                "FPS: {0}",
                FrameRate
            );
            Game.ScreenElementManager.GetText("Ticks").Text = String.Format(
                "Ticks per sec: {0}",
                UpdateRate
            );
            Game.ScreenElementManager.GetText("JumpState").Text = String.Format(
                "JumpState: {0}",
                Game.ThisPlayerPhysics.GravityState
            );
            Game.ScreenElementManager.GetText("CurrentWeapon").Text = String.Format(
                "CurrentWeapon: {0}",
                Game.ThisPlayer.CurrentWeapon.Name
            );
            Game.ScreenElementManager.GetText("CurrentChamberAmmo").Text = String.Format(
                "CurrentChamberAmmo: {0}",
                Game.ThisPlayer.CurrentWeapon.c_chamberAmmo
            );
            Game.ScreenElementManager.GetText("CurrentReservoirAmmo").Text = String.Format(
                "CurrentReservoirAmmo: {0}",
                Game.ThisPlayer.CurrentWeapon.c_reservoirAmmo
            );
            Game.ScreenElementManager.GetText("Rotation").Text = String.Format(
                "Rotation: x:{0}; y:{1}; z:{2}",
                (float)Game.ThisPlayer.Rotation.X,
                (float)Game.ThisPlayer.Rotation.Y,
                (float)Game.ThisPlayer.Rotation.Z
            );
            Game.ScreenElementManager.GetText("SpeedState").Text = String.Format(
                "Speed State: {0}",
                Game.ThisPlayer.CurrentSpeedState
            );
            Game.ScreenElementManager.GetText("SprintTimer").Text = String.Format(
                "Sprint Timer: {0}",
                Game.ThisPlayer.SprintTimer
            );
            Game.ScreenElementManager.GetText("CooldownTimer").Text = String.Format(
                "CooldownTimer: {0}",
                Game.ThisPlayer.CooldownTimer
            );
            Game.ScreenElementManager.GetText("PlayingState").Text = String.Format(
                "Playing State: {0}",
                Game.CurrentPlayingState
            );
            Game.ScreenElementManager.GetText("LifeState").Text = String.Format(
                "Life State: {0}",
                Game.ThisPlayer.CurrentLifeState
            );
            Game.ScreenElementManager.GetText("Health").Text = String.Format(
                "Health: {0}",
                Game.ThisPlayer.Health
            );
            Game.ScreenElementManager.GetText("Acceleration").Text = String.Format(
                "Acceleration: x:{0}; y:{1}; z:{2}",
                (float)Game.ThisPlayerPhysics.GetAcceleration().X,
                (float)Game.ThisPlayerPhysics.GetAcceleration().Y,
                (float)Game.ThisPlayerPhysics.GetAcceleration().Z
            );
            Game.ScreenElementManager.GetText("G-Time").Text = String.Format(
                "G-Time: {0}",
                Game.ThisPlayerPhysics.t
            );
            Game.ScreenElementManager.GetText("InitialVelocity").Text = String.Format(
                "InitialVelocity: {0}",
                Game.ThisPlayerPhysics.vi
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
            Game.ScreenElementManager.GetText("DebugConsole").Text = finalDebug;
        }
    }
}
