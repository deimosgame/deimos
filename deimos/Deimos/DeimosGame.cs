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
            GeneralFacade.Game = this;

            GeneralFacade.TempContent = new ContentManager(Services);

            Graphics = new GraphicsDeviceManager(this);

            VideoPlayer = new VideoPlayer();


            Content.RootDirectory = "Content";
            GeneralFacade.TempContent.RootDirectory = "Content";

            Renderer = new DeferredRenderer(this);
            Components.Add(Renderer);


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


            InitGameplay();

            base.Initialize();
        }


        protected override void LoadContent()
        {
            IntroVideo = Content.Load<Video>("Videos/Intro");

            DisplayFacade.SpriteBatch = new SpriteBatch(GraphicsDevice);

            DisplayFacade.ScreenElementManager = new ScreenElementManager();

            DisplayFacade.DebugScreen = new DebugScreen();

            MenuManager = new MenuManager();


            //////////////////////////////////////////////////////
            // Menus
            /////////////////////////////////////////////////////
            MenuScreen StartScreen = MenuManager.Add("Start");
            StartScreen.AddElement("Play", delegate(ScreenElement el, DeimosGame game)
            {
                MenuManager.Hide();
                CurrentGameState = GameStates.Playing;
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
                DisplayFacade.ScreenElementManager.AddImage("Intro", 0, 0, 1, 1, 1, null);
            }

            switch (CurrentGameState)
            {
                case GameStates.IntroStarting:
                    if(VideoPlayer.State == MediaState.Stopped)
                    {
                        VideoPlayer.Play(IntroVideo);
                        CurrentGameState = GameStates.Intro;
                        DisplayFacade.ScreenElementManager.AddImage("Intro", 0, 0, 1, 1, 1, null);
                    }
                    break;
                case GameStates.Intro:
                    //
                    break;
                case GameStates.StartMenu:
                case GameStates.Pause:
                case GameStates.GraphicOptions:
                    IsMouseVisible = true;
                    DisplayFacade.ScreenElementManager.HandleMouse();
                    break;
                case GameStates.Playing:
                default:
                    IsMouseVisible = false;

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
                    GeneralFacade.SceneManager.Update();
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
                    CurrentGameState = GameStates.StartMenu;
                    VideoPlayer.Dispose();
                    DisplayFacade.ScreenElementManager.RemoveImage("Intro");

                    MenuManager.Set("Start");
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
            GameplayFacade.ThisPlayerPhysics = new LocalPlayerPhysics();
            GameplayFacade.ThisPlayerDisplay = new LocalPlayerDisplay();

            GeneralFacade.SceneManager = new SceneManager(GeneralFacade.TempContent);
            GeneralFacade.SceneManager.SetScene<SceneDeimos>();

            

            DisplayFacade.Camera = new Camera(
                new Vector3(0f, 9f, 20f),
                Vector3.Zero
            );

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
