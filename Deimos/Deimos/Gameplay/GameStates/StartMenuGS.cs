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

            // On veut qu'elle soit handle par les screen elements (pour qu'on puisse cliquer)
            DisplayFacade.ScreenElementManager.HandleMouse();

            // On retire la derniere image de l'intro
            DisplayFacade.ScreenElementManager.RemoveImage("Intro");

            // On affiche les éléments du menu
            DisplayFacade.ScreenElementManager.AddRectangle(
                "StartScreenMenuPlay",
                50,
                200,
                1,
                30,
                200,
                Color.Red,
                delegate(ScreenElement el, DeimosGame game)
                {
                    GeneralFacade.GameStateManager.Set<SpawningGS>();
                    GeneralFacade.GameStateManager.Set<PlayingGS>();
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
                "StartScreenMenuExit",
                50,
                250,
                1,
                30,
                200,
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
            DisplayFacade.ScreenElementManager.RemoveRectangle("StartScreenMenuExit");
            DisplayFacade.ScreenElementManager.RemoveImage("applelogo");
        }
    }
}
