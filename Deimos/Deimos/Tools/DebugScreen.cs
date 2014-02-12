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
            float WindowWidth = Game.Renderer.NormalRT.Width;
            float WindowHeight = Game.Renderer.NormalRT.Height;
            float MapWidth = WindowWidth * coeff;
            float MapHeight = WindowHeight * coeff;
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
                (int)MapHeight + 30,
                0,
                Font,
                "FPS: ",
                Color.LightBlue
            );
            Game.ScreenElementManager.AddText(
                "Ticks",
                0,
                (int)MapHeight + 50,
                0,
                Font,
                "Ticks per sec: ",
                Color.LightBlue
            );
            Game.ScreenElementManager.AddText(
                "JumpState",
                0,
                (int)MapHeight + 70,
                0,
                Font,
                "JumpState: ",
                Color.White
            );
            Game.ScreenElementManager.AddText(
                "BunnyCoeff",
                0,
                (int)MapHeight + 90,
                0,
                Font,
                "BunnyCoeff: ",
                Color.Green
            );
            Game.ScreenElementManager.AddText(
                "CurrentWeapon",
                0,
                (int)MapHeight + 110,
                0,
                Font,
                "CurrentWeapon:",
                Color.Red
            );
            Game.ScreenElementManager.AddText(
                "CurrentChamberAmmo",
                0,
                (int)MapHeight + 130,
                0,
                Font,
                "CurrentChamberAmmo:",
                Color.Red
            );
            Game.ScreenElementManager.AddText(
                "CurrentReservoirAmmo",
                0,
                (int)MapHeight + 150,
                0,
                Font,
                "CurrentReservoirAmmo:",
                Color.Red
            );
            Game.ScreenElementManager.AddText(
                "DebugConsole",
                0,
                (int)WindowHeight - 400,
                0,
                Font,
                "Debug",
                Color.White
            );
        }

        private void Show()
        {
            Game.ScreenElementManager.GetImage("NormalMap").Show = true;
            Game.ScreenElementManager.GetImage("DepthMap").Show = true;
            Game.ScreenElementManager.GetImage("LightMap").Show = true;
            Game.ScreenElementManager.GetText("Location").Show = true;
            Game.ScreenElementManager.GetText("FPS").Show = true;
            Game.ScreenElementManager.GetText("Ticks").Show = true;
            Game.ScreenElementManager.GetText("JumpState").Show = true;
            Game.ScreenElementManager.GetText("BunnyCoeff").Show = true;
            Game.ScreenElementManager.GetText("CurrentWeapon").Show = true;
            Game.ScreenElementManager.GetText("CurrentChamberAmmo").Show = true;
            Game.ScreenElementManager.GetText("CurrentReservoirAmmo").Show = true;
        }
        private void Hide()
        {
            Game.ScreenElementManager.GetImage("NormalMap").Show = false;
            Game.ScreenElementManager.GetImage("DepthMap").Show = false;
            Game.ScreenElementManager.GetImage("LightMap").Show = false;
            Game.ScreenElementManager.GetText("Location").Show = false;
            Game.ScreenElementManager.GetText("FPS").Show = false;
            Game.ScreenElementManager.GetText("Ticks").Show = false;
            Game.ScreenElementManager.GetText("JumpState").Show = false;
            Game.ScreenElementManager.GetText("BunnyCoeff").Show = false;
            Game.ScreenElementManager.GetText("CurrentWeapon").Show = false;
            Game.ScreenElementManager.GetText("CurrentChamberAmmo").Show = false;
            Game.ScreenElementManager.GetText("CurrentReservoirAmmo").Show = false;
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
                Game.ThisPlayerPhysics.State
            );
            Game.ScreenElementManager.GetText("BunnyCoeff").Text = String.Format(
                "BunnyCoeff: {0}",
                Game.ThisPlayerPhysics.BunnyhopCoeff
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

            string finalDebug = "";
            foreach (string line in DebugConsole)
            {

            }
        }
    }
}
