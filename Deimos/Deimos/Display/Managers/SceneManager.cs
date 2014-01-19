using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Deimos
{
	class SceneManager
	{
		// Attributes
		Dictionary<string, ModelManager> ModelManagerList =
			new Dictionary<string, ModelManager>();
		Dictionary<string, LightManager> LightManagerList =
			new Dictionary<string, LightManager>();
		string CurrentScene;
        Game Game;


        // Constructor
        public SceneManager(Game game)
        {
            Game = game;
        }


		// Methods
		public void AddScene(string name)
		{
			ModelManagerList.Add(name, new ModelManager(Game.Content));
			LightManagerList.Add(name, new LightManager());

			CurrentScene = name;
		}

		public void SetCurrentScene(string name)
		{
			CurrentScene = name;
		}

		public ModelManager GetModelManager(string name = null)
		{
			if (name == null)
			{
				name = CurrentScene;
			}
			return ModelManagerList[name];
		}
		public LightManager GetLightManager(string name = null)
		{
			if (name == null)
			{
				name = CurrentScene;
			}
			return LightManagerList[name];
		}
	}
}
