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
        private Dictionary<string, LevelModel> LoadedPrivateModels =
            new Dictionary<string, LevelModel>();
 

        // Constructor
        public ModelManager()
        {
            //
        }


        // Methods

        /// <summary>
        /// Load a model to the scene, with some defaulf configuration.
        /// </summary>
        /// <param name="modelName">The name of the model</param>
        /// <param name="model">The path to the model</param>
        /// <param name="position">The position of the model</param>
        /// <param name="rotation">The rotation of the model</param>
        /// <param name="scale">The scale of the model</param>
        /// <param name="collisionType">The collision type of the model</param>
        public void LoadModel(string modelName, string model, Vector3 position,
            Vector3 rotation, float scale = 1,
            LevelModel.CollisionType collisionType = LevelModel.CollisionType.Accurate)
        {
            // Adding the model to our List/array as well as its location
            // & texture
            CollidableModel.CollidableModel thisModelCollision =
                    GeneralFacade.SceneManager.ResourceManager.LoadModel(model);
            Model thisModel = thisModelCollision.model;
            LevelModel thisLevelModel = new LevelModel();
            thisLevelModel.Position = position;
            thisLevelModel.Scale = scale;
            thisLevelModel.Rotation = rotation;
            thisLevelModel.CollisionDetection = collisionType;
            thisLevelModel.CollisionModel = thisModelCollision;
            LoadedLevelModels.Add(modelName, thisLevelModel);
            GeneralFacade.SceneManager.CollisionManager.AddLevelModel(
                thisLevelModel,
                delegate(CollisionElement el, DeimosGame game) { }
            );
        }
        /// <summary>
        /// Used to load game models ONLY. DON'T use that in the scene!
        /// </summary>
        /// <param name="modelName">The name of the model</param>
        /// <param name="model">The path to the model</param>
        /// <param name="position">The position of the model</param>
        /// <param name="rotation">The rotation of the model</param>
        /// <param name="scale">The scale of the model</param>
        /// <param name="collisionType">The collision type of the model</param>
        public void LoadPrivateModel(string modelName, string model, Vector3 position,
            Vector3 rotation, float scale = 1,
            LevelModel.CollisionType collisionType = LevelModel.CollisionType.Accurate)
        {
            // Adding the model to our List/array as well as its location
            // & texture
            CollidableModel.CollidableModel thisModelCollision =
                    GeneralFacade.SceneManager.ResourceManager.LoadModel(model);
            Model thisModel = thisModelCollision.model;
            LevelModel thisLevelModel = new LevelModel();
            thisLevelModel.Position = position;
            thisLevelModel.Scale = scale;
            thisLevelModel.Rotation = rotation;
            thisLevelModel.CollisionDetection = collisionType;
            thisLevelModel.CollisionModel = thisModelCollision;
            LoadedPrivateModels.Add(modelName, thisLevelModel);
            GeneralFacade.SceneManager.CollisionManager.AddLevelModel(
                thisLevelModel,
                delegate(CollisionElement el, DeimosGame game) { }
            );
        }

        /// <summary>
        /// Used to get the dictionnary containing all the level models.
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, LevelModel> GetLevelModels()
        {
            return LoadedLevelModels;
        }

        /// <summary>
        /// Used to get a specific level model.
        /// </summary>
        /// <param name="name">Level model name.</param>
        /// <returns></returns>
        public LevelModel GetLevelModel(string name)
        {
            return LoadedLevelModels[name];
        }

        /// <summary>
        /// Used to get a specific level model. DON'T use that in the scene!
        /// </summary>
        /// <param name="name">Level model name.</param>
        /// <returns></returns>
        public LevelModel GetPrivateModel(string name)
        {
            return LoadedPrivateModels[name];
        }

        /// <summary>
        /// Used to remove a level model from the scene.
        /// </summary>
        /// <param name="name">The name of the model to remove</param>
        public void RemoveLevelModel(string name)
        {
            LoadedLevelModels.Remove(name);
        }

        /// <summary>
        /// Used to remove a private model from the game. DON'T use that in the scene!
        /// </summary>
        /// <param name="name">The name of the model to remove</param>
        public void RemovePrivateModel(string name)
        {
            LoadedPrivateModels.Remove(name);
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

        /// <summary>
        /// Draw all the models, applying the effect to them.
        /// </summary>
        /// <param name="game"></param>
        /// <param name="camera"></param>
        public void DrawModels(Game game, Camera camera)
        {
            game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            game.GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
            game.GraphicsDevice.BlendState = BlendState.Opaque;

            IterateModels(game, camera, LoadedLevelModels);
            IterateModels(game, camera, LoadedPrivateModels);
        }

        public void IterateModels(Game game, Camera camera, 
            Dictionary<string, LevelModel> models)
        {
            foreach (KeyValuePair<string, LevelModel> thisLevelModel in
                    models)
            {
                if (thisLevelModel.Value.show)
                {
                    // Loading the model
                    LevelModel levelModel = thisLevelModel.Value;

                    Matrix[] transforms = new Matrix[levelModel.CollisionModel.model.Bones.Count];
                    levelModel.CollisionModel.model.CopyAbsoluteBoneTransformsTo(transforms);

                    foreach (ModelMesh mesh in levelModel.CollisionModel.model.Meshes)
                    {
                        Matrix meshPosition = transforms[mesh.ParentBone.Index];
                        Matrix meshWorld = meshPosition * thisLevelModel.Value.WorldMatrix;

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

        public bool LevelModelExists(string name)
        {
            return (LoadedLevelModels.ContainsKey(name));
        }
    }
}
