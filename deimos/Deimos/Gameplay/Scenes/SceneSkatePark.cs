﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    class SceneSkatePark : SceneTemplate
    {
        SceneManager SceneManager;
        ModelManager ModelManager;
        LightManager LightManager;

        // Constructor
        public SceneSkatePark(SceneManager sceneManager)
        {
            PlayerSize = new Vector2(1, 33);
            //SpawnLocations = new SpawnLocation[] {
            //    new SpawnLocation(new Vector3(-60f, 20f, -8f), Vector3.Zero)
            //};

            SceneManager = sceneManager;
            ModelManager = SceneManager.ModelManager;
            LightManager = SceneManager.LightManager;
        }

        // Destructor
        ~SceneSkatePark()
        {
            //
        }

        // Load our models and such
        public override void Load()
        {
            ModelManager.LoadModel(
                 "skatepark",
                 "Models/Map/arena", // Model
                 new Vector3(10, 0, 0), // Location
                 Vector3.Zero
            );
            SceneManager.CollisionManager.AddCollisionBox(
                new Vector3(-1000, -80, -1000),
                new Vector3(1000, -81, 1000),
                delegate(CollisionElement element, DeimosGame game)
                {
                    GameplayFacade.ThisPlayer.Position = new Vector3(0, 80, 20);
                }
            );
        }

        // Initialize our lights and such
        public override void Initialize()
        {
            LightManager.AddPointLight(
               "player",
               new Vector3(10, 10, 10), // Location
               300, // Radius
               2, // Intensity
               Color.CadetBlue
            );
        }

        //public float x = -1f;
        //Random rnd = new Random();
        //public int i = 0;
        //public int r = 0;

        // Update our things at each ticks
        public override void Update(float dt)
        {
            //PointLight light = LightManager.GetPointLight("player");
            //r = rnd.Next(0, 10);
            //if (i == r)
            //{
            //    i = 0;
                
            //    light.Intensity += x;
            //    x = -x;
            //}

            //if (i > 9)
            //{
            //    i = 0;
            //}

            //i++;
        }
    }
}
