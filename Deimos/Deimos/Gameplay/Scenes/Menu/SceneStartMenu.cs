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

        float CurrentAngle = 0;

        public override float AmbiantLight
        {
            get { return 0.05f; }
        }

        // Constructor
        public SceneStartMenu(SceneManager sceneManager)
        {
            PlayerSize = new Vector3(20, 1, 1);
            SpawnLocations = new SpawnLocation[] {
                new SpawnLocation(new Vector3(0, -40f, 340f), new Vector3(0, (float)Math.PI, 0))
            };

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
                 "DeimosMain",
                 "Models/Map/Main/DeimosMain", // Model
                 new Vector3(10, 0, 0), // Location
                 Vector3.Zero,
                 0.25f
            );
            SoundManager.AddSoundEffect("scary", "Sounds/ScaryMusic");
        }

        // Initialize our lights and such
        public override void Initialize()
        {
            LightManager.AddPointLight("mainLight", new Vector3(20, -30, 100), 380, 1, Color.White);

            SoundManager.Play("scary");
        }

        // Update our things at each ticks
        public override void Update(float dt)
        {
            CurrentAngle += dt;
            Vector2 temp = Rotate(CurrentAngle, 300, Vector2.Zero);
            DisplayFacade.Camera.Position = new Vector3(temp.X, DisplayFacade.Camera.Position.Y, temp.Y);
        }

        private Vector2 Rotate(float angle, float distance, Vector2 centre)
        {
            return new Vector2((float)(distance * Math.Cos(angle)), (float)(distance * Math.Sin(angle))) + centre;
        }
    }
}
