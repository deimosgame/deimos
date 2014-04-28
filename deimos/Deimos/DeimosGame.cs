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

        public SpriteBatch SpriteBatch
        {
            get;
            set;
        }

        public ContentManager TempContent
        {
            get;
            private set;
        }

        public Camera Camera
        {
            get;
            private set;
        }

        public DeferredRenderer Renderer
        {
            get;
            private set;
        }

        public LocalPlayer ThisPlayer
        {
            get;
            set;
        }

        internal LocalPlayerPhysics ThisPlayerPhysics
        {
            get;
            set;
        }

        internal LocalPlayerDisplay ThisPlayerDisplay
        {
            get;
            set;
        }

        internal SceneManager SceneManager
        {
            get;
            private set;
        }

        internal ScreenElementManager ScreenElementManager
        {
            get;
            set;
        }

        internal WeaponsList Weapons
        {
            get;
            set;
        }

        internal ObjectsList Objects
        {
            get;
            set;
        }

        internal BulletManager BulletManager
        {
            get;
            set;
        }

        internal DebugScreen DebugScreen
        {
            get;
            set;
        }

        internal Config Config
        {
            get;
            set;
        }

        internal Constants Constants
        {
            get;
            set;
        }

        internal MenuManager MenuManager
        {
            get;
            set;
        }



        public enum GameStates
        {
            IntroStarting,
            Intro,
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



        GameStates currentGameState = GameStates.IntroStarting;
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

        private VideoPlayer VideoPlayer;
        private Video IntroVideo;


        public DeimosGame()
        {
            TempContent = new ContentManager(Services);

            Graphics = new GraphicsDeviceManager(this);

            VideoPlayer = new VideoPlayer();


            Content.RootDirectory = "Content";
            TempContent.RootDirectory = "Content";

            Renderer = new DeferredRenderer(this);
            Components.Add(Renderer);


            Config = new Config();
            Constants = new Constants();
        }

        protected override void Initialize()
        {
            IsMouseVisible = false;

            // Game settings
            Graphics.PreferredBackBufferWidth = 1344;
            Graphics.PreferredBackBufferHeight = 840;
            //Graphics.PreferredBackBufferWidth = 1400;
            //Graphics.PreferredBackBufferHeight = 1050;
            //Graphics.IsFullScreen = true;
            //Graphics.PreferMultiSampling = true; // Anti aliasing - Useless as custom effects
            Graphics.SynchronizeWithVerticalRetrace = false; // VSync
            //IsFixedTimeStep = false; // Call the UPDATE method all the time instead of x time per sec
            Graphics.ApplyChanges();


            base.Initialize();
        }


        protected override void LoadContent()
        {
            IntroVideo = Content.Load<Video>("Videos/Intro");

            SpriteBatch = new SpriteBatch(GraphicsDevice);

            ScreenElementManager = new ScreenElementManager(this);

            DebugScreen = new DebugScreen(this);

            MenuManager = new MenuManager(this);


            //////////////////////////////////////////////////////
            // Menus
            /////////////////////////////////////////////////////
            MenuScreen StartScreen = MenuManager.Add("Start");
            StartScreen.AddElement("Play", delegate(ScreenElement el, DeimosGame game)
            {
                MenuManager.Hide();
                game.CurrentGameState = GameStates.Playing;
                game.InitGameplay();
            });
            StartScreen.AddElement("Exit", delegate(ScreenElement el, DeimosGame game)
            {
                this.Exit();
            });

            MenuScreen PauseScreen = MenuManager.Add("Pause");
            PauseScreen.AddElement("Resume", delegate(ScreenElement el, DeimosGame game)
            {
                // Making sure the mouse is in the center of the screen:
                // We don't want to generate a movement when leaving this
                // pause screen.
                Mouse.SetPosition(GraphicsDevice.Viewport.Width / 2,
                GraphicsDevice.Viewport.Height / 2);
                MenuManager.Hide();
                game.CurrentGameState = GameStates.Playing;
            });
            PauseScreen.AddElement("Exit", delegate(ScreenElement el, DeimosGame game)
            {
                this.Exit();
            });
            /////////////////////////////////////////////////////
            /////////////////////////////////////////////////////
        }

         
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }


        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                MenuManager.Set("Pause");
                CurrentGameState = GameStates.Pause;
            }

            if (CurrentGameState == GameStates.IntroStarting &&
                VideoPlayer.State == MediaState.Stopped)
            {
                VideoPlayer.Play(IntroVideo);
                CurrentGameState = GameStates.Intro;
                ScreenElementManager.AddImage("Intro", 0, 0, 1, 1, 1, null);
            }

            switch (CurrentGameState)
            {
                case GameStates.IntroStarting:
                    if(VideoPlayer.State == MediaState.Stopped)
                    {
                        VideoPlayer.Play(IntroVideo);
                        CurrentGameState = GameStates.Intro;
                        ScreenElementManager.AddImage("Intro", 0, 0, 1, 1, 1, null);
                    }
                    break;
                case GameStates.Intro:
                    //
                    break;
                case GameStates.StartMenu:
                case GameStates.Pause:
                case GameStates.GraphicOptions:
                    IsMouseVisible = true;
                    ScreenElementManager.HandleMouse();
                    break;
                case GameStates.Playing:
                default:
                    IsMouseVisible = false;

                    if (ThisPlayer.IsAlive())
                    {
                        ThisPlayer.HandleInput(gameTime);
                    }
                    else
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.NumPad1))
                        { 
                            ThisPlayer.Class = Player.Spec.Soldier;
                        }
                        if (Keyboard.GetState().IsKeyDown(Keys.NumPad2))
                        { 
                            ThisPlayer.Class = Player.Spec.Overwatch; 
                        }
                        if (Keyboard.GetState().IsKeyDown(Keys.NumPad3))
                        { 
                            ThisPlayer.Class = Player.Spec.Agent; 
                        }
                    }
                    SceneManager.Update();
                    BulletManager.Update(gameTime);

                    TestBindings(gameTime);
                    break;
            }

            DebugScreen.Update(gameTime);

            base.Update(gameTime);
        }



        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);

            if (CurrentGameState == GameStates.Intro)
            {
                Texture2D videoTexture = null;
                if (VideoPlayer.State != MediaState.Stopped)
                {
                    videoTexture = VideoPlayer.GetTexture();
                }

                // Draw the video, if we have a texture to draw.
                if (videoTexture != null)
                {
                    ScreenImage sImage = ScreenElementManager.GetImage("Intro");
                    sImage.Image = videoTexture;
                    float height = GraphicsDevice.Viewport.Height;
                    float width = GraphicsDevice.Viewport.Width;
                    float vHeight = videoTexture.Height;
                    float vWidth = videoTexture.Width;
                    sImage.ScaleX = width / vWidth;
                    sImage.ScaleY = height / vHeight;
                }
                else
                {
                    CurrentGameState = GameStates.StartMenu;
                    VideoPlayer.Dispose();
                    ScreenElementManager.RemoveImage("Intro");

                    MenuManager.Set("Start");
                }
            }

            DebugScreen.Draw(gameTime);
            ScreenElementManager.DrawElements(SpriteBatch);
        }

        private void InitGameplay()
        {
            Weapons = new WeaponsList(this);
            BulletManager = new BulletManager(this);
            Objects = new ObjectsList();

            Weapons.Initialise();
            Objects.Initialize();

            ThisPlayer = new LocalPlayer(this);
            ThisPlayerPhysics = new LocalPlayerPhysics(this);
            ThisPlayerDisplay = new LocalPlayerDisplay(this);

            SceneManager = new SceneManager(this, TempContent);
            SceneManager.SetScene<SceneDeimos>();

            

            Camera = new Camera(
                this,
                new Vector3(0f, 9f, 20f),
                Vector3.Zero
            );

            ThisPlayer.Inventory = new WeaponManager(this);
            ThisPlayer.InitializeInventory(ThisPlayer.Class);
            ThisPlayer.PlayerSpawn(new Vector3(-60f, 20f, -8f), Vector3.Zero);
        }

        private void TestBindings(GameTime gameTime)
        {
            // Testing purposes: switching clip/noclip
            if (Keyboard.GetState().IsKeyDown(Keys.N))
            {
                CurrentPlayingState = DeimosGame.PlayingStates.NoClip;
                ThisPlayerPhysics.timer_gravity = 0;
                ThisPlayerPhysics.acceleration = Vector3.Zero;
                ThisPlayerPhysics.initial_velocity = 0;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.M))
            {
                CurrentPlayingState = DeimosGame.PlayingStates.Normal;
            }

            // for testing: player death and respawning
            if (Keyboard.GetState().IsKeyDown(Keys.J))
            {
                ThisPlayer.PlayerRespawn(new Vector3(-45f, 11f, -8f), Vector3.Zero, "main");
                ThisPlayer.InitializeInventory(ThisPlayer.Class);
                CurrentPlayingState = PlayingStates.Normal;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.K))
            {
                ThisPlayer.PlayerKill();
                ThisPlayer.Inventory.Flush();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.L))
            {
                ThisPlayer.Health--;

                if (ThisPlayer.Health == 0)
                {
                    ThisPlayer.PlayerKill();
                }
            }
        }
    }
}
