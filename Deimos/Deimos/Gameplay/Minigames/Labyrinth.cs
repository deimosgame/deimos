using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    class Labyrinth : Minigame
    {
        public Labyrinth()
        {
            Type = MinigameType.Labyrinth;
            Name = "labyrinth";
            Map = "Models/Map/Minigames/Knife/KnifeFight";
            TimeLimit = 120000;
            LinearJump = true;
            Falldamage = false;
            Gravity = 9.8f;
            SpeedRate = 1;

            PlayerOneClass = Player.Spec.Victim;
            PlayerTwoClass = Player.Spec.Victim;

            Spawns = new Dictionary<byte, SpawnLocation>();

            // Player 1 spawn location
            // note: access with key '0'
            Spawns.Add(0x00, new SpawnLocation(new Vector3(-35, 4, -25), new Vector3(-0.02f, 0.7f, 0)));

            // Player 2 spawn location
            // note: access with key '1'
            Spawns.Add(0x01, new SpawnLocation(new Vector3(20, 4, 40), new Vector3(0.02f, -2.3f, 0)));
        }

        public void Load()
        {
            GameplayFacade.ThisPlayer.NextInstance = Name;
            GameplayFacade.ThisPlayer.MainClass = GameplayFacade.ThisPlayer.Class;
            GameplayFacade.ThisPlayer.Class = Player.Spec.Victim;

            GeneralFacade.GameStateManager.Set(new LoadingLevelGS<SceneKnifeMG>(delegate() { }));
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
