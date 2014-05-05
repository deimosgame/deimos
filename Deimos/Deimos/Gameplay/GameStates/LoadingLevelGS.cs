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
                "LoadingScreenBackground",
                0,
                0,
                1,
                GeneralFacade.Game.GraphicsDevice.Viewport.Width,
                GeneralFacade.Game.GraphicsDevice.Viewport.Height,
                Color.Black
            );
            DisplayFacade.ScreenElementManager.AddText(
                "LoadingScreenTitle",
                20,
                GeneralFacade.Game.GraphicsDevice.Viewport.Height - 20,
                1,
                DisplayFacade.DebugFont,
                "Loading...",
                Color.White
            );

            GeneralFacade.SceneManager.SetScene<T>();
        }

        public override void PostUnset()
        {
            DisplayFacade.ScreenElementManager.RemoveRectangle("LoadingScreenBackground");
            DisplayFacade.ScreenElementManager.RemoveText("LoadingScreenTitle");
        }
    }
}
