using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Deimos
{
    class SceneDeimos : SceneTemplate
    {
        SceneManager SceneManager;
        ModelManager ModelManager;
        LightManager LightManager;

        // Constructor
        public SceneDeimos(SceneManager sceneManager)
        {
            PlayerSize = new Vector3(33f, 1f, 1f);
            SceneManager = sceneManager;
            ModelManager = SceneManager.ModelManager;
            LightManager = SceneManager.LightManager;
        }

        // Destructor
        ~SceneDeimos()
        {
            ModelManager.UnloadModels();
        }

        // Load our models and such
        public override void Load()
        {
            ModelManager.LoadModel(
                 "ourMap",
                 "Models/Map/hl/hl", // Model
                 new Vector3(10, 0, 0), // Location
                 Vector3.Zero
            );
            ModelManager.LoadModel(
                "PP19",
                "Models/MISC/PP19/PP19Model",
                Vector3.Zero,
                new Vector3(0, 1, 0),
                0.1f,
                LevelModel.CollisionType.None
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
               Color.White
            );
        }
        // Update our things at each ticks
        public override void Update()
        {
            
        }
    }
}
