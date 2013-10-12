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

		public void DrawModels(Matrix view, Matrix projection)
		{
			foreach (Model model in LoadedModelsArray)
			{
				foreach (ModelMesh mesh in model.Meshes)
				{
					foreach (BasicEffect effect in mesh.Effects)
					{
						effect.View = view;
						effect.World = Matrix.Identity;
						effect.Projection = projection;
						effect.EnableDefaultLighting();
					}
					mesh.Draw();
				}
			}
		}
	}
}
