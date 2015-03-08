using System;
using System.Collections.Generic;
using System.Linq;
using FarseerPhysics.Common;
using FarseerPhysics.Common.Decomposition;
using FarseerPhysics.Common.PolygonManipulation;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CounterNinja;

namespace CounterNinja.Background
{
    public class BackgroundOverlay
    {
        private Texture2D overlay;
        private Body bgBody;
        private Vector2 bgOrigin;
        private CounterNinjaScreen screen;
        private float layerDepth;
        private Vector2 position;

        public BackgroundOverlay(CounterNinjaScreen screen, Body bgBody, Vector2 bgOrigin, string overlayFile, float layerDepth)
        {
            this.screen = screen;
            this.bgBody = bgBody;
            this.bgOrigin = bgOrigin;
            this.layerDepth = layerDepth;
            overlay = screen.ScreenManager.Content.Load<Texture2D>(overlayFile);
        }

        public BackgroundOverlay(CounterNinjaScreen screen, Vector2 bgOrigin, string overlayFile, float layerDepth, Vector2 position)
            : this(screen, null, bgOrigin, overlayFile, layerDepth)
        {
            this.position = position;
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        /// <summary>
        /// Draws the overlay to the screen on the layer depth it is assigned.
        /// </summary>
        public void Draw()
        {
            if (bgBody != null)
                screen.ScreenManager.SpriteBatch.Draw(overlay, ConvertUnits.ToDisplayUnits(bgBody.Position),
                null, Color.Tomato, bgBody.Rotation, bgOrigin, 1f, SpriteEffects.None, layerDepth);
            else
                screen.ScreenManager.SpriteBatch.Draw(overlay, ConvertUnits.ToDisplayUnits(position),
                null, Color.White, 0f, bgOrigin, 1f, SpriteEffects.None, layerDepth);
        }
    }
}
