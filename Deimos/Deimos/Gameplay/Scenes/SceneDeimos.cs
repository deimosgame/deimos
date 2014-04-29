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

        // tokens
        string token_health_pack;
        string token_speed_boost;
        string token_gravity_boost;

        string token_arbiter;
        string token_pistol;

        // Constructor
        public SceneDeimos(SceneManager sceneManager)
        {
            PlayerSize = new Vector3(20,5, 5);
            SpawnLocations = new SpawnLocation[] {
                new SpawnLocation(new Vector3(-60f, 20f, -8f), Vector3.Zero)
            };

            SceneManager = sceneManager;
            ModelManager = SceneManager.ModelManager;
            LightManager = SceneManager.LightManager;
            SoundManager = SceneManager.SoundManager;

            Objects = new ObjectManager();
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
                 Vector3.Zero,
                 1.5f
            );
            SoundManager.AddSoundEffect("scary", "Sounds/ScaryMusic");
            SceneManager.CollisionManager.AddCollisionBox(
                new Vector3(-1000, -80, -1000),
                new Vector3(1000, -81, 1000),
                delegate(CollisionElement element, DeimosGame game)
                {
                    GameplayFacade.ThisPlayer.Position = new Vector3(0, 80, 20);
                }
            );
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

            token_health_pack = Objects.AddEffect("Health Pack",
                new Vector3(-90, 6, -50),
                PickupObject.State.Active,
                10,
                3
            );

            token_arbiter = Objects.AddWeapon("ArbiterPickup",
                0,
                new Vector3(-60, 6, -50),
                PickupObject.State.Active,
                3
            );

            //List<Vector3> mysterspawns = new List<Vector3>();
            //mysterspawns.Add(new Vector3(-110, 5, -40));
            //mysterspawns.Add(new Vector3(-70, 8, -40));
            //Objects.CreateMystery(mysterspawns, 10, 20, 1, default(Vector3));
        }

        public float x = -1f;
        Random rnd = new Random();
        public int i = 0;
        public int r = 0;

        // Update our things at each ticks
        public override void Update()
        {
            SoundManager.SetListener("scary", GameplayFacade.ThisPlayer.Position);

            //SceneManager.Game.DebugScreen.Debug("debugconsole");

            //PointLight light1 = LightManager.GetPointLight("Corridor1");
            //PointLight light2 = LightManager.GetPointLight("Corridor2");
            //PointLight light3 = LightManager.GetPointLight("Scary");
            //r = rnd.Next(0, 10);
            //if (i == r)
            //{
            //    i = 0;

            //    light1.Intensity += x;
            //    light2.Intensity += x;
            //    light3.Intensity += x;
            //    x = -x;
            //}

            //if (i > 9)
            //{
            //    i = 0;
            //}

            //i++;

            Objects.Update(GameplayFacade.ThisPlayer.dt);
        }
    }
}
