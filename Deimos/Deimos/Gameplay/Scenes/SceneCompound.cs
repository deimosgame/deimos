using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Deimos
{
    class SceneCompound : SceneTemplate
    {
        SceneManager SceneManager;
        ModelManager ModelManager;
        LightManager LightManager;
        SoundManager SoundManager;
        ModelAnimationManager ModelAnimationManager;

        public SceneCompound(SceneManager sceneManager)
        {
            PlayerSize = new Vector3(30, 1, 1);

            SpawnLocations = new SpawnLocation[] {
                new SpawnLocation(new Vector3(18, 10, 90), Vector3.Zero)
            };

            SceneManager = sceneManager;
            ModelManager = SceneManager.ModelManager;
            LightManager = SceneManager.LightManager;
            SoundManager = SceneManager.SoundManager;
            ModelAnimationManager = DisplayFacade.ModelAnimationManager;

            Objects = new ObjectManager();
        }

        // Destructor
        ~SceneCompound()
        {
            //
        }

        public override void Load()
        {

                // Loading the map
            ModelManager.LoadModel(
                 "ourMap",
                 "Models/Map/Compound/DeimosCompound", // Model
                 new Vector3(0, 0, 0), // Location
                 Vector3.Zero,
                 0.2f
            );

            //ModelManager.LoadModel("follow", "Models/Characters/Vanquish/vanquish", Vector3.Zero, Vector3.Zero, 5, LevelModel.CollisionType.None);

            if (NetworkFacade.IsMultiplayer)
            {
                // Loading player models

                foreach (KeyValuePair<byte, Player> p in NetworkFacade.Players)
                {
                    ModelManager.LoadModel(
                        p.Value.Name,
                        p.Value.GetModelName(),
                        p.Value.Position,
                        p.Value.Rotation,
                        5,
                        LevelModel.CollisionType.None
                        );

                    ModelManager.GetLevelModel(p.Value.Name).show = false;
                }

            }

            SoundManager.AddSoundEffect("scary", "Sounds/ScaryMusic");
        }

        // Initialize our lights and such
        public override void Initialize()
        {

            LightManager.AddPointLight(
               "Main",
               new Vector3(18, 10, 90), // Location
               30, // Radius
               2, // Intensity
               Color.Red
            );
            LightManager.AddPointLight(
                "blue1",
                new Vector3(-40, 7, 26),
                25,
                2,
                Color.DarkBlue
                );
            LightManager.AddPointLight(
                "blue2",
                new Vector3(-40, 3, 71),
                25,
                2,
                Color.DarkBlue
                );
            LightManager.AddPointLight(
                "white1",
                new Vector3(-40, 22, 71),
                20,
                1,
                Color.White
                );
            LightManager.AddPointLight(
                "white2",
                new Vector3(-1, 5, 46),
                20,
                1,
                Color.White
                );
            LightManager.AddPointLight(
                "white3",
                new Vector3(20, 0, 17),
                20,
                2,
                Color.White
                );
            LightManager.AddPointLight(
                "white4",
                new Vector3(15, 0, -30),
                20,
                2,
                Color.White
                );
            LightManager.AddPointLight(
                "white5",
                new Vector3(35, 20, 50),
                20,
                1,
                Color.White
                );
            LightManager.AddPointLight(
                "mainpillar",
                new Vector3(-10, 15, 175),
                30,
                2,
                Color.White
                );

            SoundManager.Play3D("scary", DisplayFacade.Camera.Position, 
                new Vector3(0, 0, -0));
        }

        // Update our things at each ticks
        public override void Update(float dt)
        {
            SoundManager.SetListener("scary", GameplayFacade.ThisPlayer.Position);

            Objects.Update(GameplayFacade.ThisPlayer.dt);

            //ModelManager.GetLevelModel("follow").Position = GameplayFacade.ThisPlayer.Position + new Vector3(3, 1, 3);
            //ModelManager.GetLevelModel("follow").Rotation += new Vector3(0,GameplayFacade.ThisPlayer.Rotation.X, 0);
        }
    }
}
