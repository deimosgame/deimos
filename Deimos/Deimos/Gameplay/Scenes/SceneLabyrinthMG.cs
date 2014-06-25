using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tranquillity;

namespace Deimos
{
    class SceneLabyrinthMG : SceneTemplate
    {
        SceneManager SceneManager;
        ModelManager ModelManager;
        LightManager LightManager;
        SoundManager SoundManager;
        ModelAnimationManager ModelAnimationManager;

        DynamicParticleSystem ParticleSystem;
        SmokeParticleEmitter ParticleEmitter;

        string token_knife;

        // Constructor
        public SceneLabyrinthMG(SceneManager sceneManager)
        {
            PlayerSize = new Vector2(1, 7);

            SceneManager = sceneManager;
            ModelManager = SceneManager.ModelManager;
            LightManager = SceneManager.LightManager;
            SoundManager = SceneManager.SoundManager;
            ModelAnimationManager = DisplayFacade.ModelAnimationManager;

            Objects = new ObjectManager();
            Secrets = new SecretsManager();

            TimeLimit = GameplayFacade.Minigames.LabyrinthMG.TimeLimit;
        }

        // Destructor
        ~SceneLabyrinthMG()
        {
            //
        }

        // Load our models and such
        public override void Load()
        {
            ModelManager.LoadModel(
                 GameplayFacade.Minigames.LabyrinthMG.Name,
                 GameplayFacade.Minigames.LabyrinthMG.Map, // Model
                 Vector3.Zero, // Location
                 Vector3.Zero,
                 2
            );
        }

        // Initialize our lights and such
        public override void Initialize()
        {
            LightManager.AddPointLight(
               "Main",
               new Vector3(30, 13, -16), // Location
               30, // Radius
               3, // Intensity
               Color.White
            );

            token_knife = Objects.AddWeapon(
                "CarverPickup",
                0, new Vector3(30, 10, -16),
                PickupObject.State.Active,
                2000);

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
                //    GameplayFacade.Minigames.LabyrinthMG.TimeLimit -
                //    Elapsed;
            }
            else
            {
                GameplayFacade.Minigames.LabyrinthMG.Terminate();
            }
        }
    }
}
