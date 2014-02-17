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
            PlayerSize = new Vector3(33f, 1f, 1f);

            SceneManager = sceneManager;
            ModelManager = SceneManager.ModelManager;
            LightManager = SceneManager.LightManager;
        }

        // Destructor
        ~SceneSkatePark()
        {
            ModelManager.UnloadModels();
        }

        // Load our models and such
        public override void Load()
        {
            ModelManager.LoadModel(
                 "skatepark",
                 "Models/Map/arena", // Model
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
            SceneManager.Collision.AddCollisionBox(
                new Vector3(-1000, -80, -1000),
                new Vector3(1000, -81, 1000),
                delegate(CollisionElement element, DeimosGame game)
                {
                    game.ThisPlayer.Position = new Vector3(0, 80, 20);
                }
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
               Color.CadetBlue
            );
        }

        public float x = -1f;
        Random rnd = new Random();
        public int i = 0;
        public int r = 0;

        // Update our things at each ticks
        public override void Update()
        {
            //PointLight light = LightManager.GetPointLight("player");
            //r = rnd.Next(0, 10);
            //if (i == r)
            //{
            //    i = 0;
                
            //    light.Intensity += x;
            //    x = -x;
            //}

            //if (i > 9)
            //{
            //    i = 0;
            //}

            //i++;
            LevelModel weapon = ModelManager.GetLevelModel("PP19");
            weapon.Rotation = SceneManager.Game.ThisPlayer.Rotation;

            weapon.WorldMatrix = WeaponWorldMatrix(SceneManager.Game.ThisPlayer.Position, SceneManager.Game.ThisPlayer.Rotation.Y, SceneManager.Game.ThisPlayer.Rotation.X);
            
        }

        private Matrix WeaponWorldMatrix(Vector3 Position, float updown, float leftright)
        {
            Vector3 xAxis;
            Vector3 yAxis;

            xAxis.X = SceneManager.Game.Camera.View.M11;
            xAxis.Y = SceneManager.Game.Camera.View.M21;
            xAxis.Z = SceneManager.Game.Camera.View.M31;

            yAxis.X = SceneManager.Game.Camera.View.M12;
            yAxis.Y = SceneManager.Game.Camera.View.M22;
            yAxis.Z = SceneManager.Game.Camera.View.M32;

            Position += new Vector3(1, 0, 0) / 5;  //How far infront of the camera The gun will be
            Position += xAxis * 1f;      //X axis offset
            Position += -yAxis * 0.5f;     //Y axis offset
            SceneManager.Game.DebugScreen.Debug(Position.ToString());
            return Matrix.CreateScale(0.1f)                        //Size of the Gun
                * Matrix.CreateFromYawPitchRoll(MathHelper.ToRadians(5), 0, 0)      //Rotation offset
                * Matrix.CreateRotationX(updown)
                * Matrix.CreateRotationY(leftright)
                * Matrix.CreateTranslation(Position);
        }
    }
}
