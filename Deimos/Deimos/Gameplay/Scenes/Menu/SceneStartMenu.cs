using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Deimos
{
    class SceneStartMenu : SceneTemplate
    {
        SceneManager SceneManager;
        ModelManager ModelManager;
        LightManager LightManager;
        SoundManager SoundManager;

        // Constructor
        public SceneStartMenu(SceneManager sceneManager)
        {
            PlayerSize = new Vector3(20, 1, 1);
            SceneManager = sceneManager;
            ModelManager = SceneManager.ModelManager;
            LightManager = SceneManager.LightManager;
            SoundManager = SceneManager.SoundManager;
        }

        // Destructor
        ~SceneStartMenu()
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
                 Vector3.Zero,
                 1.5f
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
            SoundManager.Play3D("scary", DisplayFacade.Camera.Position, new Vector3(-127, 6, -64));
            //SoundManager.Play("scary");
        }

        // Update our things at each ticks
        public override void Update()
        {
            SoundManager.SetListener("scary", GameplayFacade.ThisPlayer.Position);
        }
    }
}
