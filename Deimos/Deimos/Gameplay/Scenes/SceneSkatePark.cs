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
            ModelManager.LoadModel(
                 "bonasse",
                 "Models/Characters/Ana_Model", // Model
                 new Vector3(0, 0, 0), // Location
                 LevelModel.CollisionType.None
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
               new Vector3(10, 10, 10), // Location
               400, // Radius
               2, // Intensity
               Color.White
            );
            LightManager.AddSpotLight(
                "test",
                new Vector3(0, 10, 0),
                new Vector3(0, 1, 0),
                Color.White,
                20
            );
        }

        // Update our things at each ticks
        public override void Update()
        {

        }
    }
}
