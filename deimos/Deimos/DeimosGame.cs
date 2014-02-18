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

        LocalPlayerDisplay thisPlayerDisplay;
        internal LocalPlayerDisplay ThisPlayerDisplay
        {
            get { return thisPlayerDisplay; }
            set { thisPlayerDisplay = value; }
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

        WeaponsList weapons;
        internal WeaponsList Weapons
        {
            get { return weapons; }
            set { weapons = value; }
        }

        BulletManager bulletManager;
        internal BulletManager BulletManager
        {
            get { return bulletManager; }
            set { bulletManager = value; }
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
            ThisPlayerPhysics = new LocalPlayerPhysics(this, thisPlayer);
            ThisPlayerDisplay = new LocalPlayerDisplay(this);

            TempContent = new ContentManager(Services);

            Graphics = new GraphicsDeviceManager(this);


            Content.RootDirectory = "Content";
            TempContent.RootDirectory = "Content";

            Renderer = new DeferredRenderer(this);
            Components.Add(renderer);

            SceneManager = new SceneManager(this, TempContent);

            Config = new Config();

            Weapons = new WeaponsList(this);
            BulletManager = new BulletManager(this);
        }

        protected override void Initialize()
        {
            Weapons.Initialise();

            Camera = new Camera(
                this,
                new Vector3(0f, 9f,20f),
                Vector3.Zero
            );

            ThisPlayer.PlayerSpawn(new Vector3(-45f, 11f, -8f), Vector3.Zero);

            IsMouseVisible = false;

            // Game settings
            Graphics.PreferredBackBufferWidth = 1344;
            Graphics.PreferredBackBufferHeight = 840;
            //Graphics.PreferredBackBufferWidth = 1920;
            //Graphics.PreferredBackBufferHeight = 1080;
            //Graphics.IsFullScreen = true;
            //Graphics.PreferMultiSampling = true; // Anti aliasing - Useless as custom effects
            Graphics.SynchronizeWithVerticalRetrace = false; // VSync
            //IsFixedTimeStep = false; // Call the UPDATE method all the time instead of x time per sec
            Graphics.ApplyChanges();


            base.Initialize();
        }


        protected override void LoadContent()
        {
            SceneManager.SetScene<SceneDeimos>();

            SpriteBatch = new SpriteBatch(GraphicsDevice);

            ScreenElementManager = new ScreenElementManager(this);

            DebugScreen = new DebugScreen(this);

            ThisPlayer.InitializeInventory();
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

            // Testing purposes: switching clip/noclip
            if (Keyboard.GetState().IsKeyDown(Keys.N))
            {
                CurrentPlayingState = DeimosGame.PlayingStates.NoClip;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.M))
            {
                CurrentPlayingState = DeimosGame.PlayingStates.Normal;
            }

            // for testing: player death and respawning
            if (Keyboard.GetState().IsKeyDown(Keys.J))
            {
                ThisPlayer.PlayerRespawn(new Vector3(-45f, 11f, -8f), Vector3.Zero);
                CurrentPlayingState = PlayingStates.Normal;
            }
            
            if (Keyboard.GetState().IsKeyDown(Keys.K))
            {
                ThisPlayer.PlayerKill();
                CurrentPlayingState = PlayingStates.NoClip;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.L))
            {
                ThisPlayer.Health--;

                if (ThisPlayer.Health == 0)
                {
                    ThisPlayer.PlayerKill();
                    CurrentPlayingState = PlayingStates.NoClip;
                }
            }

            if (currentGameState == GameStates.Playing &&
                (ThisPlayer.CurrentLifeState == Player.LifeState.Alive ||
                CurrentPlayingState == PlayingStates.NoClip))
            {
                ThisPlayer.HandleInput(gameTime);

                SceneManager.Update();
                BulletManager.Update(gameTime);

                this.IsMouseVisible = false;
            }
            else
            {
                this.IsMouseVisible = true;
            }

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
