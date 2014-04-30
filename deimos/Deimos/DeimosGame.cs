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

        public DeferredRenderer Renderer
        {
            get;
            private set;
        }



        public enum PlayingStates
        {
            Normal,
            MiniGame,
            NoClip
        }


        PlayingStates currentPlayingState = PlayingStates.Normal;
        public PlayingStates CurrentPlayingState
        {
            get { return currentPlayingState; }
            private set { currentPlayingState = value; }
        }

        public VideoPlayer VideoPlayer;
        public Video IntroVideo;


        public DeimosGame()
        {
            GeneralFacade.Game = this;

            GeneralFacade.TempContent = new ContentManager(Services);

            Graphics = new GraphicsDeviceManager(this);


            VideoPlayer = new VideoPlayer();


            Content.RootDirectory = "Content";
            GeneralFacade.TempContent.RootDirectory = "Content";

            Renderer = new DeferredRenderer(this);
            Components.Add(Renderer);

            DisplayFacade.ParticleManager = new Tranquillity.ParticleManager(this);
            DisplayFacade.ParticleManager.Visible = true;
            Components.Add(DisplayFacade.ParticleManager);

            GeneralFacade.Config = new Config();
            GameplayFacade.Constants = new Constants();
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

            DisplayFacade.SpriteBatch = new SpriteBatch(GraphicsDevice);

            GeneralFacade.GameStateManager = new GameStateManager();
            GeneralFacade.GameStateManager.Set<IntroStartingGS>();

            DisplayFacade.ScreenElementManager = new ScreenElementManager();

            DisplayFacade.DebugScreen = new DebugScreen();

            GeneralFacade.SceneManager = new SceneManager(GeneralFacade.TempContent);

            DisplayFacade.ModelAnimationManager = new ModelAnimationManager();

            DisplayFacade.Camera = new Camera(
                Vector3.Zero,
                Vector3.Zero
            );


            GeneralFacade.SceneManager.SetScene<SceneStartMenu>();
        }

         
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }


        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape)
                && GeneralFacade.GameStateManager.CurrentGameState == GameStates.Playing)
            {
                GeneralFacade.GameStateManager.Set<PauseGS>();
            }

            switch (GeneralFacade.GameStateManager.CurrentGameState)
            {
                case GameStates.IntroStarting:
                    GeneralFacade.GameStateManager.Set<Intro>();
                    break;

                case GameStates.Intro:
                    if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                    {
                        VideoPlayer.Stop();
                    }
                    break;

                case GameStates.StartMenu:
                case GameStates.Pause:
                case GameStates.GraphicOptions:
                    DisplayFacade.ModelAnimationManager.Animate((float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000);
                    DisplayFacade.ScreenElementManager.HandleMouse();
                    GeneralFacade.SceneManager.Update((float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000);
                    break;

                case GameStates.Playing:
                default:
                    if (GameplayFacade.ThisPlayer.IsAlive())
                    {
                        GameplayFacade.ThisPlayer.HandleInput(gameTime);
                    }
                    else
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.NumPad1))
                        {
                            GameplayFacade.ThisPlayer.Class = Player.Spec.Soldier;
                        }
                        if (Keyboard.GetState().IsKeyDown(Keys.NumPad2))
                        {
                            GameplayFacade.ThisPlayer.Class = Player.Spec.Overwatch;
                        }
                        if (Keyboard.GetState().IsKeyDown(Keys.NumPad3))
                        {
                            GameplayFacade.ThisPlayer.Class = Player.Spec.Agent; 
                        }
                    }
                    DisplayFacade.ModelAnimationManager.Animate((float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000);
                    GeneralFacade.SceneManager.Update((float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000);
                    GameplayFacade.BulletManager.Update(gameTime);

                    TestBindings(gameTime);
                    break;
            }

            DisplayFacade.DebugScreen.Update(gameTime);

            base.Update(gameTime);
        }



        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            base.Draw(gameTime);

            if (GeneralFacade.GameStateManager.CurrentGameState == GameStates.Intro)
            {
                Texture2D videoTexture = null;
                if (VideoPlayer.State != MediaState.Stopped)
                {
                    videoTexture = VideoPlayer.GetTexture();
                }

                // Draw the video, if we have a texture to draw.
                if (videoTexture != null)
                {
                    ScreenImage sImage = DisplayFacade.ScreenElementManager.GetImage("Intro");
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
                    VideoPlayer.Dispose();
                    GeneralFacade.GameStateManager.Set<StartMenuGS>();
                }
            }

            DisplayFacade.DebugScreen.Draw(gameTime);
            DisplayFacade.ScreenElementManager.DrawElements(DisplayFacade.SpriteBatch);
        }

        private void InitGameplay()
        {
            GameplayFacade.Weapons = new WeaponsList();
            GameplayFacade.BulletManager = new BulletManager();
            GameplayFacade.Objects = new ObjectsList();

            GameplayFacade.Weapons.Initialise();
            GameplayFacade.Objects.Initialize();

            GameplayFacade.ThisPlayer = new LocalPlayer();
            GameplayFacade.ThisPlayerPhysics = new Physics();
            GameplayFacade.ThisPlayerDisplay = new Display();

            
            GeneralFacade.SceneManager.SetScene<SceneDeimos>();


            GameplayFacade.ThisPlayer.Inventory = new WeaponManager();
            GameplayFacade.ThisPlayer.InitializeInventory(GameplayFacade.ThisPlayer.Class);
            GameplayFacade.ThisPlayer.PlayerSpawn(new Vector3(-60f, 20f, -8f), Vector3.Zero);
        }

        private void TestBindings(GameTime gameTime)
        {
            // Testing purposes: switching clip/noclip
            if (Keyboard.GetState().IsKeyDown(Keys.N))
            {
                CurrentPlayingState = DeimosGame.PlayingStates.NoClip;
                GameplayFacade.ThisPlayerPhysics.timer_gravity = 0;
                GameplayFacade.ThisPlayerPhysics.acceleration = Vector3.Zero;
                GameplayFacade.ThisPlayerPhysics.initial_velocity = 0;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.M))
            {
                CurrentPlayingState = DeimosGame.PlayingStates.Normal;
            }

            // for testing: player death and respawning
            if (Keyboard.GetState().IsKeyDown(Keys.J))
            {
                GameplayFacade.ThisPlayer.PlayerRespawn(new Vector3(-45f, 11f, -8f), Vector3.Zero, "main");
                GameplayFacade.ThisPlayer.InitializeInventory(GameplayFacade.ThisPlayer.Class);
                CurrentPlayingState = PlayingStates.Normal;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.K))
            {
                GameplayFacade.ThisPlayer.PlayerKill();
                GameplayFacade.ThisPlayer.Inventory.Flush();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.L))
            {
                GameplayFacade.ThisPlayer.Health--;

                if (GameplayFacade.ThisPlayer.Health == 0)
                {
                    GameplayFacade.ThisPlayer.PlayerKill();
                }
            }
        }
    }
}
