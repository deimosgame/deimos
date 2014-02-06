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
                 new Vector3(10, 0, 0) // Location
            );
            ModelManager.LoadModel(
                "PP19",
                "Models/MISC/PP19/PP19Model",
                Vector3.Zero,
                0.5f
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
               300, // Radius
               2, // Intensity
               Color.CadetBlue
            );
            LightManager.AddSpotLight(
                "test",
                new Vector3(0, 10, 0),
                new Vector3(0, 1, 0),
                Color.White,
                20
            );
        }

        public float x = -1f;
        Random rnd = new Random();
        public int i = 0;
        public int r = 0;

        // Update our things at each ticks
        public override void Update()
        {
            PointLight light = LightManager.GetPointLight("player");
            r = rnd.Next(0, 10);
            if (i == r)
            {
                i = 0;
                
                light.Intensity += x;
                x = -x;
            }

            if (i > 9)
            {
                i = 0;
            }

            i++;
        }
    }
}
