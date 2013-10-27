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
		static Dictionary<string, ModelManager> ModelsList = 
			new Dictionary<string, ModelManager>();
		static Dictionary<string, LightManager> LightsList =
			new Dictionary<string, LightManager>();

		static string CurrentScene;


		// Methods
		static public void AddScene(string sceneName, Effect effect, 
			Color ambientColor, float specularPower)
		{
			LightManager thisLightManager = new LightManager(ambientColor, specularPower);
			ModelsList.Add(sceneName, new ModelManager(thisLightManager.SetEffect(effect)));
			LightsList.Add(sceneName, thisLightManager);

			CurrentScene = sceneName;
		}

		static public ModelManager GetModelManager(string sceneName = null)
		{
			if (sceneName == null)
			{
				sceneName = CurrentScene;
			}

			return ModelsList[sceneName];
		}

		static public LightManager GetLightManager(string sceneName = null)
		{
			if (sceneName == null)
			{
				sceneName = CurrentScene;
			}

			return LightsList[sceneName];
		}


		static public void SetCurrentScene(string currentScene)
		{
			CurrentScene = currentScene;
		}


		static public void DrawScene(Camera camera, 
			GraphicsDevice graphicsDevice, string sceneName = null)
		{
			GetModelManager(sceneName).DrawModels(
				camera, 
				graphicsDevice,
				GetLightManager(sceneName)
			);
		}
	}
}
