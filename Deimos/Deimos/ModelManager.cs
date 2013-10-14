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

		public void LoadModel(Model model, Texture2D texture, Vector3 position)
		{
			LoadedModels.Add(model);
			LoadedModelsTexture.Add(texture);
			LoadedModelsLocation.Add(position);

			foreach (ModelMesh mesh in model.Meshes)
			{
				DebugScreen.Log("Mesh!");
				Collision.AddCollisionBoxDirectly(BuildBoundingBox(mesh, Matrix.Identity));
			}
		}

		public void DoneAddingModels()
		{
			LoadedModelsArray = LoadedModels.ToArray();
			LoadedModelsTextureArray = LoadedModelsTexture.ToArray();
			LoadedModelsLocationArray = LoadedModelsLocation.ToArray();
		}

		private BoundingBox BuildBoundingBox(ModelMesh mesh, Matrix meshTransform)
		{
			// Create initial variables to hold min and max xyz values for the mesh
			Vector3 meshMax = new Vector3(float.MinValue);
			Vector3 meshMin = new Vector3(float.MaxValue);

			foreach (ModelMeshPart part in mesh.MeshParts)
			{
				// The stride is how big, in bytes, one vertex is in the vertex buffer
				// We have to use this as we do not know the make up of the vertex
				int stride = part.VertexBuffer.VertexDeclaration.VertexStride;

				VertexPositionNormalTexture[] vertexData = new VertexPositionNormalTexture[part.NumVertices];
				part.VertexBuffer.GetData(part.VertexOffset * stride, vertexData, 0, part.NumVertices, stride);

				// Find minimum and maximum xyz values for this mesh part
				Vector3 vertPosition = new Vector3();

				for (int i = 0; i < vertexData.Length; i++)
				{
					vertPosition = vertexData[i].Position;

					// update our values from this vertex
					meshMin = Vector3.Min(meshMin, vertPosition);
					meshMax = Vector3.Max(meshMax, vertPosition);
				}
			}

			// transform by mesh bone matrix
			meshMin = Vector3.Transform(meshMin, meshTransform);
			meshMax = Vector3.Transform(meshMax, meshTransform);

			// Create the bounding box
			BoundingBox box = new BoundingBox(meshMin, meshMax);
			return box;
		}

		public void DrawModels(Camera camera, Effect effect)
		{
			if (LoadedModelsArray != null)
			{
				for (int i = 0; i < LoadedModelsArray.Length; i++)
				{
					Model model = LoadedModelsArray[i];
					Vector3 modelPosition = LoadedModelsLocationArray[i];
					Texture2D modelTexture = LoadedModelsTextureArray[i];
					Matrix world = Matrix.CreateTranslation(modelPosition);

					Matrix[] transforms = new Matrix[model.Bones.Count];
					model.CopyAbsoluteBoneTransformsTo(transforms);
					foreach (ModelMesh mesh in model.Meshes)
					{
							//effect.EnableDefaultLighting();
							//effect.View = camera.View;
							//effect.Projection = camera.Projection;
							//effect.World = Matrix.CreateTranslation(modelPosition);
							//effect.TextureEnabled = true;
							//effect.Texture = modelTexture;

							//effect.LightingEnabled = true; // Turn on the lighting subsystem.
							//effect.AmbientLightColor = new Vector3(0.2f, 0.2f, 0.2f);
							//effect.EmissiveColor = new Vector3(1, 0, 0);
							//effect.DirectionalLight0.DiffuseColor = new Vector3(1f, 0.2f, 0.2f); // a reddish light
							//effect.DirectionalLight0.Direction = new Vector3(1, 0, 0);  // coming along the x-axis
							//effect.DirectionalLight0.SpecularColor = new Vector3(0, 1, 0); // with green highlights


						foreach (ModelMeshPart part in mesh.MeshParts)
						{
							part.Effect = effect;
							effect.Parameters["World"].SetValue(world);
							effect.Parameters["View"].SetValue(camera.View);
							effect.Parameters["Projection"].SetValue(camera.Projection);

							// Ambient light
							effect.Parameters["AmbientColor"].SetValue(Color.Beige.ToVector4());
							effect.Parameters["AmbientIntensity"].SetValue(0.01f);
							effect.Parameters["DiffuseLightDirection"].SetValue(camera.ViewVector); // Make the light follow the camera

							// Difuse light
							Matrix worldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(mesh.ParentBone.Transform * world));
							effect.Parameters["WorldInverseTranspose"].SetValue(worldInverseTransposeMatrix);

							// Specular light
							effect.Parameters["ViewVector"].SetValue(camera.ViewVector);
							//DebugScreen.Log(camera.ViewVector.ToString());

							// Texturing
							effect.Parameters["ModelTexture"].SetValue(modelTexture);
						}
						mesh.Draw();
					}
				}
			}
		}
	}
}
