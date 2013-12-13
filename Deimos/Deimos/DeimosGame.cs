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
	public partial class DeimosGame : Microsoft.Xna.Framework.Game
	{
		GraphicsDeviceManager graphics;



		enum GameStates
		{
			StartMenu,
			Pause,
			GraphicOptions,
			Playing
		}
		GameStates CurrentGameState = GameStates.Playing;


		public DeimosGame()
		{
			graphics = new GraphicsDeviceManager(this);

			Content.RootDirectory = "Content";
			DeferredRenderer renderer = new DeferredRenderer(this);
			Components.Add(renderer);

		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			IsMouseVisible = false;

			// Game settings
			graphics.PreferredBackBufferWidth = 1344;
			graphics.PreferredBackBufferHeight = 840;
			//graphics.IsFullScreen = true;
			//graphics.PreferMultiSampling = true; // Anti aliasing - Useless as custom effects
			//graphics.SynchronizeWithVerticalRetrace = false; // VSync
			//IsFixedTimeStep = false; // Call the UPDATE method all the time instead of x time per sec
			graphics.ApplyChanges();


			base.Initialize(); 
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// 
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

			//DebugScreen.Draw(gameTime);

			base.Draw(gameTime);
		}
	}
}
