using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tranquillity;

namespace Deimos
{
    class SceneKnifeMG : SceneTemplate
    {
        SceneManager SceneManager;
        ModelManager ModelManager;
        LightManager LightManager;
        SoundManager SoundManager;
        ModelAnimationManager ModelAnimationManager;

        DynamicParticleSystem ParticleSystem;
        SmokeParticleEmitter ParticleEmitter;

        // Constructor
        public SceneKnifeMG(SceneManager sceneManager)
        {
            PlayerSize = new Vector2(1, 10);

            SceneManager = sceneManager;
            ModelManager = SceneManager.ModelManager;
            LightManager = SceneManager.LightManager;
            SoundManager = SceneManager.SoundManager;
            ModelAnimationManager = DisplayFacade.ModelAnimationManager;

            Objects = new ObjectManager();
            Secrets = new SecretsManager();

            TimeLimit = GameplayFacade.Minigames.KnifeMG.TimeLimit;
        }

        // Destructor
        ~SceneKnifeMG()
        {
            //
        }

        // Load our models and such
        public override void Load()
        {
            ModelManager.LoadModel(
                 GameplayFacade.Minigames.KnifeMG.Name,
                 GameplayFacade.Minigames.KnifeMG.Map, // Model
                 Vector3.Zero, // Location
                 Vector3.Zero
            );
        }

        // Initialize our lights and such
        public override void Initialize()
        {
            LightManager.AddPointLight(
               "Main",
               new Vector3(18, 49, 1), // Location
               100, // Radius
               4, // Intensity
               Color.White
            );

            LightManager.AddPointLight(
                "Team1",
                new Vector3(21,8,-62),
                30,
                2,
                Color.White
                );

            LightManager.AddPointLight(
                "Team2",
                new Vector3(21, 8, 67),
                30,
                2,
                Color.White
                );

        }
        // Update our things at each ticks
        public override void Update(float dt)
        {
            Objects.Update(GameplayFacade.ThisPlayer.dt);
            Secrets.Update(GameplayFacade.ThisPlayer.dt);

            if (Elapsed < TimeLimit)
            {
                Elapsed += dt;
                //GameplayFacade.Minigames.Remaining =
                //    GameplayFacade.Minigames.knife.TimeLimit -
                //    Elapsed;
            }
            else
            {
                GameplayFacade.Minigames.KnifeMG.Terminate();
            }
        }
    }
}
