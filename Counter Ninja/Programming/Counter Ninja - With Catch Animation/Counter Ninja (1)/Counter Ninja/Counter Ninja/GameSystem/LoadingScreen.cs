using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CounterNinja
{
    public class LoadingScreen : GameScreen
    {
        private GameScreen levelToLoad;
        private GameScreen extraScreen;
        private bool hasLoaded = false;
        private double startTime = -1;
        private double waitTime;
        private Texture2D screen;
        private float alpha = 0f;
        //private int level;
        private Vector2 origin;
        private Vector2 position;
        private float scale;

        public LoadingScreen(GameScreen levelToLoad, double waitTime)
        {
            this.levelToLoad = levelToLoad;
            this.waitTime = waitTime;
        }

        public LoadingScreen(GameScreen levelToLoad, GameScreen extraScreen, double waitTime)
            : this(levelToLoad, waitTime)
        {
            this.extraScreen = extraScreen;
        }

        public override void LoadContent()
        {
            base.LoadContent();
            screen = ScreenManager.Content.Load<Texture2D>("Menu/LoadingScreen");
            origin = new Vector2(screen.Width / 2, screen.Height / 2);
            position = new Vector2(Settings2.Default.screenWidth / 2, Settings2.Default.screenHeight / 2);
            scale = (float)Settings2.Default.screenWidth / (float)screen.Width;
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, false, false);
            if (startTime == -1)
                startTime = gameTime.TotalGameTime.TotalSeconds;
            double elapsedTime = gameTime.TotalGameTime.TotalSeconds - startTime;
            if (alpha < 1 && elapsedTime < waitTime)
                alpha += 0.02f;
            if (elapsedTime >= waitTime && !IsExiting)
            {
                if (!hasLoaded)
                {
                    if (extraScreen != null)
                        ScreenManager.AddScreen(extraScreen);
                    ScreenManager.AddScreen(levelToLoad);
                    hasLoaded = true;
                }
                if (alpha > 0)
                    alpha -= 0.02f;
                if (alpha <= 0)
                {
                    Reset();
                    ExitScreen();
                }
            }
        }

        public void Reset()
        {
            alpha = 0;
            startTime = -1;
            hasLoaded = false;
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.SpriteBatch.Begin();
            ScreenManager.SpriteBatch.Draw(screen, position, null, Color.White * alpha, 0f, origin, scale, SpriteEffects.None, 0f);
            ScreenManager.SpriteBatch.End();
        }
    }
}
