using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Deimos
{
    class Knife : Minigame
    {
        public Knife()
        {
            Type = MinigameType.Knife;
            Name = "knife";
            Map = "Models/Map/arena/arenagrounds/arena1";
            TimeLimit = 180000;
            LinearJump = true;
            Falldamage = false;
            Gravity = 9.8f;
            SpeedRate = 1;

            PlayerOneClass = Player.Spec.Cutthroat;
            PlayerTwoClass = Player.Spec.Cutthroat;

            Spawns = new Dictionary<byte, SpawnLocation>();

            // Player 1 spawn location
            // note: access with key '0'
            Spawns.Add(0x00, new SpawnLocation(new Vector3(21, 11, -69), new Vector3(-0.02f, 0.7f, 0)));

            // Player 2 spawn location
            // note: access with key '1'
            Spawns.Add(0x01, new SpawnLocation(new Vector3(21, 11, 74), new Vector3(0.02f, -2.3f, 0)));
        }

        public void Load()
        {
            GameplayFacade.ThisPlayer.NextInstance = Name;
            GameplayFacade.ThisPlayer.MainClass = GameplayFacade.ThisPlayer.Class;
            GameplayFacade.ThisPlayer.Class = Player.Spec.Cutthroat;

            GeneralFacade.GameStateManager.Set(new LoadingLevelGS<SceneKnifeMG>(delegate() { } ));
            GeneralFacade.GameStateManager.Set(new MGSpawningGS(GameplayFacade.ThisPlayer.NextInstance));
            GeneralFacade.GameStateManager.Set(new PlayingGS());
        }
        
        public void Terminate()
        {
            GameplayFacade.ThisPlayer.PlayerKill();

            GameplayFacade.ThisPlayer.NextInstance = "main";
            GameplayFacade.ThisPlayer.Class = GameplayFacade.ThisPlayer.MainClass;

            GeneralFacade.GameStateManager.Set(new LoadingLevelGS<SceneCompound>(delegate() { }));
            GeneralFacade.GameStateManager.Set(new RespawnGS(GameplayFacade.ThisPlayer.NextInstance, true));
            GeneralFacade.GameStateManager.Set(new PlayingGS());
        }

        public void Update(float dt)
        {

        }
    }
}
