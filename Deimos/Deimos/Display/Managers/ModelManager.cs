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
		private List<Model> LoadedModels = new List<Model>();
		private List<List<Texture2D>> LoadedMeshesTextures = new List<List<Texture2D>>();
		private List<Matrix> LoadedModelsWorld = new List<Matrix>();

		private bool FirstRender = true;

		Effect Effect;

		Collision CollisionManager;
 

		// Constructor
		public ModelManager(Effect effect, Collision collisionManager)
		{
			Effect = effect;
			CollisionManager = collisionManager;
		}


		// Methods
		public void LoadModel(ContentManager content, string model, 
			Vector3 position, float scale)
		{
			// Adding the model to our List/array as well as its location
			// & texture
			Model thisModel = content.Load<Model>(model);
			Matrix worldModel = 
				Matrix.CreateScale(scale) * Matrix.CreateTranslation(position);
			LoadedModels.Add(thisModel);
			LoadedModelsWorld.Add(
				worldModel
			);

			// Looping through its meshes to calculate its hitboxes and add 
			// them to our collision class
			List<Texture2D> texturesList = new List<Texture2D>();
			foreach (ModelMesh mesh in thisModel.Meshes)
			{
				CollisionManager.AddCollisionBoxDirectly(
					BuildBoundingBox(
						mesh,
						worldModel
					)
				);
				texturesList.Add(((BasicEffect)mesh.Effects[0]).Texture);

				foreach (ModelMeshPart part in mesh.MeshParts)
				{
					part.Effect = Effect.Clone();
				}
			}
			LoadedMeshesTextures.Add(texturesList);
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

		public void DrawModels(Camera camera, GraphicsDevice graphicsDevice,
			LightManager lightManager)
		{
			for (int i = 0; i < LoadedModels.Count; i++)
			{
				// Loading the model
				Model model = LoadedModels[i];
				// The model position
				Matrix modelWorld = LoadedModelsWorld[i];
				// Its texture
				List<Texture2D> meshesTextures = LoadedMeshesTextures[i];

				// Creating our transforms matrix
				Matrix[] transforms = new Matrix[model.Bones.Count];
				// And applying bones to it
				model.CopyAbsoluteBoneTransformsTo(transforms);
				foreach (ModelMesh mesh in model.Meshes)
				{
					int iMesh = model.Meshes.IndexOf(mesh);
					// Building the box of the mesh to know if it's visible
					BoundingBox meshBox = BuildBoundingBox(
						mesh,
						modelWorld
					);

					// Showing up our model with our lights
					lightManager.ApplyLights(
						mesh, 
						modelWorld, 
						meshesTextures[iMesh], 
						camera, 
						graphicsDevice
					);
				}
			}

			// Not our first rendering anymore
			FirstRender = false;
		}
	}
}
