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
        SoundManager SoundManager;

        // Constructor
        public SceneDeimos(SceneManager sceneManager)
        {
            PlayerSize = new Vector3(20, 0.2f, 0.2f);
            SceneManager = sceneManager;
            ModelManager = SceneManager.ModelManager;
            LightManager = SceneManager.LightManager;
            SoundManager = SceneManager.SoundManager;
        }

        // Destructor
        ~SceneDeimos()
        {
            //
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
            SoundManager.AddSoundEffect("scary", "Sounds/ScaryMusic");
        }

        // Initialize our lights and such
        public override void Initialize()
        {
            LightManager.AddPointLight(
               "Main",
               new Vector3(10, 10, 10), // Location
               300, // Radius
               2, // Intensity
               Color.White
            );
            SoundManager.Play3D("scary", SceneManager.Game.ThisPlayer.Position, new Vector3(-60, 10, -30));
        }
        // Update our things at each ticks
        public override void Update()
        {
            //SceneManager.Game.DebugScreen.Debug("debugconsole");
        }
    }
}
