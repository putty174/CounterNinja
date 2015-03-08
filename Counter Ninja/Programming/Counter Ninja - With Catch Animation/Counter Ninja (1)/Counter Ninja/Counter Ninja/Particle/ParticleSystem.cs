using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using CounterNinja;

namespace CounterNinja.Particle
{
    public class ParticleSystem
    {
        private CounterNinjaScreen screen;
        private Dictionary<string, Sprite> sprites;
        private Dictionary<string, string> particleSpriteNames;
        private Dictionary<string, ParticleHandler> particleHandlers;
        private List<string> dispose;
        private Dictionary<string, ParticleHandler> particlesToAdd;
        private int particleCount;

        /// <summary>
        /// Creates the particle system
        /// </summary>
        /// <param name="screen">Screen this belongs to.</param>
        public ParticleSystem(CounterNinjaScreen screen)
        {
            this.screen = screen;
            sprites = new Dictionary<string, Sprite>();
            particleSpriteNames = new Dictionary<string, string>();
            particleHandlers = new Dictionary<string, ParticleHandler>();
            particlesToAdd = new Dictionary<string, ParticleHandler>();
            dispose = new List<string>();
            particleCount = 0;
        }

        #region Get/Set Accessors

        public List<string> DisposeList
        {
            get { return dispose; }
        }

        public World World
        {
            get { return screen.World; }
        }

        public ScreenManager ScreenManager
        {
            get { return screen.ScreenManager; }
        }

        public CounterNinjaScreen Screen
        {
            get { return screen; }
        }

        #endregion

        /// <summary>
        /// Unloads all particles, and basically clears this system for disposal.
        /// </summary>
        public void UnloadContent()
        {
            sprites.Clear();
            particleSpriteNames.Clear();
            particleHandlers.Clear();
            particlesToAdd.Clear();
            dispose.Clear();
        }

        /// <summary>
        /// Adds a Sprite for this system to handle.
        /// If this sprite file already exists, skip adding.
        /// </summary>
        /// <param name="textureFile">File to create sprite from.</param>
        public void AddSprite(string textureFile)
        {
            if (!sprites.ContainsKey(textureFile))
            {
                Sprite sprite = new Sprite(ScreenManager.Content.Load<Texture2D>(textureFile));
                sprites.Add(textureFile, sprite);
            }
        }

        /// <summary>
        /// Allows adding for a premade Sprite object.
        /// </summary>
        /// <param name="name">file name this sprite will take</param>
        /// <param name="sprite">Sprite object to add</param>
        public void AddPremadeSprite(string name, Sprite sprite)
        {
            if (!sprites.ContainsKey(name))
                sprites.Add(name, sprite);
        }

        /// <summary>
        /// Adds a particle to the system. If the sprite doesn't exist yet,
        /// it will be created from textureFile.
        /// </summary>
        /// <param name="textureFile">Texture file name to use</param>
        /// <param name="position">Position in Vector2 form</param>
        /// <param name="name">name of particle in system</param>
        /// <param name="ignoreGravity">bool for being affected by gravity.</param>
        /// <param name="pHandler">ParticleHandler object. This handles how the particle behaves.</param>
        public void AddParticle(string textureFile, Vector2 position, string name, bool ignoreGravity, 
            ParticleHandler pHandler)
        {
            AddSprite(textureFile);
            name += "_ParticleSystem_" + particleCount;
            particleCount++;
            //Console.WriteLine(name);
            particleSpriteNames.Add(name, textureFile);
            pHandler.Name = name;
            pHandler.Position = position;
            particlesToAdd.Add(name, pHandler);
        }

        /// <summary>
        /// Adds a particle to the system. If the sprite doesn't exist yet,
        /// it will be created from textureFile.
        /// </summary>
        /// <param name="textureFile">Texture file name to use</param>
        /// <param name="position">Position in Vector2 form</param>
        /// <param name="name">name of particle in system</param>
        /// <param name="ignoreGravity">bool for being affected by gravity.</param>
        /// <param name="pHandler">ParticleHandler object. This handles how the particle behaves.</param>
        /// <param name="initForce">Initial force the particle has</param>
        public void AddParticle(string textureFile, Vector2 position, string name, bool ignoreGravity, 
            ParticleHandler particleHandler, Vector2 initForce)
        {
            AddParticle(textureFile, position, name, ignoreGravity, particleHandler);
            particleHandler.Force = initForce;
        }

        /// <summary>
        /// Removes the particle from the system with the name.
        /// </summary>
        /// <param name="name">Name of particle to remove</param>
        public void RemoveParticle(string name)
        {
            dispose.Add(name);
        }
        
        /// <summary>
        /// Process particles that are to be removed.
        /// </summary>
        public void ProcessDispose()
        {
            foreach (string name in dispose)
            {
                particleSpriteNames.Remove(name);
                particleHandlers.Remove(name);
            }
            dispose.Clear();
        }

        public void ProcessParticlesToAdd()
        {
            foreach (KeyValuePair<string, ParticleHandler> pair in particlesToAdd)
                particleHandlers.Add(pair.Key, pair.Value);
            particlesToAdd.Clear();
        }

        /// <summary>
        /// Call ProcessDispose, and then process each particle's Particlehandler.
        /// </summary>
        public void Update()
        {
            ProcessDispose();
            ProcessParticlesToAdd();
            foreach (KeyValuePair<string, ParticleHandler> pair in particleHandlers)
                pair.Value.ProcessParticle();
        }

        /// <summary>
        /// Draw the particles by calling the Draw method of all particles
        /// </summary>
        public void Draw()
        {
            SpriteBatch batch = ScreenManager.SpriteBatch;
            foreach (KeyValuePair<string, ParticleHandler> pair in particleHandlers)
            {
                string name = pair.Key;
                ParticleHandler pHandler = pair.Value;
                Sprite bodySprite = sprites[particleSpriteNames[name]];
                batch.Draw(bodySprite.Texture, ConvertUnits.ToDisplayUnits(pHandler.Position), null, Color.White * pHandler.Alpha,
                    MathHelper.ToRadians((float)pHandler.Rotation), bodySprite.Origin, pHandler.Scale, SpriteEffects.None, 0.60f);
            }
        }
    }
}
