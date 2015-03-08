using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using CounterNinja;

namespace CounterNinja.Unit
{
    public class AnimatedSprite
    {
        private ScreenManager screen;
        private Sprite[] anim;
        private int animIndex;
        private double currentDuration;
        private double durationPerFrame;
        private bool autoLoop;

        /// <summary>
        /// Creates an AnimatedSprite that handles animating an array of Sprite.
        /// </summary>
        /// <param name="anim">Array of Sprite to animate with</param>
        /// <param name="durationPerFrame">Duration in Milliseconds for each frame to last</param>
        /// <param name="autoLoop">True/false auto-loop to start on reaching end of Sprite array, anim.</param>
        public AnimatedSprite(ScreenManager screen, Sprite[] anim, double durationPerFrame, bool autoLoop)
        {
            this.screen = screen;
            this.anim = anim;
            this.durationPerFrame = durationPerFrame;
            this.autoLoop = autoLoop;
            currentDuration = 0;
        }

        #region Accessor methods

        public int AnimationIndex
        {
            get { return animIndex; }
            set { animIndex = value; }
        }

        public bool AutoLoop
        {
            get { return autoLoop; }
            set { autoLoop = value; }
        }

        public double DurationPerFrame
        {
            get { return durationPerFrame; }
            set { durationPerFrame = value; }
        }

        #endregion

        /// <summary>
        /// This counts the duration passed so far, and if it is longer than durationPerFrame,
        /// animIndex increases.
        /// If it is autoLoop, animIndex resets to 0 on reaching the end of array anim.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            currentDuration += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (currentDuration >= durationPerFrame)
            {
                if (animIndex < anim.Length - 1)
                    animIndex++;
                else if (autoLoop)
                    animIndex = 0;
                currentDuration = 0;
            }
        }

        /// <summary>
        /// Draws the current sprite of this AnimatedSprite.
        /// </summary>
        /// <param name="position">Position in pixel on screen.</param>
        public void Draw(Vector2 position, float rotation, float scale = 1f, SpriteEffects spriteEffect = SpriteEffects.None, float layerDepth = 0.5f)
        {
            SpriteBatch batch = screen.SpriteBatch;
            batch.Draw(anim[animIndex].Texture, position, null,
                Color.White, rotation, anim[animIndex].Origin, scale, spriteEffect, layerDepth);
        }
    }
}
