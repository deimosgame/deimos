using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Deimos
{
    class SceneManager
    {
        // Attributes
        internal ResourceManager ResourceManager
        {
            get;
            private set;
        }

        internal ModelManager ModelManager
        {
            get;
            private set;
        }

        internal LightManager LightManager
        {
            get;
            private set;
        }

        internal SoundManager SoundManager
        {
            get;
            private set;
        }

        internal CollisionManager CollisionManager
        {
            get;
            private set;
        }

        internal PlayerCollision PlayerCollision
        {
            get;
            private set;
        }

        SceneTemplate CurrentScene;

        ContentManager Content;

        float CurrentAmbiantLight = 0.1f;


        // Constructor
        public SceneManager(ContentManager content)
        {
            Content = content;
        }


        // Methods
        public void SetScene<T>()
        {
            if(!(CurrentScene is T))
            {
                SetSceneForce<T>();
            }
        }

        public void SetSceneForce<T>()
        {
            // Setting it to null makes the GarbageCollector call the destructor
            // of the Scene
            CurrentScene = null;

            if (ResourceManager != null)
            {
                ResourceManager.Unload();
            }

            ResourceManager = null;
            ModelManager = null;
            LightManager = null;
            SoundManager = null;
            CollisionManager = null;
            PlayerCollision = null;

            ResourceManager = new ResourceManager(Content);
            ModelManager = new ModelManager();
            LightManager = new LightManager();
            SoundManager = new SoundManager(Content);
            CollisionManager = new CollisionManager();

            CurrentScene = (SceneTemplate)Activator.CreateInstance(typeof(T), new object[] { this });

            CurrentAmbiantLight = CurrentScene.AmbiantLight;

            PlayerCollision = new PlayerCollision(
                CurrentScene.PlayerSize.X,
                CurrentScene.PlayerSize.Y,
                CurrentScene.PlayerSize.Z
            );

            PlayerCollision.ElementType = CollisionElement.CollisionType.Box;

            // Adding the player in the collision elements
            CollisionManager.AddElementDirectly(PlayerCollision);



            DisplayFacade.Camera.Position = CurrentScene.SpawnLocations[0].Location;
            DisplayFacade.Camera.Rotation = CurrentScene.SpawnLocations[0].Rotation;

            // Let's load our default files
            LoadDefault();

            // Constructor is automatically called (of the Scene)

            // We need to call the load method to load our models
            CurrentScene.Load();

            // Let's init the scene (useful for our lights for example)
            CurrentScene.Initialize();
        }

        public void LoadDefault()
        {
            // Footstepping
            SoundManager.AddSoundEffect("s1", "Sounds/steps/boot1");
            SoundManager.AddSoundEffect("s2", "Sounds/steps/boot2");
            SoundManager.AddSoundEffect("s3", "Sounds/steps/boot3");
            SoundManager.AddSoundEffect("s4", "Sounds/steps/boot4");
            SoundManager.AddSoundEffect("l1", "Sounds/steps/wood_walk2");
            SoundManager.AddSoundEffect("l2", "Sounds/steps/wood_walk3");

            // Weapons
                // Melee swinging
            SoundManager.AddSoundEffect("w_sw1", "Sounds/weapons/swing1");
            SoundManager.AddSoundEffect("w_sw2", "Sounds/weapons/swing2");
            SoundManager.AddSoundEffect("w_sw3", "Sounds/weapons/swing3");
            SoundManager.AddSoundEffect("w_sw4", "Sounds/weapons/swing4");
                // Firearm firing
            SoundManager.AddSoundEffect("gun", "Sounds/GunFire");
            SoundManager.AddSoundEffect("rifle", "Sounds/weapons/fire_gun");
            SoundManager.AddSoundEffect("rocket", "Sounds/weapons/explode");
                // No ammo
            SoundManager.AddSoundEffect("noammo", "Sounds/weapons/noammo");
                // Weapon switching
            SoundManager.AddSoundEffect("w_c", "Sounds/weapons/change");
            //SoundManager.AddSoundEffect("w_c_m", "Sounds/weapons/select(5)");
            SoundManager.AddSoundEffect("w_sel1", "Sounds/weapons/select(1)");
            SoundManager.AddSoundEffect("w_sel2", "Sounds/weapons/select(2)");
            SoundManager.AddSoundEffect("w_sel3", "Sounds/weapons/select(3)");
            SoundManager.AddSoundEffect("w_sel4", "Sounds/weapons/select(4)");
            SoundManager.AddSoundEffect("w_sel5", "Sounds/weapons/select(6)");
            SoundManager.AddSoundEffect("w_sel6", "Sounds/weapons/select");
                // Picking up a weapon
            SoundManager.AddSoundEffect("w_pu", "Sounds/weapons/w_pkup");

            // Sound Effects
                // Speed boost pickup
            SoundManager.AddSoundEffect("speed", "Sounds/fx/speed");
                // Gravity boost pickup
            SoundManager.AddSoundEffect("gravity", "Sounds/fx/jumpbuild");
                // Health pack pickup
            SoundManager.AddSoundEffect("heal", "Sounds/fx/heal");

                // Effect fade
            SoundManager.AddSoundEffect("effectoff", "Sounds/fx/eoff");

                // Rocket explosion
            SoundManager.AddSoundEffect("explosion", "Sounds/fx/hit_wall");
        }

        public void Update(float dt)
        {
            CurrentScene.Update(dt);
            SoundManager.Update();
        }

        public float GetCurrentAmbiantLight()
        {
            return CurrentAmbiantLight;
        }
    }
}
