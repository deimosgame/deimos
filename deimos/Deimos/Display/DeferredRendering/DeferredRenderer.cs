using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace Deimos
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class DeferredRenderer : DrawableGameComponent
    {
        DeimosGame MainGame;

        private QuadRenderComponent QuadRenderer;

        public RenderTarget2D ColorRT; //color and specular intensity
        public RenderTarget2D NormalRT; //normals + specular power
        public RenderTarget2D DepthRT; //depth
        public RenderTarget2D LightRT; //lighting
        public RenderTarget2D SSAORT; //lighting
        public RenderTarget2D SceneRT; //scene
        public RenderTarget2D BlurredRT; //blurredscene
        public RenderTarget2D DepthOfFieldRT; //depthoffield

        private Effect ClearBufferEffect;
        private Effect DirectionalLightEffect;
        private Effect PointLightEffect;
        private Effect SpotLightEffect;
        private Effect SSAOEffect;
        private Effect BlurEffect;
        private Effect DepthOfFieldEffect;
        private Effect FinalCombineEffect;

        private Model SphereModel; //point ligt volume
        private Model ConeModel;

        private Vector2 HalfPixel;

        public DeferredRenderer(DeimosGame game)
            : base(game)
        {
            MainGame = game;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            
            QuadRenderer = new QuadRenderComponent(Game);
            Game.Components.Add(QuadRenderer);
        }

        protected override void LoadContent()
        {
            HalfPixel = new Vector2()
            {
                X = 0.5f / (float)GraphicsDevice
                    .PresentationParameters.BackBufferWidth,
                Y = 0.5f / (float)GraphicsDevice
                    .PresentationParameters.BackBufferHeight
            };

            int backbufferWidth = 
                GraphicsDevice.PresentationParameters.BackBufferWidth;
            int backbufferHeight = 
                GraphicsDevice.PresentationParameters.BackBufferHeight;

            ColorRT = new RenderTarget2D(
                GraphicsDevice, 
                backbufferWidth, 
                backbufferHeight, 
                false, 
                SurfaceFormat.Color, 
                DepthFormat.Depth24
            );
            NormalRT = new RenderTarget2D(
                GraphicsDevice, 
                backbufferWidth, 
                backbufferHeight, 
                false, 
                SurfaceFormat.Color, 
                DepthFormat.None
            );
            DepthRT = new RenderTarget2D(
                GraphicsDevice, 
                backbufferWidth, 
                backbufferHeight, 
                false, 
                SurfaceFormat.Single, 
                DepthFormat.None
            );
            LightRT = new RenderTarget2D(
                GraphicsDevice,
                backbufferWidth,
                backbufferHeight,
                false,
                SurfaceFormat.Color,
                DepthFormat.None
            );
            SSAORT = new RenderTarget2D(
                GraphicsDevice,
                backbufferWidth,
                backbufferHeight,
                false,
                SurfaceFormat.Color,
                DepthFormat.None
            );
            SceneRT = new RenderTarget2D(
                GraphicsDevice,
                backbufferWidth,
                backbufferHeight,
                false,
                SurfaceFormat.Color,
                DepthFormat.None
            );
            BlurredRT = new RenderTarget2D(
                GraphicsDevice,
                backbufferWidth,
                backbufferHeight,
                false,
                SurfaceFormat.Color,
                DepthFormat.None
            );
            DepthOfFieldRT = new RenderTarget2D(
                GraphicsDevice,
                backbufferWidth,
                backbufferHeight,
                false,
                SurfaceFormat.Color,
                DepthFormat.None
            );


            ClearBufferEffect = Game.Content.Load<Effect>(
                @"Shaders\DeferredRendering\ClearGBuffer"
            );
            FinalCombineEffect = Game.Content.Load<Effect>(
                @"Shaders\DeferredRendering\CombineFinal"
            );
            DirectionalLightEffect = Game.Content.Load<Effect>(
                @"Shaders\Lights\DirectionalLight"
            );
            PointLightEffect = Game.Content.Load<Effect>(
                @"Shaders\Lights\PointLight"
            );
            SpotLightEffect = Game.Content.Load<Effect>(
                @"Shaders\Lights\SpotLight"
            );
            SSAOEffect = Game.Content.Load<Effect>(
                @"Shaders\SSAO\Simple"
            );
            BlurEffect = Game.Content.Load<Effect>(
                @"Shaders\MISC\Blur"
            );
            DepthOfFieldEffect = Game.Content.Load<Effect>(
                @"Shaders\DepthOfField\Simple"
            );
            SphereModel = Game.Content.Load<Model>(
                @"Models\DeferredRendering\sphere"
            );
            ConeModel = Game.Content.Load<Model>(
                @"Models\DeferredRendering\cone"
            );

            base.LoadContent();
        }

        private void SetGBuffer()
        {
            GraphicsDevice.SetRenderTargets(ColorRT, NormalRT, DepthRT);
        }

        private void ResolveGBuffer()
        {
            GraphicsDevice.SetRenderTargets(null);
        }

        private void ClearGBuffer()
        {
            ClearBufferEffect.Techniques[0].Passes[0].Apply();
            GeneralFacade.Game.GraphicsDevice.BlendState = BlendState.Opaque;
            QuadRenderer.Render(Vector2.One * -1, Vector2.One);
        }

        private void DrawDirectionalLight(Vector3 lightDirection, Color color,
            float intensity = 1f)
        {
            DirectionalLightEffect.Parameters["colorMap"].SetValue(ColorRT);
            DirectionalLightEffect.Parameters["normalMap"].SetValue(NormalRT);
            DirectionalLightEffect.Parameters["depthMap"].SetValue(DepthRT);

            DirectionalLightEffect.Parameters["lightDirection"].SetValue(
                lightDirection
            );
            DirectionalLightEffect.Parameters["Color"].SetValue(
                color.ToVector3()
            );
            DirectionalLightEffect.Parameters["Intensity"].SetValue(intensity);

            DirectionalLightEffect.Parameters["cameraPosition"].SetValue(
                DisplayFacade.Camera.Position
            );
            DirectionalLightEffect.Parameters["InvertViewProjection"].SetValue(
                Matrix.Invert(DisplayFacade.Camera.View * DisplayFacade.Camera.Projection)
            );

            DirectionalLightEffect.Parameters["halfPixel"].SetValue(HalfPixel);

            DirectionalLightEffect.Techniques[0].Passes[0].Apply();
            QuadRenderer.Render(Vector2.One * -1, Vector2.One);
        }

        private void DrawPointLight(Vector3 lightPosition, Color color,
            float lightRadius, float lightIntensity)
        {
            // Set the G-Buffer parameters
            PointLightEffect.Parameters["colorMap"].SetValue(ColorRT);
            PointLightEffect.Parameters["normalMap"].SetValue(NormalRT);
            PointLightEffect.Parameters["depthMap"].SetValue(DepthRT);

            // Compute the light world matrix
            // scale according to light radius, and translate it to 
            // light position
            Matrix sphereWorldMatrix = Matrix.CreateScale(lightRadius) *
                Matrix.CreateTranslation(lightPosition);
            PointLightEffect.Parameters["World"].SetValue(sphereWorldMatrix);
            PointLightEffect.Parameters["View"].SetValue(DisplayFacade.Camera.View);
            PointLightEffect.Parameters["Projection"]
                .SetValue(DisplayFacade.Camera.Projection);
            // Light position
            PointLightEffect.Parameters["lightPosition"]
                .SetValue(lightPosition);

            // Set the color, radius and Intensity
            PointLightEffect.Parameters["Color"].SetValue(color.ToVector3());
            PointLightEffect.Parameters["lightRadius"].SetValue(lightRadius);
            PointLightEffect.Parameters["lightIntensity"]
                .SetValue(lightIntensity);

            // Parameters for specular computations
            PointLightEffect.Parameters["cameraPosition"]
                .SetValue(DisplayFacade.Camera.Position);
            PointLightEffect.Parameters["InvertViewProjection"].SetValue(
                Matrix.Invert(DisplayFacade.Camera.View * DisplayFacade.Camera.Projection)
            );
            // Size of a halfpixel, for texture coordinates alignment
            PointLightEffect.Parameters["halfPixel"].SetValue(HalfPixel);
            // Calculate the distance between the camera and light center
            float cameraToCenter = Vector3.Distance(
                DisplayFacade.Camera.Position,
                lightPosition
            );
            // If we are inside the light volume, draw the sphere's inside face
            if (cameraToCenter < lightRadius + 1f)
                GraphicsDevice.RasterizerState = RasterizerState.CullClockwise;
            else
                GraphicsDevice.RasterizerState =
                    RasterizerState.CullCounterClockwise;

            GraphicsDevice.DepthStencilState = DepthStencilState.None;

            PointLightEffect.Techniques[0].Passes[0].Apply();
            foreach (ModelMesh mesh in SphereModel.Meshes)
            {
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    GraphicsDevice.Indices = meshPart.IndexBuffer;
                    GraphicsDevice.SetVertexBuffer(meshPart.VertexBuffer);

                    GraphicsDevice.DrawIndexedPrimitives(
                        PrimitiveType.TriangleList,
                        0,
                        0,
                        meshPart.NumVertices,
                        meshPart.StartIndex,
                        meshPart.PrimitiveCount
                    );
                }
            }

            GraphicsDevice.RasterizerState =
                RasterizerState.CullCounterClockwise;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
        }

        private void DrawSpotLight(Vector3 lightPosition, Vector3 lightDirection,
            float lightRadius, float lightAngle, Color color, float lightIntensity)
        {
            // Set the G-Buffer parameters
            SpotLightEffect.Parameters["colorMap"].SetValue(ColorRT);
            SpotLightEffect.Parameters["normalMap"].SetValue(NormalRT);
            SpotLightEffect.Parameters["depthMap"].SetValue(DepthRT);

            // Compute the light world matrix
            // scale according to light radius, and translate it to 
            // light position
            Matrix scaleMatrix = Matrix.CreateScale(lightRadius, lightAngle, lightAngle);
            Matrix coneWorldMatrix =
                Matrix.CreateRotationX(lightDirection.X) *
                Matrix.CreateRotationY(lightDirection.Y) *
                Matrix.CreateRotationZ(lightDirection.Z) *
                scaleMatrix *
                Matrix.CreateTranslation(lightPosition);
            SpotLightEffect.Parameters["World"].SetValue(coneWorldMatrix);
            SpotLightEffect.Parameters["View"].SetValue(DisplayFacade.Camera.View);
            SpotLightEffect.Parameters["Projection"]
                .SetValue(DisplayFacade.Camera.Projection);
            // Light position
            SpotLightEffect.Parameters["lightPosition"]
                .SetValue(lightPosition);

            SpotLightEffect.Parameters["lightDirection"].SetValue(lightDirection);
            SpotLightEffect.Parameters["lightAngleCosine"].SetValue((float)Math.Cos(MathHelper.ToRadians(lightAngle)));
            SpotLightEffect.Parameters["lightDecayExponent"].SetValue(100f);

            // Set the color, radius and Intensity
            SpotLightEffect.Parameters["Color"].SetValue(color.ToVector3());
            SpotLightEffect.Parameters["lightRadius"].SetValue(lightRadius);
            SpotLightEffect.Parameters["lightIntensity"]
                .SetValue(lightIntensity);

            // Parameters for specular computations
            SpotLightEffect.Parameters["cameraPosition"]
                .SetValue(DisplayFacade.Camera.Position);
            SpotLightEffect.Parameters["InvertViewProjection"].SetValue(
                Matrix.Invert(DisplayFacade.Camera.View * DisplayFacade.Camera.Projection)
            );
            // Size of a halfpixel, for texture coordinates alignment
            SpotLightEffect.Parameters["halfPixel"].SetValue(HalfPixel);
            // Calculate the distance between the camera and light center
            float cameraToCenter = Vector3.Distance(
                DisplayFacade.Camera.Position,
                lightPosition
            );
            // If we are inside the light volume, draw the sphere's inside face
            if (cameraToCenter < lightRadius + 1f)
                GraphicsDevice.RasterizerState = RasterizerState.CullClockwise;
            else
                GraphicsDevice.RasterizerState =
                    RasterizerState.CullCounterClockwise;

            GraphicsDevice.DepthStencilState = DepthStencilState.None;

            SpotLightEffect.Techniques[0].Passes[0].Apply();
            foreach (ModelMesh mesh in ConeModel.Meshes)
            {
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    GraphicsDevice.Indices = meshPart.IndexBuffer;
                    GraphicsDevice.SetVertexBuffer(meshPart.VertexBuffer);

                    GraphicsDevice.DrawIndexedPrimitives(
                        PrimitiveType.TriangleList,
                        0,
                        0,
                        meshPart.NumVertices,
                        meshPart.StartIndex,
                        meshPart.PrimitiveCount
                    );
                }
            }

            GraphicsDevice.RasterizerState =
                RasterizerState.CullCounterClockwise;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
        }

        private void DrawSSAO()
        {
            GraphicsDevice.SetRenderTarget(SSAORT);

            Vector3 cornerFrustum = Vector3.Zero;
            cornerFrustum.Y = (float)Math.Tan(Math.PI / 3.0 / 2.0) * 0.1f;
            cornerFrustum.X = cornerFrustum.Y * DisplayFacade.Camera.AspectRatio;
            cornerFrustum.Z = 0.1f;

            SSAOEffect.Parameters["Projection"].SetValue(DisplayFacade.Camera.Projection);
            SSAOEffect.Parameters["cornerFustrum"].SetValue(cornerFrustum);
            SSAOEffect.Parameters["sampleRadius"].SetValue(0);
            SSAOEffect.Parameters["distanceScale"].SetValue(0);
            SSAOEffect.Parameters["GBufferTextureSize"].SetValue(new Vector2(SSAORT.Width, SSAORT.Height));
            GeneralFacade.Game.GraphicsDevice.Textures[0] = NormalRT;
            GeneralFacade.Game.GraphicsDevice.Textures[1] = DepthRT;
            SSAOEffect.CurrentTechnique.Passes[0].Apply();

            QuadRenderer.Render(Vector2.One * -1, Vector2.One, true);

            GraphicsDevice.SetRenderTarget(null);
        }

        private void DrawBlurred()
        {
            GraphicsDevice.SetRenderTarget(BlurredRT);

            BlurEffect.Parameters["SceneTexture"].SetValue(SceneRT);
            BlurEffect.Parameters["halfPixel"].SetValue(HalfPixel);
            BlurEffect.CurrentTechnique.Passes[0].Apply();

            QuadRenderer.Render(Vector2.One * -1, Vector2.One, true);

            GraphicsDevice.SetRenderTarget(null);
        }

        private void DrawDepthOfField()
        {
            GraphicsDevice.SetRenderTarget(DepthOfFieldRT);

            Color[,] depthColors = new Color[DepthRT.Width, DepthRT.Height];

            DepthOfFieldEffect.Parameters["DepthTexture"].SetValue(DepthRT);
            DepthOfFieldEffect.Parameters["BlurredTexture"].SetValue(BlurredRT);
            DepthOfFieldEffect.Parameters["SceneTexture"].SetValue(SceneRT);
            //DepthOfFieldEffect.Parameters["CenterDepth"].SetValue(depthColors[DepthRT.Width / 2, DepthRT.Height / 2]);
            DepthOfFieldEffect.CurrentTechnique.Passes[0].Apply();

            QuadRenderer.Render(Vector2.One * -1, Vector2.One, true);

            GraphicsDevice.SetRenderTarget(null);
        }

        private void DrawLights(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(LightRT);
            GraphicsDevice.Clear(Color.Transparent);
            GraphicsDevice.BlendState = BlendState.AlphaBlend;
            GraphicsDevice.DepthStencilState = DepthStencilState.None;

            // Ambient light
            DrawDirectionalLight(
                Vector3.Zero,
                Color.White,
                GeneralFacade.SceneManager.GetCurrentAmbiantLight()
            );
            foreach (KeyValuePair<string, DirectionalLight> thisLight in
                GeneralFacade.SceneManager.LightManager.GetDirectionalLights())
            {
                DrawDirectionalLight(
                    thisLight.Value.Direction, 
                    thisLight.Value.Color
                );
            }
            foreach (KeyValuePair<string, PointLight> thisLight in
                GeneralFacade.SceneManager.LightManager.GetPointLights())
            {
                DrawPointLight(
                    thisLight.Value.Position,
                    thisLight.Value.Color,
                    thisLight.Value.Radius,
                    thisLight.Value.Intensity
                );
            }
            foreach (KeyValuePair<string, SpotLight> thisLight in
                GeneralFacade.SceneManager.LightManager.GetSpotLights())
            {
                DrawSpotLight(
                    thisLight.Value.Position,
                    thisLight.Value.Direction,
                    20,
                    60,
                    thisLight.Value.Color,
                    thisLight.Value.Power
                );
            }

            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.None;
            GraphicsDevice.RasterizerState = 
                RasterizerState.CullCounterClockwise;

            GraphicsDevice.SetRenderTarget(SceneRT);

            //Combine everything
            FinalCombineEffect.Parameters["colorMap"].SetValue(ColorRT);
            FinalCombineEffect.Parameters["lightMap"].SetValue(LightRT);
            FinalCombineEffect.Parameters["halfPixel"].SetValue(HalfPixel);

            FinalCombineEffect.Techniques[0].Passes[0].Apply();
            QuadRenderer.Render(Vector2.One * -1, Vector2.One);

            GraphicsDevice.SetRenderTarget(null);
        }


        public override void Draw(GameTime gameTime)
        {
            SetGBuffer();
            ClearGBuffer();
            GeneralFacade.SceneManager.ModelManager.DrawModels(Game, DisplayFacade.Camera);
            ResolveGBuffer();
            DrawLights(gameTime);
            DrawSSAO();
            DrawBlurred();

            // Draw the final image
            GraphicsDevice.SetRenderTarget(null);
            DisplayFacade.SpriteBatch.Begin();

            DisplayFacade.SpriteBatch.Draw(
                SceneRT,
                Vector2.Zero,
                null,
                Color.White,
                0,
                Vector2.Zero,
                new Vector2(1, 1),
                SpriteEffects.None,
                0f
            );

            DisplayFacade.SpriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
