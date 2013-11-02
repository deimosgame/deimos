using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Deimos
{
	static class SceneManager
	{
		// Attributes
		static Dictionary<string, ModelManager> ModelManagerList =
			new Dictionary<string, ModelManager>();
		static Dictionary<string, LightManager> LightManagerList =
			new Dictionary<string, LightManager>();
		static string CurrentScene;


		// Methods
		public static void AddScene(string name)
		{
			ModelManager thisModelManager = new ModelManager();
			ModelManagerList.Add(name, thisModelManager);
			LightManager thisLightManager = new LightManager();
			LightManagerList.Add(name, thisLightManager);

			CurrentScene = name;
		}

		public static void SetCurrentScene(string name)
		{
			CurrentScene = name;
		}

		public static ModelManager GetModelManager(string name = null)
		{
			if (name == null)
			{
				name = CurrentScene;
			}
			return ModelManagerList[name];
		}
		public static LightManager GetLightManager(string name = null)
		{
			if (name == null)
			{
				name = CurrentScene;
			}
			return LightManagerList[name];
		}

		public static void DrawCurrentScene(Camera camera, GraphicsDevice graphicsDevice, Effect effect)
		{
			
		}
	}
}
