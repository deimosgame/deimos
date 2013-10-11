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

		Menu MainMenu;



		enum GameStates
		{
			StartMenu,
			Pause,
			GraphicOptions,
			Playing
		}
		GameStates CurrentGameState = GameStates.StartMenu;


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
			Camera = new Camera(this, new Vector3(10f, 3f, 5f), Vector3.Zero, 5f); // f means it's a float
			Components.Add(Camera);

			Floor = new Floor(GraphicsDevice, 20, 20);
			Effect = new BasicEffect(GraphicsDevice);
			DebugScreen.SetCamera(Camera);


			IsMouseVisible = false;

			// Game settings
			//graphics.PreferredBackBufferHeight = 340;
			//graphics.PreferredBackBufferWidth = 480;
			graphics.IsFullScreen = true;
			graphics.PreferMultiSampling = true; // Anti aliasing
			//graphics.SynchronizeWithVerticalRetrace = false; // Anti FPS blocking
			//IsFixedTimeStep = false; // Call the UPDATE method all the time instead of x time per sec
			graphics.ApplyChanges();


			MainMenu = new Menu("Menu Title");
			MainMenu.AddMenuItem("Start Game", k => { if (k == Keys.Enter) { CurrentGameState = GameStates.Playing; } });
			MainMenu.AddMenuItem("Options", k => { if (k == Keys.Enter) { CurrentGameState = GameStates.GraphicOptions; } });
			MainMenu.AddMenuItem("Exit", k => { if (k == Keys.Enter) { Exit(); } });

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

			// TODO: use this.Content to load your game content here
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


			switch (CurrentGameState)
			{
				case GameStates.StartMenu:
					//MainMenu.DrawMenu(800);
					//MainMenu.Navigate(Keyboard.GetState(), gameTime);
					break;

				case GameStates.Pause:

					break;

				case GameStates.GraphicOptions:
					MainMenu.Navigate(Keyboard.GetState(), gameTime);
					break;

				case GameStates.Playing:
					

					break;
			}

			DebugScreen.Update(gameTime);
			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			Floor.Draw(Camera, Effect);

			DebugScreen.Draw(gameTime);


			base.Draw(gameTime);
		}
	}
}
