using Microsoft.Xna.Framework;
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
            SceneManager = sceneManager;
            ModelManager = SceneManager.ModelManager;
            LightManager = SceneManager.LightManager;

            ModelManager.LoadModel(
                 "skatepark",
                 "Models/Map/arena", // Model
                 new Vector3(0, 0, 0), // Location
                 LevelModel.CollisionType.Accurate
            );
        }

        // Destructor
        ~SceneSkatePark()
        {
            ModelManager.UnloadModels();
        }

        // Initialize our lights and such
        public override void Initialize()
        {
            LightManager.AddPointLight(
               "player",
               new Vector3(0, 0, 0), // Location
               100, // Radius
               2, // Intensity
               Color.White
            );
        }

        // Update our things at each ticks
        float coeff = 0.2f;
        public override void Update()
        {
            LightManager.GetPointLight("player").Position = 
                SceneManager.Game.Camera.CameraPosition;
            PointLight light = LightManager.GetPointLight("player");
            if (light.Radius <= 90 || light.Radius >= 130)
            {
                coeff = -coeff;
            }
            light.Radius += coeff;
        }
    }
}
