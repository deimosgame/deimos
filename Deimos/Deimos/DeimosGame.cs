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

        SpriteBatch spriteBatch;
        public SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
            set { spriteBatch = value; }
        }

        ContentManager ModelsContent;
        public ContentManager TempContent
        {
            get { return ModelsContent; }
            private set { ModelsContent = value; }
        }

        Camera camera;
        public Camera Camera
        {
            get { return camera; }
            private set { camera = value; }
        }

        DeferredRenderer renderer;
        public DeferredRenderer Renderer
        {
            get { return renderer; }
            private set { renderer = value; }
        }

        LocalPlayer thisPlayer;
        public LocalPlayer ThisPlayer
        {
            get { return thisPlayer; }
            set { thisPlayer = value; }
        }

        LocalPlayerPhysics thisPlayerPhysics;
        internal LocalPlayerPhysics ThisPlayerPhysics
        {
            get { return thisPlayerPhysics; }
            set { thisPlayerPhysics = value; }
        }

        SceneManager sceneManager;
        internal SceneManager SceneManager
        {
            get { return sceneManager; }
            private  set { sceneManager = value; }
        }

        ScreenElementManager screenElementManager;
        internal ScreenElementManager ScreenElementManager
        {
            get { return screenElementManager; }
            set { screenElementManager = value; }
        }

        DebugScreen debugScreen;
        internal DebugScreen DebugScreen
        {
            get { return debugScreen; }
            set { debugScreen = value; }
        }

        Config config;
        internal Config Config
        {
            get { return config; }
            set { config = value; }
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
            ThisPlayer = new LocalPlayer(this);
            ThisPlayerPhysics = new LocalPlayerPhysics(this);


            TempContent = new ContentManager(Services);

            Graphics = new GraphicsDeviceManager(this);


            Content.RootDirectory = "Content";
            TempContent.RootDirectory = "Content";

            Renderer = new DeferredRenderer(this);
            Components.Add(renderer);

            SceneManager = new SceneManager(this, TempContent);

            Config = new Config();
        }


        protected override void Initialize()
        {
            Camera = new Camera(
                this,
                new Vector3(0f, 80f, 20f),
                Vector3.Zero
            );

            ThisPlayer.MoveTo(new Vector3(0f, 80f, 20f), Vector3.Zero);


            IsMouseVisible = false;

            // Game settings
            Graphics.PreferredBackBufferWidth = 1344;
            Graphics.PreferredBackBufferHeight = 840;
            //Graphics.PreferredBackBufferWidth = 1920;
            //Graphics.PreferredBackBufferHeight = 1080;
            //Graphics.IsFullScreen = true;
            //Graphics.PreferMultiSampling = true; // Anti aliasing - Useless as custom effects
            //Graphics.SynchronizeWithVerticalRetrace = false; // VSync
            //IsFixedTimeStep = false; // Call the UPDATE method all the time instead of x time per sec
            Graphics.ApplyChanges();


            base.Initialize();
        }


        protected override void LoadContent()
        {
            SceneManager.SetScene<SceneSkatePark>();

            SpriteBatch = new SpriteBatch(GraphicsDevice);

            ScreenElementManager = new ScreenElementManager(GraphicsDevice);

            DebugScreen = new DebugScreen(this);
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

            ThisPlayer.HandleInput(gameTime);

            SceneManager.Update();
            DebugScreen.Update(gameTime);

            base.Update(gameTime);
        }



        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);

            DebugScreen.Draw(gameTime);
            ScreenElementManager.DrawElements(SpriteBatch);
        }
    }
}
