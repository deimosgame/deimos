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
               new Vector3(-85, 6, -45), // Location
               50, // Radius
               2, // Intensity
               Color.White
            );
            LightManager.AddPointLight(
               "Corridor1",
               new Vector3(-21, 8, 110), // Location
               18, // Radius
               1, // Intensity
               Color.Red
            );
            LightManager.AddPointLight(
               "Corridor2",
               new Vector3(88, 6, 110), // Location
               18, // Radius
               1, // Intensity
               Color.Red
            );
            LightManager.AddPointLight(
               "Scary",
               new Vector3(37, -6, 200), // Location
               50, // Radius
               2, // Intensity
               Color.LightBlue
            );
            SoundManager.Play3D("scary", SceneManager.Game.ThisPlayer.Position, new Vector3(-127, 6, -64));
            //SoundManager.Play("scary");
        }

        public float x = -1f;
        Random rnd = new Random();
        public int i = 0;
        public int r = 0;

        // Update our things at each ticks
        public override void Update()
        {
            SoundManager.SetListener("scary", SceneManager.Game.ThisPlayer.Position);
            //SceneManager.Game.DebugScreen.Debug("debugconsole");

            PointLight light1 = LightManager.GetPointLight("Corridor1");
            PointLight light2 = LightManager.GetPointLight("Corridor2");
            PointLight light3 = LightManager.GetPointLight("Scary");
            r = rnd.Next(0, 10);
            if (i == r)
            {
                i = 0;

                light1.Intensity += x;
                light2.Intensity += x;
                light3.Intensity += x;
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
