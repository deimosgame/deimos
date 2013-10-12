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


		public ModelManager()
		{
			//
		}

		public void LoadModel(Model model)
		{
			LoadedModels.Add(model);
		}

		public void DoneAddingModels()
		{
			LoadedModelsArray = LoadedModels.ToArray();
		}

		public void DrawModels(Camera camera)
		{
			foreach (Model model in LoadedModelsArray)
			{
				Matrix[] transforms = new Matrix[model.Bones.Count];
				model.CopyAbsoluteBoneTransformsTo(transforms);
				foreach (ModelMesh mesh in model.Meshes)
				{
					foreach (BasicEffect effect in mesh.Effects)
					{
						effect.EnableDefaultLighting();
						effect.View = camera.View;
						effect.Projection = camera.Projection;
						effect.World = Matrix.Identity;
					}
					mesh.Draw();
				}
			}
		}
	}
}
