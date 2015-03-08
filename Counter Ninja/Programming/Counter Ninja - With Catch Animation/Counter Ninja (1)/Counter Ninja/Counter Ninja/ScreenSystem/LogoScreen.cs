using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CounterNinja
{
    public class LogoScreen : GameScreen
    {
        private const float LogoScreenHeightRatio = 4f / 6f;
        private const float LogoWidthHeightRatio = 874f / 353f;

        private ContentManager content;
        private Rectangle destination;
        private TimeSpan duration;
        private Texture2D farseerLogoTexture;

        public LogoScreen(TimeSpan duration)
        {
            this.duration = duration;
            TransitionOffTime = TimeSpan.FromSeconds(2.0);
        }

        // Loads graphics content for this screen. The background texture is quite
        // big, so we use our own local ContentManager to load it. This allows us
        // to unload before going from the menus into the game itself, wheras if we
        // used the shared ContentManager provided by the Game class, the content
        // would remain loaded forever.
        public override void LoadContent()
        {
            if (content == null)
            {
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            }

            farseerLogoTexture = content.Load<Texture2D>("Common/logo");

            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            int rectHeight = (int)(viewport.Height * LogoScreenHeightRatio);
            int rectWidth = (int)(rectHeight * LogoWidthHeightRatio);
            int posX = viewport.Bounds.Center.X - rectWidth / 2;
            int posY = viewport.Bounds.Center.Y - rectHeight / 2;

            destination = new Rectangle(posX, posY, rectWidth, rectHeight);
        }

        // <summary>
        // Unloads graphics content for this screen.
        // </summary>
        public override void UnloadContent()
        {
            content.Unload();
        }

        public override void HandleInput(InputHelper input, GameTime gameTime)
        {
            if (input.KeyboardState.GetPressedKeys().Length > 0 ||
                input.GamePadState.IsButtonDown(Buttons.A | Buttons.Start | Buttons.Back) ||
                input.MouseState.LeftButton == ButtonState.Pressed)
            {
                duration = TimeSpan.Zero;
            }
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                    bool coveredByOtherScreen)
        {
            duration -= gameTime.ElapsedGameTime;
            if (duration <= TimeSpan.Zero)
            {
                ExitScreen();
            }

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(Color.White);

            ScreenManager.SpriteBatch.Begin();
            ScreenManager.SpriteBatch.Draw(farseerLogoTexture, destination, Color.White);
            ScreenManager.SpriteBatch.End();
        }
    }
}