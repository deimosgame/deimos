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
        private Dictionary<string, LevelModel> LoadedLevelModels = 
            new Dictionary<string, LevelModel>();
        ContentManager ContentManager;
 

        // Constructor
        public ModelManager(ContentManager contentManager)
        {
            ContentManager = contentManager;
        }


        // Methods
        public void LoadModel(string modelName, 
            string model, Vector3 position, float scale = 1f, 
            bool collisionDetection = true)
        {
            // Adding the model to our List/array as well as its location
            // & texture
            CollidableModel.CollidableModel thisModelCollision =
                    ContentManager.Load<CollidableModel.CollidableModel>(
                        model
                    );
            Model thisModel = thisModelCollision.model;
            LevelModel thisLevelModel = new LevelModel();
            thisLevelModel.Position = position;
            thisLevelModel.Scale = scale;
            thisLevelModel.CollisionDetection = collisionDetection;
            thisLevelModel.CollisionModel = thisModelCollision;
            LoadedLevelModels.Add(modelName, thisLevelModel);
        }

        public void UnloadModels()
        {
            ContentManager.Unload();
            LoadedLevelModels.Clear();
        }

        public Dictionary<string, LevelModel> GetLevelModels()
        {
            return LoadedLevelModels;
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

        public void DrawModels(Game game, Camera camera)
        {
            game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            game.GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
            game.GraphicsDevice.BlendState = BlendState.Opaque;


            if (LoadedLevelModels.Count > 0)
            {
                foreach (KeyValuePair<string, LevelModel> thisLevelModel in
                    LoadedLevelModels)
                {
                    // Loading the model
                    LevelModel levelModel = thisLevelModel.Value;

                    Matrix modelWorld = Matrix.CreateScale(levelModel.Scale) *
                                Matrix.CreateTranslation(levelModel.Position);

                    Matrix[] transforms = new Matrix[levelModel.CollisionModel.model.Bones.Count];
                    levelModel.CollisionModel.model.CopyAbsoluteBoneTransformsTo(transforms);

                    foreach (ModelMesh mesh in levelModel.CollisionModel.model.Meshes)
                    {
                        Matrix meshPosition = transforms[mesh.ParentBone.Index];
                        Matrix meshWorld = meshPosition * modelWorld;

                        foreach (Effect effect in mesh.Effects)
                        {
                            effect.Parameters["World"]
                                .SetValue(meshWorld);
                            effect.Parameters["View"]
                                .SetValue(camera.View);
                            effect.Parameters["Projection"]
                                .SetValue(camera.Projection);
                        }
                        mesh.Draw();
                    }
                }
            }
        }
    }
}
