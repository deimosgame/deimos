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
		private Camera Camera;
		private QuadRenderComponent QuadRenderer;

		private RenderTarget2D ColorRT; //color and specular intensity
		private RenderTarget2D NormalRT; //normals + specular power
		private RenderTarget2D DepthRT; //depth
		private RenderTarget2D LightRT; //lighting

		private Effect ClearBufferEffect;
		private Effect DirectionalLightEffect;
		private Effect PointLightEffect;
		private Effect FinalCombineEffect;

		private Model SphereModel; //point ligt volume


		private SpriteBatch SpriteBatch;

		private Vector2 HalfPixel;

		public DeferredRenderer(Game game)
			: base(game)
		{
			//
		}

		/// <summary>
		/// Allows the game component to perform any initialization it needs to before starting
		/// to run.  This is where it can query for any required services and load content.
		/// </summary>
		public override void Initialize()
		{
			// TODO: Add your initialization code here

			base.Initialize();
			Camera = new Camera(
				Game, 
				new Vector3(0f, 5f, 0f), 
				Vector3.Zero, 
				10f
			);
			QuadRenderer = new QuadRenderComponent(Game);
			Game.Components.Add(Camera);
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

			int backbufferWidth = GraphicsDevice.PresentationParameters.BackBufferWidth;
			int backbufferHeight = GraphicsDevice.PresentationParameters.BackBufferHeight;

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



			ClearBufferEffect = Game.Content.Load<Effect>(
				@"Shaders\DeferredRendering\ClearGBuffer"
			);
			FinalCombineEffect = Game.Content.Load<Effect>(
				@"Shaders\DeferredRendering\CombineFinal"
			);
			SphereModel = Game.Content.Load<Model>(
				@"Models\DeferredRendering\sphere"
			);
			DirectionalLightEffect = Game.Content.Load<Effect>(
				@"Shaders\Lights\DirectionalLight"
			);
			PointLightEffect = Game.Content.Load<Effect>(
				@"Shaders\Lights\PointLight"
			);

			SceneManager.AddScene("main");
			SceneManager.GetModelManager().LoadModel(
				Game.Content,
				"Models/Map/Sponza/sponza", // Model
				"Models/Characters/Alexandra/Ana_dif", // Texture
				new Vector3(0, 0, 0), // Location
				0.01f
			);
			//SceneManager.GetModelManager().LoadModel(
			//	Game.Content,
			//	"Models/Map/coucou", // Model
			//	"Models/Map/Grid", // Texture
			//	new Vector3(0, 0, 0), // Location
			//	1f
			//);

			SceneManager.GetLightManager().AddPointLight(
				"center", 
				new Vector3(0, 10, 0), 
				30, 
				2, 
				Color.White
			);


			SpriteBatch = new SpriteBatch(GraphicsDevice);
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
			QuadRenderer.Render(Vector2.One * -1, Vector2.One);
		}

		private void DrawDirectionalLight(Vector3 lightDirection, Color color)
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

			DirectionalLightEffect.Parameters["cameraPosition"].SetValue(
				Camera.Position
			);
			DirectionalLightEffect.Parameters["InvertViewProjection"].SetValue(
				Matrix.Invert(Camera.View * Camera.Projection)
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
			PointLightEffect.Parameters["View"].SetValue(Camera.View);
			PointLightEffect.Parameters["Projection"]
				.SetValue(Camera.Projection);
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
				.SetValue(Camera.Position);
			PointLightEffect.Parameters["InvertViewProjection"].SetValue(
				Matrix.Invert(Camera.View * Camera.Projection)
			);
			// Size of a halfpixel, for texture coordinates alignment
			PointLightEffect.Parameters["halfPixel"].SetValue(HalfPixel);
			// Calculate the distance between the camera and light center
			float cameraToCenter = Vector3.Distance(
				Camera.Position, 
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

		public override void Draw(GameTime gameTime)
		{
			SetGBuffer();
			ClearGBuffer();
			SceneManager.GetModelManager().DrawModels(Game, Camera);
			ResolveGBuffer();
			DrawLights(gameTime);

			base.Draw(gameTime);
		}

		private void DrawLights(GameTime gameTime)
		{
			GraphicsDevice.SetRenderTarget(LightRT);
			GraphicsDevice.Clear(Color.Transparent);
			GraphicsDevice.BlendState = BlendState.AlphaBlend;
			GraphicsDevice.DepthStencilState = DepthStencilState.None;

			foreach (KeyValuePair<string, DirectionalLight> thisLight in 
				SceneManager.GetLightManager().GetDirectionalLights())
			{
				DrawDirectionalLight(
					thisLight.Value.Direction, 
					thisLight.Value.Color
				);
			}
			foreach (KeyValuePair<string, PointLight> thisLight in
				SceneManager.GetLightManager().GetPointLights())
			{
				DrawPointLight(
					thisLight.Value.Position,
					thisLight.Value.Color,
					thisLight.Value.Radius,
					thisLight.Value.Intensity
				);
			}

			GraphicsDevice.BlendState = BlendState.Opaque;
			GraphicsDevice.DepthStencilState = DepthStencilState.None;
			GraphicsDevice.RasterizerState = 
				RasterizerState.CullCounterClockwise;

			GraphicsDevice.SetRenderTarget(null);

			//Combine everything
			FinalCombineEffect.Parameters["colorMap"].SetValue(ColorRT);
			FinalCombineEffect.Parameters["lightMap"].SetValue(LightRT);
			FinalCombineEffect.Parameters["halfPixel"].SetValue(HalfPixel);

			FinalCombineEffect.Techniques[0].Passes[0].Apply();
			QuadRenderer.Render(Vector2.One * -1, Vector2.One);

			//DebugScreen.Draw(gameTime);
		}

		/// <summary>
		/// Allows the game component to update itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public override void Update(GameTime gameTime)
		{
			//

			base.Update(gameTime);
		}
	}
}
