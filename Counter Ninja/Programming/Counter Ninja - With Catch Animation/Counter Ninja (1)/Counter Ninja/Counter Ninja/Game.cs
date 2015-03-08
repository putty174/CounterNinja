using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;
using System.Xml.Serialization;
using System.Diagnostics;
using FarseerPhysics.GameLevel;
using CounterNinja;

namespace CounterNinja
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class CounterNinjaGame : Game
    {
        private GraphicsDeviceManager GDM;
        //private BackgroundScreen background;

        public CounterNinjaGame()
        {
            Window.Title = "Counter Ninja";
            GDM = new GraphicsDeviceManager(this);
            GDM.PreferMultiSampling = Settings2.Default.antiAlias;
            GDM.SynchronizeWithVerticalRetrace = Settings2.Default.vSync;
            GDM.PreferredBackBufferWidth = Settings2.Default.screenWidth;
            GDM.PreferredBackBufferHeight = Settings2.Default.screenHeight;
            GDM.IsFullScreen = Settings2.Default.fullScreen;
            ConvertUnits.SetDisplayUnitToSimUnitRatio(24f);
            IsFixedTimeStep = true;
            GDM.ApplyChanges();

            Content.RootDirectory = "Content";

            //new-up components and add to Game.Components
            ScreenManager = new ScreenManager(this);
            Components.Add(ScreenManager);

            //FrameRateCounter frameRateCounter = new FrameRateCounter(ScreenManager);
            //frameRateCounter.DrawOrder = 101;
            //Components.Add(frameRateCounter);
        }

        public ScreenManager ScreenManager { get; set; }

        // Allows the game to perform any initialization it needs to before starting to run.
        // This is where it can query for any required services and load any non-graphic
        // related content.  Calling base.Initialize will enumerate through any components
        // and initialize them as well.
        protected override void Initialize()
        {
            base.Initialize();

            MenuScreen menuScreen = new MenuScreen("Counter Ninja");

            MenuOptionScreen menuOption = new MenuOptionScreen("Options", this);
            LoadMenuOptionScreen(menuOption);

            MenuOptionScreen levelSelectScreen = new MenuOptionScreen("Select Level", this);
            levelSelectScreen.AddMenuItem(" ", EntryOptionType.Separator, null);
            levelSelectScreen.AddMenuItem("Sample 1", EntryOptionType.Screen, new LoadingScreen(new Sample1(), 2));
            levelSelectScreen.AddMenuItem("Sample 2", EntryOptionType.Screen, new LoadingScreen(new Sample2(), 2));
            levelSelectScreen.AddMenuItem("Sample 3", EntryOptionType.Screen, new LoadingScreen(new Sample3(), 2));
            levelSelectScreen.AddMenuItem("Sample 4", EntryOptionType.Screen, new LoadingScreen(new Sample4(), 2));
            levelSelectScreen.AddMenuItem("Return to Main Menu", EntryOptionType.ExitScreen, null);

            menuScreen.AddMenuItem(" ", EntryType.Separator, null);
            menuScreen.AddMenuItem(" ", EntryType.Separator, null);
            menuScreen.AddMenuItem("Main Menu", EntryType.Separator, null);
            menuScreen.AddMenuItem("New Game", EntryType.Screen, new LoadingScreen(new Sample2(), 2));
            menuScreen.AddMenuItem("Select Level", EntryType.Screen, levelSelectScreen);
            //menuScreen.AddMenuItem("Credits", EntryType.Screen, new LoadingScreen(new CreditScreen(), 2));
            menuScreen.AddMenuItem("Settings", EntryType.Screen, menuOption);

            menuScreen.AddMenuItem("", EntryType.Separator, null);
            menuScreen.AddMenuItem("Exit", EntryType.ExitItem, null);

            LoadingScreen loadMenuScreen = new LoadingScreen(menuScreen, new BackgroundScreen(), 0.0);

            //Play Music in the menu
            MediaPlayer.Play(ScreenManager.Content.Load<Song>("Music/CounterNinja_BGM"));

            //background = new BackgroundScreen();
            //ScreenManager.AddScreen(background);
            ScreenManager.AddScreen(loadMenuScreen);
            //ScreenManager.AddScreen(new GameStartScreen(loadMenuScreen));
            //ScreenManager.AddScreen(new LogoScreen(TimeSpan.FromSeconds(3.0)));
        }

        protected override void Update(GameTime gameTime)
        {
            //UpdateInput();
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            base.Update(gameTime);
        }

        #region Graphic options

        private void LoadMenuOptionScreen(MenuOptionScreen menuOption)
        {
            MenuOptionScreen menuResolution = new MenuOptionScreen("Change Resolution", this);
            menuResolution.AddMenuItem("Restart the game for resolution changes to take effect.", EntryOptionType.Separator, null);
            menuResolution.AddMenuItem("640x480", 640, 480);
            menuResolution.AddMenuItem("800x600", 800, 600);
            menuResolution.AddMenuItem("854x480", 854, 480);
            menuResolution.AddMenuItem("1024x768", 1024, 768);
            menuResolution.AddMenuItem("1280x600", 1280, 600);
            menuResolution.AddMenuItem("1280x720", 1280, 720);
            menuResolution.AddMenuItem("1280x768", 1280, 768);
            menuResolution.AddMenuItem("1360x768", 1360, 768);
            menuResolution.AddMenuItem("1366x768", 1366, 768);
            menuResolution.AddMenuItem("1600x900", 1600, 900);
            menuResolution.AddMenuItem("1920x1080", 1920, 1080);
            menuResolution.AddMenuItem("2560x1600", 2560, 1600);
            menuResolution.AddMenuItem("Back", EntryOptionType.ExitScreen, null);

            if (Settings2.Default.antiAlias)
                menuOption.AddMenuItem("Anti-Alias: Enabled", EntryOptionType.AntiAlias, null);
            else
                menuOption.AddMenuItem("Anti-Alias: Disabled", EntryOptionType.AntiAlias, null);
            if (Settings2.Default.fullScreen)
                menuOption.AddMenuItem("Full Screen: Enabled", EntryOptionType.FullScreen, null);
            else
                menuOption.AddMenuItem("Full Screen: Disabled", EntryOptionType.FullScreen, null);
            if (Settings2.Default.vSync)
                menuOption.AddMenuItem("VSync: Enabled", EntryOptionType.VSync, null);
            else
                menuOption.AddMenuItem("VSync: Disabled", EntryOptionType.VSync, null);
            menuOption.AddMenuItem("Change Resolution", EntryOptionType.Screen, menuResolution);
            menuOption.AddMenuItem("Back", EntryOptionType.ExitScreen, null);
        }

        public void ToggleAntiAlias()
        {
            GDM.PreferMultiSampling = !GDM.PreferMultiSampling;
            Settings2.Default.antiAlias = GDM.PreferMultiSampling;
            Settings2.Default.Save();
            GDM.ApplyChanges();
        }

        public bool IsAntiAlias()
        {
            return Settings2.Default.antiAlias;
        }

        public void ToggleFullScreen()
        {
            GDM.ToggleFullScreen();
            Settings2.Default.fullScreen = GDM.IsFullScreen;
            Settings2.Default.Save();
        }

        public bool IsFullScreen()
        {
            return Settings2.Default.fullScreen;
        }

        public void ChangeResolution(int sWidth, int sHeight)
        {
            GDM.PreferredBackBufferWidth = sWidth;
            GDM.PreferredBackBufferHeight = sHeight;
            Settings2.Default.screenWidth = sWidth;
            Settings2.Default.screenHeight = sHeight;
            Settings2.Default.Save();
            GDM.ApplyChanges();
            //background.Reload();
        }

        public int GetScreenWidth()
        {
            return Settings2.Default.screenWidth;
        }

        public int GetScreenHeight()
        {
            return Settings2.Default.screenHeight;
        }

        public void ToggleVSync()
        {
            GDM.SynchronizeWithVerticalRetrace = !GDM.SynchronizeWithVerticalRetrace;
            Settings2.Default.vSync = GDM.SynchronizeWithVerticalRetrace;
            Settings2.Default.Save();
            GDM.ApplyChanges();
        }

        public bool IsVSync()
        {
            return Settings2.Default.vSync;
        }

        #endregion
    }
}