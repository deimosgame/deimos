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
                new Vector3(-90f, 1f, -49f),
                PickupObject.State.Active,
                50,
                5
            );

            token_speed_boost = Objects.AddEffect("Speed Boost",
                new Vector3(-70, 3, -49f),
                PickupObject.State.Active,
                20,
                10,
                5
            );

            token_gravity_boost = Objects.AddEffect("Gravity Boost",
                new Vector3(-110, 2, -49),
                PickupObject.State.Active,
                3,
                10,
                5
            );

            token_arbiter = Objects.AddWeapon("ArbiterPickup",
                0,
                new Vector3(-100, 2, -55),
                PickupObject.State.Active,
                10
            );

            token_pistol = Objects.AddWeapon("PistolPickup",
                20,
                new Vector3(-70, 2, -55),
                PickupObject.State.Active,
                10
            );

            List<Vector3> mysterspawns = new List<Vector3>();
            mysterspawns.Add(new Vector3(-110, 5, -40));
            mysterspawns.Add(new Vector3(-70, 8, -40));
            Objects.CreateMystery(mysterspawns, 10, 20, 1, default(Vector3));
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


            if (GameplayFacade.ThisPlayer.ks.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.G)
                && (Objects.GetEffect(token_gravity_boost).Status == PickupObject.State.Active))
            {
                Objects.TreatEffect(Objects.GetEffect(token_gravity_boost), token_gravity_boost);
            }
            if (GameplayFacade.ThisPlayer.ks.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.F)
                && (Objects.GetEffect(token_speed_boost).Status == PickupObject.State.Active))
            {
                Objects.TreatEffect(Objects.GetEffect(token_speed_boost), token_speed_boost);
            }
            if (GameplayFacade.ThisPlayer.ks.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.H)
                && (Objects.GetEffect(token_health_pack).Status == PickupObject.State.Active))
            {
                Objects.TreatEffect(Objects.GetEffect(token_health_pack), token_health_pack);
            }
            if (GameplayFacade.ThisPlayer.ks.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.C)
                && (Objects.GetWeapon(token_arbiter).Status == PickupObject.State.Active))
            {
                Objects.TreatWeapon(Objects.GetWeapon(token_arbiter), token_arbiter);
            }
            if (GameplayFacade.ThisPlayer.ks.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.V)
                && (Objects.GetWeapon(token_pistol).Status == PickupObject.State.Active))
            {
                Objects.TreatWeapon(Objects.GetWeapon(token_pistol), token_pistol);
            }
            if (GameplayFacade.ThisPlayer.ks.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.U)
                && (Objects.GetMysteryPickup().Status == PickupObject.State.Active))
            {
                Objects.TreatWeapon(Objects.GetMysteryPickup(), Objects.GetMysteryPickup().Name);
                Objects.SetMysteryRespawn();
            }


            Objects.Update(GameplayFacade.ThisPlayer.dt);
        }
    }
}
