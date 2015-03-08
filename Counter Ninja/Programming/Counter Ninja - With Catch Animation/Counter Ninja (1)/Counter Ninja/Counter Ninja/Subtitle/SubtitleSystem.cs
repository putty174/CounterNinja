using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CounterNinja;

namespace CounterNinja.Subtitle
{
    public class SubtitleSystem
    {
        private CounterNinjaScreen screen;
        private SpriteFont font;
        private List<BasicSubtitle> subtitles;
        private List<BasicSubtitle> dispose;

        /// <summary>
        /// Creates a Subtitle System that handles all subtitle processing.
        /// </summary>
        /// <param name="screen">Screen this system belongs to.</param>
        public SubtitleSystem(CounterNinjaScreen screen)
        {
            this.screen = screen;
            font = screen.ScreenManager.Content.Load<SpriteFont>("Fonts/SubtitleFont");
            subtitles = new List<BasicSubtitle>();
            dispose = new List<BasicSubtitle>();
        }

        /// <summary>
        /// Method that handles unloading this system.
        /// </summary>
        public void UnloadContent()
        {
            subtitles.Clear();
            dispose.Clear();
        }

        #region Get/Set Accessor Methods

        /// <summary>
        /// Get the SpriteFont that this system uses.
        /// </summary>
        public SpriteFont Font
        {
            get { return font; }
        }

        #endregion

        /// <summary>
        /// Adds a BasicSubtitle to the system for handling.
        /// </summary>
        /// <param name="subtitle"></param>
        public void AddSubtitle(BasicSubtitle subtitle)
        {
            subtitles.Add(subtitle);
        }

        /// <summary>
        /// Disposes a subtitle.
        /// </summary>
        /// <param name="subtitle"></param>
        public void DisposeSubtitle(BasicSubtitle subtitle)
        {
            dispose.Add(subtitle);
        }

        /// <summary>
        /// Update method to run while game is in pause. This keeps the timing synced.
        /// </summary>
        /// <param name="gameTime"></param>
        public void UpdatePause(GameTime gameTime)
        {
            foreach (BasicSubtitle subtitle in dispose)
                subtitles.Remove(subtitle);
            dispose.Clear();
            foreach (BasicSubtitle subtitle in subtitles)
                subtitle.UpdatePause(gameTime);
        }

        /// <summary>
        /// Main method to process subtitles. First remove deleted subtitles from the system,
        /// then run each BasicSubtitle's Update method.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            foreach (BasicSubtitle subtitle in dispose)
                subtitles.Remove(subtitle);
            dispose.Clear();
            foreach (BasicSubtitle subtitle in subtitles)
                subtitle.Update(gameTime);
        }

        /// <summary>
        /// Calls on all BasicSubtitle's Draw method.
        /// </summary>
        public void Draw()
        {
            foreach (BasicSubtitle subtitle in subtitles)
                subtitle.Draw();
        }
    }
}
