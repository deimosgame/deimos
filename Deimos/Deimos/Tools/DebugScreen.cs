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

            Game.ScreenElementManager.AddText(
                "Location",
                0,
                (int)MapHeight + 10,
                0,
                Font,
                "Location: ",
                Color.White
            );
        }

        private void Show()
        {
            Game.ScreenElementManager.GetImage("NormalMap").Show = true;
            Game.ScreenElementManager.GetImage("DepthMap").Show = true;
            Game.ScreenElementManager.GetImage("LightMap").Show = true;
            Game.ScreenElementManager.GetText("Location").Show = true;
        }
        private void Hide()
        {
            Game.ScreenElementManager.GetImage("NormalMap").Show = false;
            Game.ScreenElementManager.GetImage("DepthMap").Show = false;
            Game.ScreenElementManager.GetImage("LightMap").Show = false;
            Game.ScreenElementManager.GetText("Location").Show = false;
        }

        public void Draw(GameTime gameTime)
        {
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
        }
    }
}
