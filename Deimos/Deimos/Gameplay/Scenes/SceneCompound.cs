﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Tranquillity;
using Microsoft.Xna.Framework.Graphics;
using XNAnimation.Controllers;
using XNAnimation;

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
            PlayerSize = new Vector2(5.5f, 3);

            Spawns = new Dictionary<byte, SpawnLocation>();

            Spawns.Add(0, new SpawnLocation(new Vector3(7, 10, -21), new Vector3(0.05f, 0.2f, 0)));
            Spawns.Add(1, new SpawnLocation(new Vector3(22, 10, -31), new Vector3(0.05f, -1, 0)));
            Spawns.Add(2, new SpawnLocation(new Vector3(20, 2, 15), new Vector3(0.05f, -2.3f, 0)));
            Spawns.Add(3, new SpawnLocation(new Vector3(-41, 2, 23), new Vector3(0.05f, 1.5f, 0)));
            Spawns.Add(4, new SpawnLocation(new Vector3(-21, 2, 60), new Vector3(0.05f, -0.6f, 0)));
            Spawns.Add(5, new SpawnLocation(new Vector3(-5, 2, 177), new Vector3(0.05f, -3f, 0)));
            Spawns.Add(6, new SpawnLocation(new Vector3(-51, 17, 64), new Vector3(0.05f, 1.5f, 0)));


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
                 "Models/Map/d_compound2/dfinal", // Model
                 new Vector3(0, 0, 0), // Location
                 Vector3.Zero,
                 1
            );

            if (!NetworkFacade.Local)
            {
                //foreach (KeyValuePair<byte, Player> p in NetworkFacade.Players)
                //{
                //    if (!GeneralFacade.SceneManager.ModelManager.LevelModelExists(p.Value.Name))
                //    {
                //        GeneralFacade.SceneManager.ModelManager.LoadModel(
                //            p.Value.Name,
                //            p.Value.GetModelName(),
                //            p.Value.Position,
                //            p.Value.Rotation,
                //            5,
                //            LevelModel.CollisionType.None
                //        );
                //    }
                //}
            }

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
                1
            );


            // Animations
            //ModelManager.LoadModel(
            //     "dude",
            //     "Models/Characters/dude/dude", // Model
            //     new Vector3(15, 1, -30), // Location
            //     Vector3.Zero,
            //     0.2f,
            //     LevelModel.CollisionType.Accurate
            //);



            SoundManager.AddSoundEffect("scary", "Sounds/ScaryMusic");

            if (!NetworkFacade.Local)
            {
                NetworkFacade.Players.LoadModels();
            }
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
        }

        // Update our things at each ticks
        public override void Update(float dt)
        {
            if (!NetworkFacade.Local)
            {
                NetworkFacade.Players.Update();
            }

            SoundManager.SetListener("scary", GameplayFacade.ThisPlayer.Position);

            Objects.Update(GameplayFacade.ThisPlayer.dt);
            Secrets.Update(GameplayFacade.ThisPlayer.dt);

            if (Elapsed < TimeLimit)
            {
                Elapsed += dt;
            }
        }
    }
}
