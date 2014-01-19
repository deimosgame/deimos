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
		GraphicsDeviceManager Graphics;

        Camera camera;
        public Camera Camera
        {
            get { return camera; }
            private set { camera = value; }
        }

        SceneManager sceneManager;

        internal SceneManager SceneManager
        {
            get { return sceneManager; }
            private  set { sceneManager = value; }
        }


		public enum GameStates
		{
			StartMenu,
			Pause,
			GraphicOptions,
			Playing
		}
        public enum PlayingStates
        {
            Normal,
            MiniGame,
            NoClip
        }

        GameStates currentGameState = GameStates.Playing;
        public GameStates CurrentGameState
        {
            get { return currentGameState; }
            private set { currentGameState = value; }
        }

        PlayingStates currentPlayingState = PlayingStates.Normal;
        public PlayingStates CurrentPlayingState
        {
            get { return currentPlayingState; }
            private set { currentPlayingState = value; }
        }


		public DeimosGame()
		{
			Graphics = new GraphicsDeviceManager(this);
            Camera = new Camera(
                this,
                new Vector3(0f, 10f, 20f),
                Vector3.Zero,
                10f
            );
            Components.Add(Camera);

			Content.RootDirectory = "Content";
			DeferredRenderer renderer = new DeferredRenderer(this);
			Components.Add(renderer);

            SceneManager = new SceneManager(this);
		}


		protected override void Initialize()
		{
			IsMouseVisible = false;

			// Game settings
			Graphics.PreferredBackBufferWidth = 1344;
			Graphics.PreferredBackBufferHeight = 840;
			//graphics.IsFullScreen = true;
			//graphics.PreferMultiSampling = true; // Anti aliasing - Useless as custom effects
			//graphics.SynchronizeWithVerticalRetrace = false; // VSync
			//IsFixedTimeStep = false; // Call the UPDATE method all the time instead of x time per sec
			Graphics.ApplyChanges();


			base.Initialize(); 
		}


		protected override void LoadContent()
		{
			// 
		}


		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
		}


		protected override void Update(GameTime gameTime)
		{
			// Allows the game to exit
			if (Keyboard.GetState().IsKeyDown(Keys.Escape))
				this.Exit();

		    base.Update(gameTime);
		}



		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.Black);

			

			base.Draw(gameTime);
		}
	}
}
