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

        public void Set(GameStateObj gs)
        {
            if (CurrentGameStateObj != null)
            {
                CurrentGameStateObj.PostUnset();
            }

            CurrentGameStateObj = gs;
            CurrentGameState = gs.GameState;

            CurrentGameStateObj.PreSet();
        }
    }
}
