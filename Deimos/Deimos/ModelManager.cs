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
		private Collision Collision;

		private List<Model> LoadedModels = new List<Model>();
		private Model[] LoadedModelsArray;
		private List<Texture2D> LoadedModelsTexture = new List<Texture2D>();
		private Texture2D[] LoadedModelsTextureArray;
		private List<Vector3> LoadedModelsLocation = new List<Vector3>();
		private Vector3[] LoadedModelsLocationArray;
 
		public ModelManager(Collision collision)
		{
			Collision = collision;
		}

		public void LoadModel(Model model, Texture2D texture, Vector3 location)
		{
			LoadedModels.Add(model);
			LoadedModelsTexture.Add(texture);
			LoadedModelsLocation.Add(location);
		}

		public void DoneAddingModels()
		{
			LoadedModelsArray = LoadedModels.ToArray();
			LoadedModelsTextureArray = LoadedModelsTexture.ToArray();
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
					Texture2D modelTexture = LoadedModelsTextureArray[i];

					Matrix[] transforms = new Matrix[model.Bones.Count];
					model.CopyAbsoluteBoneTransformsTo(transforms);
					foreach (ModelMesh mesh in model.Meshes)
					{
						Collision.AddCollisionSphereDirectly(mesh.BoundingSphere);

						foreach (BasicEffect effect in mesh.Effects)
						{
							effect.EnableDefaultLighting();
							effect.View = camera.View;
							effect.Projection = camera.Projection;
							effect.World = Matrix.CreateTranslation(modelPosition);
							effect.TextureEnabled = true;
							effect.Texture = modelTexture;
						}
						mesh.Draw();
					}
				}
			}
		}
	}
}
