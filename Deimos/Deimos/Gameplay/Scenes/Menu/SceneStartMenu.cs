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
            CurrentAngle += dt * 0.05f;
            if (DisplayFacade.Camera.Position.Z < 50)
            {
                CurrentAngle += dt;
            }
            else
            {
                CurrentAngle += dt * 0.1f;
            }
            DisplayFacade.Camera.Position = ReCreateViewMatrix(CurrentAngle, DisplayFacade.Camera.Position);
            DisplayFacade.Camera.CameraLookAt = new Vector3(0, -40, 0);
        }

        private Vector3 ReCreateViewMatrix(float angle, Vector3 position)
        {
            //Calculate the relative position of the camera
            position = Vector3.Transform(Vector3.Backward, Matrix.CreateFromYawPitchRoll(angle, 0, 0));
            //Convert the relative position to the absolute position
            position *= 300;
            position += new Vector3(0, -40, 0);

            //Calculate a new viewmatrix
            //viewMatrix = Matrix.CreateLookAt(position, lookAt, Vector3.Up);

            return position;
        }
    }
}
