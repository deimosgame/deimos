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
            ModelManager = new ModelManager(Content, game);
            LightManager = new LightManager();
            Collision    = new LocalPlayerCollision(33f, 1f, 1f, game);
        }


        // Methods
        public void SetScene<T>()
        {
            // Setting it to null makes the GarbageCollector call the destructor
            // of the Scene
            CurrentScene = null;

            ModelManager = null;
            LightManager = null;
            Collision    = null;
           
            ModelManager = new ModelManager(Content, Game);
            LightManager = new LightManager();
            Collision    = new LocalPlayerCollision(33f, 1f, 1f, Game);

            CurrentScene = (SceneTemplate)Activator.CreateInstance(typeof(T), new object[] { this });

            // Constructor is automatically called (of the Scene), which is 
            // supposed to load models and so on.

            // Let's init the scene (useful for our lights for example)
            CurrentScene.Initialize();
        }

        public void Update()
        {
            CurrentScene.Update();
        }
    }
}
