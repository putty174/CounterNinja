using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Common;
using FarseerPhysics.Collision.Shapes;
using CounterNinja.Subtitle;
using CounterNinja.Particle;
using CounterNinja.Unit;
using CounterNinja.Sound;
using CounterNinja.Script;
using CounterNinja.Background;
using CounterNinja.Hud;

namespace CounterNinja
{
    public class CounterNinjaScreen : PhysicsGameScreen
    {
        private Player player;
        private BackgroundSystem backgroundSystem;
        private ParticleSystem particleSystem;
        private ScriptSystem scriptSystem;
        private SoundSystem soundSystem;
        private MusicSystem musicSystem;
        private SubtitleSystem subtitleSystem;
        private UnitSystem unitSystem;
        private HudSystem hudSystem;
        private Body cameraTargetBody;
        private Body cameraTempTargetBody;
        private bool pastOtherScreenHasFocus = false;
        private bool debug = true;

        public CounterNinjaScreen()
            : base()
        {
        }

        public override void LoadContent()
        {
            base.LoadContent();
            backgroundSystem = new BackgroundSystem(this);
            unitSystem = new UnitSystem(this);
            particleSystem = new ParticleSystem(this);
            scriptSystem = new ScriptSystem(this);
            soundSystem = new SoundSystem(this);
            musicSystem = new MusicSystem(this);
            subtitleSystem = new SubtitleSystem(this);
            hudSystem = new HudSystem(this);

            World.Gravity = new Vector2(0, 98f);
            EnableCameraControl = false;
            player = new Player(unitSystem);
            unitSystem.AddUnit(player);
            Camera.TrackingBody = player.Body;
            Camera.EnableTracking = true;
            Camera.EnableRotationTracking = false;

            Vertices rectDoor = PolygonTools.CreateRectangle(1, 1);
            PolygonShape shape = new PolygonShape(rectDoor, 1f);
            AssetCreator creator = ScreenManager.Assets;
            Body body = BodyFactory.CreateBody(World, Vector2.Zero, null);
            body.CreateFixture(shape);
            body.Friction = 0f;
            body.BodyType = BodyType.Static;
            body.Mass = 1f;
            body.SleepingAllowed = true;
            body.Enabled = false;
            cameraTempTargetBody = body;
            cameraTargetBody = body;
        }

        #region Get/Set Accessors

        public Player Player
        {
            get { return player; }
        }

        public UnitSystem UnitSystem
        {
            get { return unitSystem; }
        }

        public HudSystem HudSystem
        {
            get { return hudSystem; }
        }

        public Body CameraTargetBody
        {
            get { return cameraTargetBody; }
        }

        public Body CameraTempTargetBody
        {
            get { return cameraTempTargetBody; }
        }

        public BackgroundSystem BackgroundSystem
        {
            get { return backgroundSystem; }
        }

        public bool Debug
        {
            get { return debug; }
            set { debug = value; }
        }

        public ParticleSystem ParticleSystem
        {
            get { return particleSystem; }
        }

        public ScriptSystem ScriptSystem
        {
            get { return scriptSystem; }
        }

        public SoundSystem SoundSystem
        {
            get { return soundSystem; }
        }

        public MusicSystem MusicSystem
        {
            get { return musicSystem; }
        }

        public SubtitleSystem SubtitleSystem
        {
            get { return subtitleSystem; }
        }

        public bool CameraTrackingPlayer
        {
            get { return cameraTargetBody == player.Body; }
        }

        public World World
        {
            get { return base.World; }
        }

        #endregion

        /// <summary>
        /// Handles the camera rotation to the player's rotation.
        /// Includes smoothly rotating the camera.
        /// </summary>
        public void HandleCameraRotation()
        {
            Body tempBody = cameraTargetBody;
            double playerDegree = 180;
            double cameraDegree = MathHelper.ToDegrees(Camera.Rotation) + 180;
            float rotate = (float)MathHelper.ToRadians((float)playerDegree - (float)cameraDegree) / 8f;
            if (MathHelper.ToDegrees(rotate) < 0.5 && MathHelper.ToDegrees(rotate) > -0.5)
                rotate *= 2f;
            Camera.RotateCamera(rotate);
            Camera.TrackingBody = tempBody;
            Camera.EnablePositionTracking = true;
        }

        /// <summary>
        /// Set the camera to track player or not depending on parameter.
        /// </summary>
        /// <param name="trackPlayer">
        /// True - Track Player.
        /// False - Stop tracking player</param>
        public void CameraTrackPlayer(bool trackPlayer)
        {
            if (trackPlayer)
            {
                EnableCameraControl = false;
                Camera.EnableTracking = true;
                Camera.EnableRotationTracking = false;
                cameraTargetBody = player.Body;
                Camera.TrackingBody = cameraTargetBody;
            }
            else
            {
                cameraTargetBody = null;
                Camera.TrackingBody = cameraTargetBody;
                EnableCameraControl = false;
            }
        }

        /// <summary>
        /// Make the camera track a physics body.
        /// </summary>
        /// <param name="body">The body to track.</param>
        public void CameraTrackBody(Body body)
        {
            EnableCameraControl = false;
            Camera.EnableTracking = true;
            cameraTargetBody = body;
            Camera.TrackingBody = cameraTargetBody;
        }

