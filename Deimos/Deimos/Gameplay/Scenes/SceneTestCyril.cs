using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    class SceneTestCyril : SceneTemplate
    {
        SceneManager SceneManager;
        ModelManager ModelManager;
        LightManager LightManager;

        // Constructor
        public SceneTestCyril(SceneManager sceneManager)
        {
            SceneManager = sceneManager;
            ModelManager = SceneManager.ModelManager;
            LightManager = SceneManager.LightManager;

            ModelManager.LoadModel(
                 "house",
                 "Models/Map/Cyril", // Model
                 new Vector3(0, 0, 0) // Location
             );
        }

        // Destructor
        ~SceneTestCyril()
        {
            ModelManager.UnloadModels();
        }

        // Initialize our lights and such
        public override void Initialize()
        {
            LightManager.AddPointLight(
               "player",
               new Vector3(0, 10, 0), // Location
               50, // Radius
               2, // Intensity
               Color.White
            );
        }

        // Update our things at each ticks
        public override void Update()
        {
            LightManager.GetPointLight("player").Position =
                SceneManager.Game.Camera.CameraPosition;
        }
    }
}
