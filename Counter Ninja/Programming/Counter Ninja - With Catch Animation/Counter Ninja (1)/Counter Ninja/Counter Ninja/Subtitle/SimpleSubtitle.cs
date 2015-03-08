using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Common.Decomposition;
using FarseerPhysics.Common.PolygonManipulation;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Common;
using FarseerPhysics;
using CounterNinja;

namespace CounterNinja.Subtitle
{
    public class SimpleSubtitle : BasicSubtitle
    {
        private double duration;
        private double startTime;
        private float alpha;
        private float alphaRate = 0.05f;
        private Sprite textBack;
        private Vector2 fontOrigin;
        private Vector2 position;
        private float scale;

        /// <summary>
        /// Creates a SimpleSubtitle that is able to display text on the screen.
        /// </summary>
        /// <param name="screen">Screen this belongs to.</param>
        /// <param name="subtitleSystem">SubtitleSystem this belongs to.</param>
        /// <param name="subtitle">Text to display.</param>
        /// <param name="duration">How long this subtitle stays.</param>
        /// <param name="offsetX">X coord in Pixel coord.</param>
        /// <param name="offsetY">Y coord in Pixel coord.</param>
        public SimpleSubtitle(CounterNinjaScreen screen, SubtitleSystem subtitleSystem, string subtitle,
            double duration, int offsetX, int offsetY)
            : base(screen, subtitleSystem, subtitle)
        {
            this.duration = duration;
            startTime = -1;
            alpha = 0f;
            Vector2 size = subtitleSystem.Font.MeasureString(Subtitle);
            textBack = new Sprite(screen.ScreenManager.Assets.TextureFromVertices(
                PolygonTools.CreateRectangle(ConvertUnits.ToSimUnits(size.X/2 + 20), ConvertUnits.ToSimUnits(size.Y/2)),
                MaterialType.Blank, Color.SlateGray, 1f));
            fontOrigin = SubtitleSystem.Font.MeasureString(Subtitle) / 2;
            position = new Vector2(Settings2.Default.screenWidth / 2, Settings2.Default.screenHeight / 2);
            position = Vector2.Add(position, new Vector2(offsetX, offsetY));
            scale = Settings2.Default.screenWidth / 1280f;
            if (scale > 1f)
                scale = 1f;
        } 

        /// <summary>
        /// Update the subtitle. Fade away based on time calculations from the game's time.
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            double curTime = gameTime.TotalGameTime.TotalSeconds;
            if (startTime == -1)
                startTime = curTime;
            if (curTime - startTime < duration && alpha < 1)
                alpha += alphaRate;
            else if (curTime - startTime > duration)
            {
                if (alpha > 0)
                {
                    alpha -= alphaRate;
                    position = Vector2.Add(position, new Vector2(0, -1));
                }
                else
                    Dispose();
            }
        }

        /// <summary>
        /// If game is paused, delay the subtitle according to how much time has gone by.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void UpdatePause(GameTime gameTime)
        {
            startTime += gameTime.ElapsedGameTime.TotalSeconds;
        }

        /// <summary>
        /// Draws the subtitle onto the screen.
        /// </summary>
        public override void Draw()
        {
            SpriteBatch batch = CounterNinjaScreen.ScreenManager.SpriteBatch;
            batch.DrawString(SubtitleSystem.Font, Subtitle, position, Color.White * alpha,
                0f, fontOrigin, scale, SpriteEffects.None, 0.3f);
            batch.DrawString(SubtitleSystem.Font, Subtitle, Vector2.Add(position, new Vector2(2, 2)), Color.Black * alpha,
                0f, fontOrigin, scale, SpriteEffects.None, 0.2f);
            batch.Draw(textBack.Texture, position, null, Color.White * (alpha / 2f), 0f,
                textBack.Origin, scale, SpriteEffects.None, 0.1f);
        }
    }
}
