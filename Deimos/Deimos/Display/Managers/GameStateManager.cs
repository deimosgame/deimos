using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    public enum GameStates
    {
        IntroStarting,
        Intro,
        StartMenu,
        Pause,
        GraphicOptions,
        Spawning,
        Playing
    }

    class GameStateManager
    {
        protected GameStateObj CurrentGameStateObj = null;
        public GameStates CurrentGameState
        {
            get;
            private set;
        }

        public GameStateManager()
        {
            // Default gamestate
            CurrentGameState = GameStates.IntroStarting;
        }

        public void Set<T>()
        {
            if (CurrentGameStateObj != null)
            {
                CurrentGameStateObj.PostUnset();
            }

            CurrentGameStateObj = (GameStateObj)Activator.CreateInstance(typeof(T), new object[] {  });
            CurrentGameState = CurrentGameStateObj.GameState;

            CurrentGameStateObj.PreSet();
        }
    }
}
