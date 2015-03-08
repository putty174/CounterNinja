using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CounterNinja
{
    /// <summary>
    /// The background screen sits behind all the other menu screens.
    /// It draws a background image that remains fixed in place regardless
    /// of whatever transitions the screens on top of it may be doing.
    /// </summary>
    public class BackgroundScreen : GameScreen
    {
        private Texture2D backgroundTexture;
        private Vector2 position;
        private Vector2 origin;
        private float scale;

        /// <summary>
        /// Constructor.
        /// </summary>
        public BackgroundScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        public override void LoadContent()
        {
            backgroundTexture = ScreenManager.Content.Load<Texture2D>("Menu/MainMenu");
            scale = (float)Settings2.Default.screenWidth / (float)backgroundTexture.Width;
            origin = new Vector2(backgroundTexture.Width / 2, backgroundTexture.Height / 2);
            position = new Vector2(Settings2.Default.screenWidth / 2, Settings2.Default.screenHeight / 2);
        }

        /// <summary>
        /// Updates the background screen. Unlike most screens, this should not
        /// transition off even if it has been covered by another screen: it is
        /// supposed to be covered, after all! This overload forces the
        /// coveredByOtherScreen parameter to false in order to stop the base
        /// Update method wanting to transition off.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                    bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);
        }

        /// <summary>
        /// Draws the background screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            ScreenManager.SpriteBatch.Begin();
            ScreenManager.SpriteBatch.Draw(backgroundTexture, position, null, Color.White, 0f, origin, scale, SpriteEffects.None, 0f);
            ScreenManager.SpriteBatch.End();
        }
    }
}