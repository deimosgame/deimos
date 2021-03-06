﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;

namespace Deimos
{
    class LoadingLevelGS<T> : GameStateObj
    {
        Action PostLoadingEvent;

        public override GameStates GameState
        {
            get { return GameStates.LoadingLevel; }
        }

        public LoadingLevelGS(Action myEvent)
        {
            PostLoadingEvent = myEvent;
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
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += new DoWorkEventHandler((sender, e) =>
            {
                
            });
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler((sender, e) =>
            {
                //System.Threading.Thread.Sleep(1000);
                
            });

            GeneralFacade.SceneManager.SetScene<T>(); 
            PostLoadingEvent();
            //bw.RunWorkerAsync();
        }

        public override void PostUnset()
        {
            DisplayFacade.ScreenElementManager.RemoveRectangle("LoadingScreenBackground");
            DisplayFacade.ScreenElementManager.RemoveText("LoadingScreenTitle");



            if (NetworkFacade.Local)
            {
                GameplayFacade.ChatInterface.AddChatInput("You are playing Deimos alone... or are you?");
                GameplayFacade.ChatInterface.AddChatInput("Type '/help' to see commands list");
            }
            else
            {
                GameplayFacade.ChatInterface.AddChatInput("Have fun annihilating everyone");
            }
        }
    }
}
