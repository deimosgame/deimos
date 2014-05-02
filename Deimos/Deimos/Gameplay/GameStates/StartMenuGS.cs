using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    class StartMenuGS : GameStateObj
    {
        public override GameStates GameState
        {
            get { return GameStates.StartMenu; }
        }

        public override void PreSet()
        {
            //On veut afficher la souris
            GeneralFacade.Game.IsMouseVisible = true;

            // On affiche les éléments du menu
            DisplayFacade.ScreenElementManager.AddRectangle(
                "StartScreenMenuPlay",
                50,
                200,
                1,
                200,
                30,
                Color.Red,
                delegate(ScreenElement el, DeimosGame game)
                {
                    GeneralFacade.GameStateManager.Set(new LoadingLevelGS<SceneCompound>());
                    GeneralFacade.GameStateManager.Set(new SpawningGS());
                    GeneralFacade.GameStateManager.Set(new PlayingGS());
                },
                delegate(ScreenElement el, DeimosGame game)
                {
                    ScreenRectangle t = DisplayFacade.ScreenElementManager.GetRectangle("StartScreenMenuPlay");
                    t.Color = Color.Purple;
                },
                delegate(ScreenElement el, DeimosGame game)
                {
                    ScreenRectangle t = DisplayFacade.ScreenElementManager.GetRectangle("StartScreenMenuPlay");
                    t.Color = Color.Red;
                }
            );
            DisplayFacade.ScreenElementManager.AddRectangle(
                "StartScreenMenuServerList",
                50,
                250,
                1,
                200,
                30,
                Color.Blue,
                delegate(ScreenElement el, DeimosGame game)
                {
                    GeneralFacade.GameStateManager.Set(new ServerListMenuGS());
                }
            );
            DisplayFacade.ScreenElementManager.AddRectangle(
                "StartScreenMenuExit",
                50,
                300,
                1,
                200,
                30,
                Color.Green,
                delegate(ScreenElement el, DeimosGame game)
                {
                    GeneralFacade.Game.Exit();
                }
            );
            //DisplayFacade.ScreenElementManager.AddImage("applelogo", 500, 500, 1, 1, GeneralFacade.Game.Content.Load<Texture2D>("Images/Menu/apple"));
        }

        public override void PostUnset()
        {
            DisplayFacade.ScreenElementManager.RemoveRectangle("StartScreenMenuPlay");
            DisplayFacade.ScreenElementManager.RemoveRectangle("StartScreenMenuServerList");
            DisplayFacade.ScreenElementManager.RemoveRectangle("StartScreenMenuExit");
        }
    }
}
