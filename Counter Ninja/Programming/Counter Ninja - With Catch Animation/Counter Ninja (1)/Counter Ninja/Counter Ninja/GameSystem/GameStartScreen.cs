using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace CounterNinja
{
    public class GameStartScreen : GameScreen
    {
        private GameScreen levelToLoad;
        private Texture2D[] logo;
        private int logoIndex = 0;
        private double startTime = -1;
        private double[] duration;
        private float alpha = 0f;
        private Vector2[] origin;
        private Vector2[] position;
        private Song song;
        private float scale;

        public GameStartScreen(GameScreen levelToLoad)
        {
            this.levelToLoad = levelToLoad;
        }

        public override void LoadContent()
        {
            base.LoadContent();
            logo = new Texture2D[3];
            logo[0] = ScreenManager.Content.Load<Texture2D>("Menu/FarseerLogo");
            logo[1] = ScreenManager.Content.Load<Texture2D>("Menu/TempScreen");
            logo[2] = ScreenManager.Content.Load<Texture2D>("Menu/TempScreen");
            duration = new double[] { 1.5, 1.5, 1.0 };
            origin = new Vector2[3];
            origin[0] = new Vector2(logo[0].Width / 2, logo[0].Height / 2);
            origin[1] = new Vector2(logo[1].Width / 2, logo[1].Height / 2);
            origin[2] = new Vector2(logo[2].Width / 2, logo[2].Height / 2);
            position = new Vector2[3];
            position[0] = new Vector2(Settings2.Default.screenWidth / 2, Settings2.Default.screenHeight / 2);
            position[1] = new Vector2(Settings2.Default.screenWidth / 2, Settings2.Default.screenHeight / 2);
            position[2] = new Vector2(Settings2.Default.screenWidth / 2, Settings2.Default.screenHeight / 2);
            scale = (float)Settings2.Default.screenWidth / (float)logo[0].Width;
            //videoPlayer = new VideoPlayer();
            //song = ScreenManager.Content.Load<Song>("Music/Zel_Wip__Collab_1_");
            //MediaPlayer.Play(song);
            //MediaPlayer.IsRepeating = false;
            //MediaPlayer.Volume = 0.8f;
            //videoPlayer.Play(ScreenManager.Content.Load<Video>("Menu/GravityShiftIntro"));
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
            if (IsExiting)
                return;
            if (startTime == -1)
                startTime = gameTime.TotalGameTime.TotalSeconds;
            double elapsedTime = gameTime.TotalGameTime.TotalSeconds - startTime;
            if (alpha < 1f && elapsedTime < duration[logoIndex])
                alpha += 0.02f;
            if (alpha > 0f && elapsedTime > duration[logoIndex])
                alpha -= 0.02f;
            if (alpha <= 0f && elapsedTime > duration[logoIndex])
            {
                startTime = gameTime.TotalGameTime.TotalSeconds;
                if (logoIndex < logo.Length)
                    logoIndex++;
                if (logoIndex >= logo.Length)
                {
                    //ScreenManager.AddScreen(new BackgroundScreen());
                    ScreenManager.AddScreen(levelToLoad);
                    ExitScreen();
                }
            }
            //if (!IsExiting && videoPlayer.State == MediaState.Stopped)
            //{
            //    videoPlayer.Dispose();
            //    ScreenManager.AddScreen(levelToLoad);
            //    MediaPlayer.Play(song);
            //    MediaPlayer.IsRepeating = false;
            //    ExitScreen();
            //}
        }

        public override void Draw(GameTime gameTime)
        {
            //base.Draw(gameTime);
            //ScreenManager.GraphicsDevice.Clear(Color.Black);

            ScreenManager.SpriteBatch.Begin();
            if (logoIndex < logo.Length)
                ScreenManager.SpriteBatch.Draw(logo[logoIndex], position[logoIndex], null, Color.White * alpha, 0f, origin[logoIndex], scale, SpriteEffects.None, 0f);
            //ScreenManager.SpriteBatch.Draw(videoPlayer.GetTexture(), Vector2.Zero, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            ScreenManager.SpriteBatch.End();
        }
    }
}
