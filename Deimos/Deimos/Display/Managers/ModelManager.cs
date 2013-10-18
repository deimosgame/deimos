using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;


namespace Deimos
{
	class ModelManager
	{
		// Attributes
		private Collision Collision;

		private List<Model> LoadedModels = new List<Model>();
		private Model[] LoadedModelsArray;
		private List<Texture2D> LoadedModelsTexture = new List<Texture2D>();
		private Texture2D[] LoadedModelsTextureArray;
		private List<Vector3> LoadedModelsLocation = new List<Vector3>();
		private Vector3[] LoadedModelsLocationArray;
 

		// Constructor
		public ModelManager(Collision collision)
		{
			// Storing the collision class for later user
			Collision = collision;
		}


		// Methods
		public void LoadModel(ContentManager content, string model, 
			string texture, Vector3 position)
		{
			// Adding the model to our List/array as well as its location
			// & texture
			Model thisModel = content.Load<Model>(model);
			LoadedModels.Add(thisModel);
			LoadedModelsTexture.Add(content.Load<Texture2D>(texture));
			LoadedModelsLocation.Add(position);

			// Looping through its meshes to calculate its hitboxes and add 
			// them to our collision class
			foreach (ModelMesh mesh in thisModel.Meshes)
			{
				Collision.AddCollisionBoxDirectly(
					BuildBoundingBox(
						mesh, 
						Matrix.Identity
					)
				);
			}
		}

		public void DoneAddingModels()
		{
			// Converting all our lists to arrays
			LoadedModelsArray = LoadedModels.ToArray();
			LoadedModelsTextureArray = LoadedModelsTexture.ToArray();
			LoadedModelsLocationArray = LoadedModelsLocation.ToArray();
		}

		private BoundingBox BuildBoundingBox(ModelMesh mesh, 
			Matrix meshTransform)
		{
			// Create initial variables to hold min and max xyz values 
			// for the mesh
			Vector3 meshMax = new Vector3(float.MinValue);
			Vector3 meshMin = new Vector3(float.MaxValue);

			foreach (ModelMeshPart part in mesh.MeshParts)
			{
				// The stride is how big, in bytes, one vertex is in the 
				// vertex buffer. We have to use this as we do not know the
				// make up of the vertex
				int stride = part.VertexBuffer.VertexDeclaration.VertexStride;

				VertexPositionNormalTexture[] vertexData = 
					new VertexPositionNormalTexture[part.NumVertices];
				part.VertexBuffer.GetData(
					part.VertexOffset * stride, 
					vertexData, 0, 
					part.NumVertices, 
					stride
				);

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
						/* This could be usefull for debugging
						foreach (BasicEffect effect2 in mesh.Effects)
						{
							effect2.EnableDefaultLighting();
							effect2.View = camera.View;
							effect2.Projection = camera.Projection;
							effect2.World = Matrix.CreateTranslation(modelPosition);
							effect2.TextureEnabled = true;
							effect2.Texture = modelTexture;
							effect.LightingEnabled = true; // Turn on the lighting subsystem.
							effect.AmbientLightColor = new Vector3(0.2f, 0.2f, 0.2f);
							effect.EmissiveColor = new Vector3(1, 0, 0);
							effect.DirectionalLight0.DiffuseColor = new Vector3(1f, 0.2f, 0.2f); // a reddish light
							effect.DirectionalLight0.Direction = new Vector3(1, 0, 0);  // coming along the x-axis
							effect.DirectionalLight0.SpecularColor = new Vector3(0, 1, 0); // with green highlights
						} */


						BoundingBox meshBox = BuildBoundingBox(
							mesh,
							Matrix.Identity
						);

						// Only showing the model if the mesh is visible
						if (camera.Frustum.Contains(meshBox)
							!= ContainmentType.Disjoint)
						{
							foreach (ModelMeshPart part in mesh.MeshParts)
							{
								// Applying our shader to all the mesh parts
								effect.Parameters["WVP"].SetValue(
									world *
									camera.View *
									camera.Projection
								);
								effect.Parameters["World"].SetValue(world);
								effect.Parameters["eyePosition"].SetValue(
									Vector3.Transform(
										camera.Position,
										Matrix.Invert(world)
									)
								);
								Vector3 cameraView = camera.ViewVector;
								cameraView = new Vector3(
									-cameraView.X,
									-cameraView.Y,
									-cameraView.Z
								);
								effect.Parameters["lightDirection"].SetValue(
									Vector3.TransformNormal(
										cameraView,
										Matrix.Invert(world)
									)
								);
								effect.Parameters["lightPosition"].SetValue(
									Vector3.Transform(
										camera.Position,
										Matrix.Invert(world)
									)
								);


								// Texturing
								if (modelTexture != null)
								{
									effect.Parameters["Texture"].SetValue(
										modelTexture
									);
									//effect.CurrentTechnique = effect.Techniques["WithTexture"];
								}
								else
								{
									//effect.CurrentTechnique = effect.Techniques["WithoutTexture"];
								}

								effect.CurrentTechnique =
									effect.Techniques["SpotLight"];
								effect.CurrentTechnique.Passes[0].Apply();

								part.Effect = effect;
							}
							mesh.Draw();
						}
					}
				}
			}
		}
	}
}
