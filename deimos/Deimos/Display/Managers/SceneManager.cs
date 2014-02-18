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

        internal LocalPlayerCollision Collision
        {
            get;
            private set;
        }
        SceneTemplate CurrentScene;

        public DeimosGame Game
        {
            get;
            private set;
        }

        ContentManager Content;


        // Constructor
        public SceneManager(DeimosGame game, ContentManager content)
        {
            Game = game;
            Content = content;
        }


        // Methods
        public void SetScene<T>()
        {
            // Setting it to null makes the GarbageCollector call the destructor
            // of the Scene
            CurrentScene = null;

            ResourceManager = null;
            ModelManager = null;
            LightManager = null;
            Collision    = null;

            ResourceManager = new ResourceManager(Content);
            ModelManager = new ModelManager(Game);
            LightManager = new LightManager();

            CurrentScene = (SceneTemplate)Activator.CreateInstance(typeof(T), new object[] { this });

            Collision = new LocalPlayerCollision(
                CurrentScene.PlayerSize.X,
                CurrentScene.PlayerSize.Y,
                CurrentScene.PlayerSize.Z, 
                Game
            );

            // Constructor is automatically called (of the Scene)

            // We need to call the load method to load our models
            CurrentScene.Load();

            // Let's init the scene (useful for our lights for example)
            CurrentScene.Initialize();
        }

        public void Update()
        {
            CurrentScene.Update();
        }
    }
}
