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
            Map = "Models/Map/hl/hl";
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
            Spawns.Add(0x00, new SpawnLocation(new Vector3(10, 8, 10), Vector3.Zero));

            // Player 2 spawn location
            // note: access with key '1'
            Spawns.Add(0x01, new SpawnLocation(new Vector3(20, 8, 20), Vector3.Zero));
        }

        public void Load()
        {
            GeneralFacade.GameStateManager.Set(new LoadingLevelGS<SceneKnifeMG>(delegate() { } ));
            GeneralFacade.GameStateManager.Set(new MGSpawningGS("knife"));
            GeneralFacade.GameStateManager.Set(new PlayingGS());
        }

        public void Terminate()
        {

        }

        public void Update(float dt)
        {
        }
    }
}
