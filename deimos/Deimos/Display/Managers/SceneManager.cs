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
        Dictionary<string, ModelManager> ModelManagerList =
            new Dictionary<string, ModelManager>();
        Dictionary<string, LightManager> LightManagerList =
            new Dictionary<string, LightManager>();
        ModelManager modelManager;
        internal ModelManager ModelManager
        {
            get { return modelManager; }
            private set { modelManager = value; }
        }

        LightManager lightManager;
        internal LightManager LightManager
        {
            get { return lightManager; }
            private set { lightManager = value; }
        }
        SceneTemplate CurrentScene;

        DeimosGame game;
        public DeimosGame Game
        {
            get { return game; }
            private set { game = value; }
        }

        ContentManager Content;


        // Constructor
        public SceneManager(DeimosGame game, ContentManager content)
        {
            Game = game;
            Content = content;
            ModelManager = new ModelManager(Content);
            LightManager = new LightManager();
        }


        // Methods
        public void SetScene<T>()
        {
            // Setting it to null makes the GarbageCollector call the destructor
            // of the Scene
            CurrentScene = null;

            ModelManager = null;
            LightManager = null;
            ModelManager = new ModelManager(Content);
            LightManager = new LightManager();

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
