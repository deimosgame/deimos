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
            SceneManager = sceneManager;
            ModelManager = SceneManager.ModelManager;
            LightManager = SceneManager.LightManager;

            ModelManager.LoadModel(
                 "ourMap",
                 "Models/Map/ourMap", // Model
                 new Vector3(10, 0, 0), // Location
                 Vector3.Zero
            );
            ModelManager.LoadModel(
                "PP19",
                "Models/MISC/PP19/PP19Model",
                Vector3.Zero,
                new Vector3(0, 1, 0),
                0.5f,
                LevelModel.CollisionType.None
            );
        }

        // Destructor
        ~SceneDeimos()
        {
            ModelManager.UnloadModels();
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
        // Update our things at each ticks
        public override void Update()
        {
            
        }
    }
}
