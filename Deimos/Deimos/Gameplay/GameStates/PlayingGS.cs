using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    class PlayingGS : GameStateObj
    {
        public override GameStates GameState
        {
            get { return GameStates.Playing; }
        }

        public override void PreSet()
        {
            GeneralFacade.Game.IsMouseVisible = false;

            Mouse.SetPosition(GeneralFacade.Game.GraphicsDevice.Viewport.Width / 2,
                    GeneralFacade.Game.GraphicsDevice.Viewport.Height / 2);


            int midScreenWidth = GeneralFacade.Game.GraphicsDevice.Viewport.Width / 2;
            int midScreenHeight = GeneralFacade.Game.GraphicsDevice.Viewport.Height / 2;

            int crosshairWidth = 20;

            DisplayFacade.ScreenElementManager.AddLine(
                "crosshairx",
                new Vector2(midScreenWidth, midScreenHeight - (crosshairWidth / 2)),
                new Vector2(midScreenWidth, midScreenHeight + (crosshairWidth / 2)),
                1,
                Color.White
            );
            DisplayFacade.ScreenElementManager.AddLine(
                "crosshairy",
                new Vector2(midScreenWidth - (crosshairWidth / 2), midScreenHeight),
                new Vector2(midScreenWidth + (crosshairWidth / 2), midScreenHeight),
                1,
                Color.White
            );
            DisplayFacade.ScreenElementManager.AddText(
                "life",
                (int)(GeneralFacade.Game.GraphicsDevice.Viewport.Width - DisplayFacade.UIFont.MeasureString("100/100").X - 20),
                (int)(GeneralFacade.Game.GraphicsDevice.Viewport.Height - DisplayFacade.UIFont.MeasureString("100/100").Y - 20),
                1,
                DisplayFacade.UIFont,
                "100/100",
                Color.White
            );
            DisplayFacade.ScreenElementManager.AddText(
                "ammo",
                20,
                (int)(GeneralFacade.Game.GraphicsDevice.Viewport.Height - DisplayFacade.UIFont.MeasureString("100/100").Y - 20),
                1,
                DisplayFacade.UIFont,
                "0/0",
                Color.White
            );
        }

        public override void PostUnset()
        {
            DisplayFacade.ScreenElementManager.RemoveLine("crosshairx");
            DisplayFacade.ScreenElementManager.RemoveLine("crosshairy");
            DisplayFacade.ScreenElementManager.RemoveText("life");
            DisplayFacade.ScreenElementManager.RemoveText("ammo");
        }
    }
}
