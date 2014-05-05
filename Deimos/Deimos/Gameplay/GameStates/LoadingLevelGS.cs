using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    class LoadingLevelGS<T> : GameStateObj
    {
        public override GameStates GameState
        {
            get { return GameStates.LoadingLevel; }
        }

        public override void PreSet()
        {
            DisplayFacade.ScreenElementManager.AddRectangle(
                "blackScreen",
                0,
                0,
                1,
                GeneralFacade.Game.GraphicsDevice.Viewport.Height,
                GeneralFacade.Game.GraphicsDevice.Viewport.Width,
                Color.Black
            );
            DisplayFacade.ScreenElementManager.AddText(
                "stringLoading",
                300,
                GeneralFacade.Game.GraphicsDevice.Viewport.Height / 2,
                1,
                DisplayFacade.DebugFont,
                "Loading...",
                Color.White
            );

            GeneralFacade.SceneManager.SetScene<T>();
        }

        public override void PostUnset()
        {
            DisplayFacade.ScreenElementManager.RemoveRectangle("blackScreen");
            DisplayFacade.ScreenElementManager.RemoveText("stringLoading");
        }
    }
}
