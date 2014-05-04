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
using System.Threading;
using System.Collections;

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
            HelperFacade.Helpers = new Helpers();

            GeneralFacade.Game = this;

            GeneralFacade.TempContent = new ContentManager(Services);

            Graphics = new GraphicsDeviceManager(this);


            VideoPlayer = new VideoPlayer();


            Content.RootDirectory = "Content";
            GeneralFacade.TempContent.RootDirectory = "Content";

            DisplayFacade.ParticleManager = new Tranquillity.ParticleManager(this);
            DisplayFacade.ParticleManager.Visible = true;
            Components.Add(DisplayFacade.ParticleManager);

            Renderer = new DeferredRenderer(this);
            Components.Add(Renderer);

            GeneralFacade.Config = new Config();
            GameplayFacade.Constants = new Constants();
        }

        protected override void Initialize()
        {
            IsMouseVisible = false;

            // Game settings
            Graphics.PreferredBackBufferWidth = 1980;
            Graphics.PreferredBackBufferHeight = 1024;
            //Graphics.PreferredBackBufferWidth = 1400;
            //Graphics.PreferredBackBufferHeight = 1050;
            //Graphics.IsFullScreen = true;
            //Graphics.PreferMultiSampling = true; // Anti aliasing - Useless as custom effects
            Graphics.SynchronizeWithVerticalRetrace = false; // VSync
            //IsFixedTimeStep = false; // Call the UPDATE method all the time instead of x time per sec
            Graphics.ApplyChanges();

            if (NetworkFacade.IsMultiplayer)
            {
                // Initialization
                NetworkFacade.Network = new NetworkManager();

                NetworkFacade.MainHandling = new MainHandler();
                NetworkFacade.NetworkHandling = new NetworkHandler();
                NetworkFacade.DataHandling = new DataHandler();

                NetworkFacade.Sending = new Queue();
                NetworkFacade.Receiving = new Queue();

                NetworkFacade.Outgoing = new Thread(NetworkFacade.Network.HandleSend);
                NetworkFacade.Incoming = new Thread(NetworkFacade.Network.HandleReceive);
                NetworkFacade.Interpret = new Thread(NetworkFacade.Network.Process);
                NetworkFacade.World = new Thread(NetworkFacade.Network.UpdateWorld);
                NetworkFacade.MovePacket = new Thread(NetworkFacade.Network.SendMovePacket);

                NetworkFacade.Players = new Dictionary<byte, Player>();

                // Thread optimization
                // Sending thread
                NetworkFacade.Outgoing.Name = "out";
                NetworkFacade.Outgoing.IsBackground = true;
                // Receiving thread
                NetworkFacade.Incoming.Name = "in";
                NetworkFacade.Incoming.IsBackground = true;
                // Interpreting thread
                NetworkFacade.Interpret.Name = "proc";
                NetworkFacade.Interpret.IsBackground = true;
                // World updating thread
                NetworkFacade.World.Name = "world";
                NetworkFacade.World.IsBackground = true;

                NetworkFacade.Outgoing.Start();
                NetworkFacade.Incoming.Start();
                NetworkFacade.Interpret.Start();
            }

            base.Initialize();
        }


        protected override void LoadContent()
        {
            IntroVideo = Content.Load<Video>("Videos/Intro");
            DisplayFacade.BackgroundMenu = GeneralFacade.Game.Content.Load<Texture2D>("Images/Menu/backgroundMenu");

            DisplayFacade.MenuImages.Add("StartMenuPlay", GeneralFacade.Game.Content.Load<Texture2D>("Images/Menu/StartMenu/play"));
            DisplayFacade.MenuImages.Add("StartMenuPlayHover", GeneralFacade.Game.Content.Load<Texture2D>("Images/Menu/StartMenu/playHover"));
            DisplayFacade.MenuImages.Add("StartMenuServers", GeneralFacade.Game.Content.Load<Texture2D>("Images/Menu/StartMenu/servers"));
            DisplayFacade.MenuImages.Add("StartMenuServersHover", GeneralFacade.Game.Content.Load<Texture2D>("Images/Menu/StartMenu/serversHover"));
            DisplayFacade.MenuImages.Add("StartMenuQuit", GeneralFacade.Game.Content.Load<Texture2D>("Images/Menu/StartMenu/quit"));
            DisplayFacade.MenuImages.Add("StartMenuQuitHover", GeneralFacade.Game.Content.Load<Texture2D>("Images/Menu/StartMenu/quitHover"));

            DisplayFacade.MenuImages.Add("PauseMenuResume", GeneralFacade.Game.Content.Load<Texture2D>("Images/Menu/PauseMenu/resume"));
            DisplayFacade.MenuImages.Add("PauseMenuResumeHover", GeneralFacade.Game.Content.Load<Texture2D>("Images/Menu/PauseMenu/resumeHover"));
            DisplayFacade.MenuImages.Add("PauseMenuMain", GeneralFacade.Game.Content.Load<Texture2D>("Images/Menu/PauseMenu/main"));
            DisplayFacade.MenuImages.Add("PauseMenuMainHover", GeneralFacade.Game.Content.Load<Texture2D>("Images/Menu/PauseMenu/mainHover"));

            DisplayFacade.DebugFont = GeneralFacade.Game.Content.Load<SpriteFont>("Fonts/debug");
            DisplayFacade.TableFont = GeneralFacade.Game.Content.Load<SpriteFont>("Fonts/table");
            DisplayFacade.TitleFont = GeneralFacade.Game.Content.Load<SpriteFont>("Fonts/title");

            DisplayFacade.SpriteBatch = new SpriteBatch(GraphicsDevice);

            GeneralFacade.GameStateManager = new GameStateManager();
            GeneralFacade.GameStateManager.Set(new IntroStartingGS());

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
                GeneralFacade.GameStateManager.Set(new PauseGS());
            }

            switch (GeneralFacade.GameStateManager.CurrentGameState)
            {
                case GameStates.IntroStarting:
                    GeneralFacade.GameStateManager.Set(new IntroGS());
                    break;

                case GameStates.Intro:
                    if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                    {
                        VideoPlayer.Stop();
                    }
                    break;

                case GameStates.StartMenu:
                case GameStates.Pause:
                case GameStates.ServerListMenu:
                case GameStates.GraphicOptions:
                    DisplayFacade.ModelAnimationManager.Animate((float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000);
                    DisplayFacade.ScreenElementManager.HandleMouse((float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000);
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
                    GeneralFacade.GameStateManager.Set(new StartMenuGS());
                }
            }

            DisplayFacade.DebugScreen.Draw(gameTime);
            DisplayFacade.ScreenElementManager.DrawElements(DisplayFacade.SpriteBatch);
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
                GameplayFacade.ThisPlayer.PlayerRespawn(new Vector3(18, 10, 90), Vector3.Zero, "main");
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