        /// <summary>
        /// Handles default inputs for the screen.
        /// Includes handling of Player HandleMovement and HandleMouse, and HandleGravityShift
        /// </summary>
        /// <param name="input">Input device</param>
        /// <param name="gameTime">Game time</param>
        public override void HandleInput(InputHelper input, GameTime gameTime)
        {
            if (debug)
                DebugHelper(input, gameTime);
            player.HandleInput(input);
            if (input.IsNewKeyPress(Keys.Escape))
            {
                MenuOptionScreen menuScreen = new MenuOptionScreen("Counter Ninja", this);

                menuScreen.AddMenuItem("", EntryOptionType.Separator, null);
                menuScreen.AddMenuItem("", EntryOptionType.Separator, null);
                menuScreen.AddMenuItem("Paused", EntryOptionType.Separator, null);
                menuScreen.AddMenuItem("Resume", EntryOptionType.ExitScreen, null);

                menuScreen.AddMenuItem("", EntryOptionType.Separator, null);
                menuScreen.AddMenuItem("Return to Main Menu", EntryOptionType.ExitToMainMenu, null);
                ScreenManager.AddScreen(menuScreen);
            }
            //ExitScreen();
        }

        /// <summary>
        /// For debugging and helping with level editing.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="gameTime"></param>
        public void DebugHelper(InputHelper input, GameTime gameTime)
        {
            if (EnableCameraControl)
                base.HandleInput(input, gameTime);
            if (EnableCameraControl)
            {
                if (input.IsNewMouseButtonPress(MouseButtons.LeftButton))
                {
                    Vector2 curMousePos = Camera.ConvertScreenToWorld(input.Cursor);
                    Console.WriteLine("(X, Y): " + (int)ConvertUnits.ToDisplayUnits(curMousePos.X)
                        + ", " + (int)ConvertUnits.ToDisplayUnits(curMousePos.Y));
                }
                if (input.IsNewKeyPress(Keys.K))
                {
                    Vector2 curMousePos = Camera.ConvertScreenToWorld(input.Cursor);
                    player.Body.Position = curMousePos;
                }
            }
            if (input.IsNewKeyPress(Keys.P))
            {
                if (!EnableCameraControl)
                {
                    EnableCameraControl = true;
                    Camera.EnableTracking = false;
                    cameraTargetBody = null;
                    Camera.TrackingBody = null;
                }
                else
                {
                    EnableCameraControl = false;
                    Camera.EnableTracking = true;
                    Camera.EnableRotationTracking = true;
                    cameraTargetBody = player.Body;
                    Camera.TrackingBody = cameraTargetBody;
                }
            }
        }

        /// <summary>
        /// Update logic of the game.
        /// If some other screen has focus, pause the sound system, pause the game, and run UpdatePause
        /// of systems that require syncing with pause time.
        /// In the initial unpausing of the game, resume the Sound System,
        /// and then continue with calling Update methods of all the other systems.
        /// Also calls Update from it's super class, PhysicsGameScreen.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="otherScreenHasFocus"></param>
        /// <param name="coveredByOtherScreen"></param>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            //ScreenManager.Game.TargetElapsedTime
            if (otherScreenHasFocus)
            {
                scriptSystem.UpdatePause(gameTime);
                subtitleSystem.UpdatePause(gameTime);
                unitSystem.Update(gameTime, true);
                hudSystem.Update(gameTime, true);
                if (!pastOtherScreenHasFocus)
                    soundSystem.PauseAll();
                pastOtherScreenHasFocus = otherScreenHasFocus;
                return;
            }
            else if (pastOtherScreenHasFocus)
                soundSystem.ResumeAll();
            pastOtherScreenHasFocus = otherScreenHasFocus;

            //if (player != null && player.Health.IsDead)
            //{
            //    base.Update(gameTime, otherScreenHasFocus, false);
            //    return;
            //}
            particleSystem.Update();
            scriptSystem.Update(gameTime);
            musicSystem.Update();
            subtitleSystem.Update(gameTime);
            soundSystem.Update();
            unitSystem.Update(gameTime, false);
            hudSystem.Update(gameTime, false);
            HandleCameraRotation();
            base.Update(gameTime, otherScreenHasFocus, false);
            if (player != null && player.Health.IsDead)
                GameOver();
        }

        /// <summary>
        /// Gameover method that is called on player death
        /// </summary>
        public virtual void GameOver()
        {
        }

        /// <summary>
        /// Unload method that is called on exiting this screen (and destroying it).
        /// Calls all unload methods for all systemsin this screen.
        /// </summary>
        public override void UnloadContent()
        {
            unitSystem.UnloadContent();
            particleSystem.UnloadContent();
            scriptSystem.UnloadContent();
            musicSystem.UnloadContent();
            subtitleSystem.UnloadContent();
            soundSystem.UnloadContent();
            hudSystem.UnloadContent();
            base.UnloadContent();
        }

        /// <summary>
        /// Draws the game, and calls all the Draw methods 
        /// of systems in this screen.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(Color.Black);
            ScreenManager.SpriteBatch.Begin(SpriteSortMode.FrontToBack, null, null, null, null, null, Camera.View);

            backgroundSystem.Draw();
            unitSystem.Draw();
            particleSystem.Draw();
            ScreenManager.SpriteBatch.End();

            ScreenManager.SpriteBatch.Begin();
            subtitleSystem.Draw();
            hudSystem.Draw();
            ScreenManager.SpriteBatch.End();
        }
    }
}
