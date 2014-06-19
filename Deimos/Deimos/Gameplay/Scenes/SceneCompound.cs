using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Tranquillity;
using Microsoft.Xna.Framework.Graphics;

namespace Deimos
{
    class SceneCompound : SceneTemplate
    {
        SceneManager SceneManager;
        ModelManager ModelManager;
        LightManager LightManager;
        SoundManager SoundManager;
        ModelAnimationManager ModelAnimationManager;

        string token_rl;
        string token_gravity;
        string token_speed;
        string token_health;
        string token_secret;

        public SceneCompound(SceneManager sceneManager)
        {
            PlayerSize = new Vector2(6, 3);

            SpawnLocations = new SpawnLocation[] {
                new SpawnLocation(new Vector3(18, 10, 90), Vector3.Zero)
            };

            SceneManager = sceneManager;
            ModelManager = SceneManager.ModelManager;
            LightManager = SceneManager.LightManager;
            SoundManager = SceneManager.SoundManager;
            ModelAnimationManager = DisplayFacade.ModelAnimationManager;

            Objects = new ObjectManager();
            Secrets = new SecretsManager();
        }

        // Destructor
        ~SceneCompound()
        {
            //
        }

        public override void Load()
        {

                // Loading the map
            ModelManager.LoadModel(
                 "ourMap",
                 "Models/Map/Compound/DeimosCompound", // Model
                 new Vector3(0, 0, 0), // Location
                 Vector3.Zero,
                 0.2f
            );

            token_rl = Objects.AddWeapon("BazookaPickup",
                2,
                new Vector3(17, -6, 110),
                PickupObject.State.Active,
                30
                );

            token_health = Objects.AddEffect("Health Pack",
                new Vector3(1, -6, 96),
                PickupObject.State.Active,
                50,
                15
                );

            token_speed = Objects.AddEffect("Speed Boost",
                new Vector3(32, -6, 87),
                PickupObject.State.Active,
                15,
                10,
                5
                );

            token_gravity = Objects.AddEffect("Gravity Boost",
                new Vector3(32, -5, 106),
                PickupObject.State.Active,
                1,
                10,
                5
                );

            token_secret = Secrets.AddWall("Bricks", "Models/SecretWalls/Brick/SecretWallBrick",
                new Vector3(12, 29, 12), SecretObject.State.Undiscovered,
                3,
                new Vector2(10,30),
                1);

            SoundManager.AddSoundEffect("scary", "Sounds/ScaryMusic");
        }

        // Initialize our lights and such
        public override void Initialize()
        {
            LightManager.AddPointLight(
               "Main",
               new Vector3(18, 10, 90), // Location
               30, // Radius
               2, // Intensity
               Color.Red
            );
            LightManager.AddPointLight(
                "blue1",
                new Vector3(-40, 7, 26),
                25,
                2,
                Color.DarkRed
                );
            LightManager.AddPointLight(
                "blue2",
                new Vector3(-40, 3, 71),
                25,
                2,
                Color.DarkRed
                );
            LightManager.AddPointLight(
                "white1",
                new Vector3(-40, 22, 71),
                20,
                1,
                Color.White
                );
            LightManager.AddPointLight(
                "white2",
                new Vector3(-1, 5, 46),
                20,
                1,
                Color.White
                );
            LightManager.AddPointLight(
                "white3",
                new Vector3(20, 0, 14),
                13,
                2,
                Color.White
                );
            LightManager.AddPointLight(
                "white4",
                new Vector3(15, 0, -30),
                20,
                2,
                Color.White
                );
            LightManager.AddPointLight(
                "white5",
                new Vector3(35, 20, 50),
                20,
                1,
                Color.White
                );
            LightManager.AddPointLight(
                "mainpillar",
                new Vector3(-10, 15, 175),
                30,
                2,
                Color.White
                );

            SoundManager.Play3D("scary", DisplayFacade.Camera.Position,
                new Vector3(0, 0, -0));

            //foreach (KeyValuePair<byte, Player> p in NetworkFacade.Players)
            //{
            //    ModelManager.LoadModel(
            //        p.Value.Name,
            //        p.Value.GetModelName(),
            //        p.Value.Position,
            //        p.Value.Rotation,
            //        5,
            //        LevelModel.CollisionType.None
            //        );

            //    ModelManager.GetLevelModel(p.Value.Name).show = true;
            //}
        }

        // Update our things at each ticks
        public override void Update(float dt)
        {
            SoundManager.SetListener("scary", GameplayFacade.ThisPlayer.Position);

            Objects.Update(GameplayFacade.ThisPlayer.dt);
            Secrets.Update(GameplayFacade.ThisPlayer.dt);


            if (NetworkFacade.IsMultiplayer)
            {
                for (int i = 0; i < NetworkFacade.Players.Count; i++)
                {
                    KeyValuePair<byte, Player> pair = NetworkFacade.Players.ElementAt(i);

                    if (pair.Value != null
                        && GeneralFacade.SceneManager.ModelManager.LevelModelExists(pair.Value.Name))
                    {
                        if (pair.Value.IsAlive())
                        {
                            GeneralFacade.SceneManager.ModelManager.GetLevelModel(pair.Value.Name).show = true;
                        }
                        else
                        {
                            GeneralFacade.SceneManager.ModelManager.GetLevelModel(pair.Value.Name).show = false;
                        }

                        GeneralFacade.SceneManager.ModelManager.GetLevelModel(pair.Value.Name).Position =
                            pair.Value.Position;

                        GeneralFacade.SceneManager.ModelManager.GetLevelModel(pair.Value.Name).Rotation =
                            pair.Value.Rotation;
                    }
                }
            }

            if (Elapsed < TimeLimit)
            {
                Elapsed += dt;
            }
        }
    }
}
