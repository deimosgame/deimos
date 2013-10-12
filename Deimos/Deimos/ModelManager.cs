using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Deimos
{
	class ModelManager
	{
		private List<Model> LoadedModels = new List<Model>();
		private Model[] LoadedModelsArray;
		private List<Vector3> LoadedModelsLocation = new List<Vector3>();
		private Vector3[] LoadedModelsLocationArray;

		public ModelManager()
		{
			//
		}

		public void LoadModel(Model model, Vector3 location)
		{
			LoadedModels.Add(model);
			LoadedModelsLocation.Add(location);
		}

		public void DoneAddingModels()
		{
			LoadedModelsArray = LoadedModels.ToArray();
			LoadedModelsLocationArray = LoadedModelsLocation.ToArray();
		}

		public void DrawModels(Camera camera)
		{
			if (LoadedModelsArray != null)
			{
				for (int i = 0; i < LoadedModelsArray.Length; i++)
				{
					Model model = LoadedModelsArray[i];
					Vector3 modelPosition = LoadedModelsLocationArray[i];

					Matrix[] transforms = new Matrix[model.Bones.Count];
					model.CopyAbsoluteBoneTransformsTo(transforms);
					foreach (ModelMesh mesh in model.Meshes)
					{
						foreach (BasicEffect effect in mesh.Effects)
						{
							effect.EnableDefaultLighting();
							effect.View = camera.View;
							effect.Projection = camera.Projection;
							effect.World = Matrix.CreateWorld(modelPosition, Vector3.Zero, Vector3.Zero);
						}
						mesh.Draw();
					}
				}
			}
		}
	}
}
