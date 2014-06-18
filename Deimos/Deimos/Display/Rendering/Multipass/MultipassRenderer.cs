using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    class MultipassRenderer : DrawableGameComponent
    {
        DeimosGame MainGame;
        public Effect MultipassEffect;

        public MultipassRenderer(DeimosGame game) : base(game)
        {
            MainGame = game;
        }



        public override void Initialize()
        {


            base.Initialize();
        }

        protected override void LoadContent()
        {
            MultipassEffect = Game.Content.Load<Effect>(@"Shaders\Multipass\MultiPassLight");

            MultipassEffect.Parameters["globalAmbient"].SetValue(Color.White.ToVector3());
            MultipassEffect.Parameters["Ke"].SetValue(0.0f);
            MultipassEffect.Parameters["Ka"].SetValue(0.05f);
            MultipassEffect.Parameters["Kd"].SetValue(1.0f);
            MultipassEffect.Parameters["Ks"].SetValue(0.3f);
            MultipassEffect.Parameters["specularPower"].SetValue(100);
            MultipassEffect.Parameters["spotPower"].SetValue(20);

            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            //GeneralFacade.SceneManager.ModelManager.DrawModels(Game, DisplayFacade.Camera);
            DrawModels(GeneralFacade.SceneManager.ModelManager.GetLevelModels());
            DrawModels(GeneralFacade.SceneManager.ModelManager.GetPrivateModels());

            base.Draw(gameTime);
        }

        private void DrawModels(Dictionary<string, LevelModel> LoadedModels)
        {
            foreach (KeyValuePair<string, LevelModel> item in LoadedModels)
            {
                LevelModel levelModel = item.Value;
                // Loading the model
                Model model = levelModel.CollisionModel.model;
                // The model position
                Matrix modelWorld = levelModel.WorldMatrix;
                // Its texture
                List<Texture2D> meshesTextures = levelModel.TextureList;

                // Creating our transforms matrix
                Matrix[] transforms = new Matrix[model.Bones.Count];
                // And applying bones to it
                model.CopyAbsoluteBoneTransformsTo(transforms);
                foreach (ModelMesh mesh in model.Meshes)
                {
                    int iMesh = model.Meshes.IndexOf(mesh);

                    // Showing up our model with our lights
                    ApplyLights(mesh, modelWorld, meshesTextures[iMesh]);
                }
            }
        }

        private void ApplyLights(ModelMesh mesh, Matrix world,
            Texture2D modelTexture)
        {
            GraphicsDevice graphicsDevice = GeneralFacade.Game.GraphicsDevice;
            Camera camera = DisplayFacade.Camera;
            // Looping thorugh all of its meshparts
            foreach (ModelMeshPart part in mesh.MeshParts)
            {
                Effect effect = part.Effect;
                graphicsDevice.SetVertexBuffer(part.VertexBuffer);
                graphicsDevice.Indices = part.IndexBuffer;

                // Setting up the default settings
                effect.Parameters["WVP"].SetValue(
                    world *
                    camera.View *
                    camera.Projection
                );
                effect.Parameters["World"].SetValue(world);
                effect.Parameters["eyePosition"].SetValue(
                    camera.Position
                );

                GraphicsDevice.BlendState = BlendState.Opaque;
                GraphicsDevice.DepthStencilState = DepthStencilState.Default;

                effect.CurrentTechnique.Passes["Ambient"].Apply();
                graphicsDevice.DrawIndexedPrimitives(
                    PrimitiveType.TriangleList,
                    part.VertexOffset,
                    0,
                    part.NumVertices,
                    part.StartIndex,
                    part.PrimitiveCount
                );




                // Texturing
                graphicsDevice.BlendState = BlendState.AlphaBlend;
                if (modelTexture != null)
                {
                    effect.Parameters["Texture"].SetValue(
                        modelTexture
                    );
                }

                // Drawing the changes
                graphicsDevice.DrawIndexedPrimitives(
                    PrimitiveType.TriangleList,
                    part.VertexOffset,
                    0,
                    part.NumVertices,
                    part.StartIndex,
                    part.PrimitiveCount
                );


                GraphicsDevice.BlendState = BlendState.Additive;

                foreach (KeyValuePair<string, DirectionalLight> thisLight in
                    GeneralFacade.SceneManager.LightManager.GetDirectionalLights())
                {
                    DirectionalLight light = thisLight.Value;
                    effect.Parameters["lightColor"].SetValue(light.Color.ToVector3());
                    effect.Parameters["lightDirection"].SetValue(light.Direction);

                    effect.CurrentTechnique.Passes["Directional"].Apply();
                    graphicsDevice.DrawIndexedPrimitives(
                        PrimitiveType.TriangleList,
                        part.VertexOffset,
                        0,
                        part.NumVertices,
                        part.StartIndex,
                        part.PrimitiveCount
                    );
                }

                foreach (KeyValuePair<string, PointLight> thisLight in
                    GeneralFacade.SceneManager.LightManager.GetPointLights())
                {
                    PointLight light = thisLight.Value;

                    effect.Parameters["lightColor"].SetValue(light.Color.ToVector3());
                    effect.Parameters["lightPosition"].SetValue(light.Position);

                    effect.CurrentTechnique.Passes["Point"].Apply();
                    graphicsDevice.DrawIndexedPrimitives(
                        PrimitiveType.TriangleList,
                        part.VertexOffset,
                        0,
                        part.NumVertices,
                        part.StartIndex,
                        part.PrimitiveCount
                    ); 
                }

                foreach (KeyValuePair<string, SpotLight> thisLight in
                    GeneralFacade.SceneManager.LightManager.GetSpotLights())
                {
                    SpotLight light = thisLight.Value;

                    effect.Parameters["lightColor"].SetValue(light.Color.ToVector3());
                    effect.Parameters["lightPosition"].SetValue(light.Position);
                    effect.Parameters["lightDirection"].SetValue(light.Direction);
                    effect.Parameters["spotPower"].SetValue(light.Power);

                    effect.CurrentTechnique.Passes["Spot"].Apply();
                    graphicsDevice.DrawIndexedPrimitives(
                        PrimitiveType.TriangleList,
                        part.VertexOffset,
                        0,
                        part.NumVertices,
                        part.StartIndex,
                        part.PrimitiveCount
                    ); 
                }
            }
        }

        public override void Update(GameTime gameTime)
        {


            base.Update(gameTime);
        }
    }
}
