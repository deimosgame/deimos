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
	/// This is the main type for your game
	/// </summary>
	public class Game1 : Microsoft.Xna.Framework.Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		Camera Camera;
		Floor Floor;
		BasicEffect Effect;

		Effect ShaderEffect;

		ModelManager ModelManager;



		enum GameStates
		{
			StartMenu,
			Pause,
			GraphicOptions,
			Playing
		}
		GameStates CurrentGameState = GameStates.Playing;


		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);

			Content.RootDirectory = "Content";
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			Camera = new Camera(this, new Vector3(10f, 3f, 5f), Vector3.Zero, 10f); // f means it's a float
			Components.Add(Camera);

			Floor = new Floor(GraphicsDevice, 20, 20);
			Effect = new BasicEffect(GraphicsDevice);
			DebugScreen.SetCamera(Camera);

			ModelManager = new ModelManager(Camera.Collision);

			IsMouseVisible = false;

			// Game settings
			//graphics.PreferredBackBufferHeight = 340;
			//graphics.PreferredBackBufferWidth = 480;
			//graphics.IsFullScreen = true;
			graphics.PreferMultiSampling = true; // Anti aliasing
			//graphics.SynchronizeWithVerticalRetrace = false; // Anti FPS blocking
			//IsFixedTimeStep = false; // Call the UPDATE method all the time instead of x time per sec
			graphics.ApplyChanges();


			GraphicsDevice.BlendState = BlendState.AlphaBlend;
			GraphicsDevice.DepthStencilState = DepthStencilState.None;
			GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
			GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
			GraphicsDevice.BlendState = BlendState.Opaque;
			GraphicsDevice.DepthStencilState = DepthStencilState.Default;


			base.Initialize(); 
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);

			DebugScreen.LoadFont(spriteBatch, Content.Load<SpriteFont>("Fonts/debug"));

			ModelManager.LoadModel(
				Content.Load<Model>("Models/MISC/Ana_Model"), // Model
				Content.Load<Texture2D>("Models/MISC/Alexandra/Ana_dif"), // Texture
				new Vector3(0, 0, 0) // Location
			);
			ModelManager.DoneAddingModels();

			ShaderEffect = Content.Load<Effect>("Shaders/Lights/ambient");
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// all content.
		/// </summary>
		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			// Allows the game to exit
			if (Keyboard.GetState().IsKeyDown(Keys.Back))
				this.Exit();


			GraphicsDevice.BlendState = BlendState.AlphaBlend;
			GraphicsDevice.DepthStencilState = DepthStencilState.None;
			GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
			GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
			GraphicsDevice.BlendState = BlendState.Opaque;
			GraphicsDevice.DepthStencilState = DepthStencilState.Default;


			switch (CurrentGameState)
			{
				case GameStates.StartMenu:

					break;

				case GameStates.Pause:

					break;

				case GameStates.GraphicOptions:
					
					break;

				case GameStates.Playing:
					DebugScreen.Update(gameTime);
					base.Update(gameTime);
					break;
			}
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.Black);

			//Floor.Draw(Camera, Effect);


			// Loading our effect in the ModelManager
			ModelManager.DrawModels(Camera, ShaderEffect);


			// Fixed bugs when mixing 2D and 3D (with the Sprites used in DebugScreen to draw text for instance)
			GraphicsDevice.BlendState = BlendState.AlphaBlend;
			GraphicsDevice.DepthStencilState = DepthStencilState.None;
			GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
			GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
			GraphicsDevice.BlendState = BlendState.Opaque;
			GraphicsDevice.DepthStencilState = DepthStencilState.Default;

			DebugScreen.Draw(gameTime);

			base.Draw(gameTime);
		}
	}
}
