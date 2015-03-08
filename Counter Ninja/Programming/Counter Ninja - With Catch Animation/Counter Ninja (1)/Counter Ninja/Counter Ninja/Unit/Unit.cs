using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FarseerPhysics.Dynamics;
using CounterNinja;
using CounterNinja.Sound;
using CounterNinja.Particle;

namespace CounterNinja.Unit
{
    public enum TypeId
    {
        Object,
        Player,
        Enemy,
        Projectile
    }

    public class Unit
    {
        private UnitSystem unitSystem;
        private TypeId type;
        private int id;
        private HealthCounter health;

        /// <summary>
        /// Initializes a Unit. TypeId tells what type of Unit this is.
        /// Leaving both Lives and Health as -1 will make it indestructible.
        /// </summary>
        /// <param name="unitSystem">UnitSystem this Unit belongs to.</param>
        /// <param name="type">Type of Unit</param>
        /// <param name="lives">Number of lives this Unit has. Default is -1 (invincible)</param>
        /// <param name="health">Amount of health per Life. Default is -1 (invincible)</param>
        public Unit(UnitSystem unitSystem, TypeId type, int lives = -1, int maxHealth = -1)
        {
            this.unitSystem = unitSystem;
            this.type = type;
            health = new HealthCounter(lives, maxHealth);
        }

        #region Accessor methods

        public HealthCounter Health
        {
            get { return health; }
        }

        public bool IsBeingDisposed
        {
            get { return unitSystem.Dispose.Contains(id); }
        }

        public CounterNinjaScreen CounterNinjaScreen
        {
            get { return unitSystem.CounterNinjaScreen; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public TypeId TypeId
        {
            get { return type; }
            set { type = value; }
        }

        public ScreenManager ScreenManager
        {
            get { return unitSystem.ScreenManager; }
        }

        public UnitSystem UnitSystem
        {
            get { return unitSystem; }
        }

        public World World
        {
            get { return unitSystem.World; }
        }

        public SoundSystem SoundSystem
        {
            get { return CounterNinjaScreen.SoundSystem; }
        }

        public ParticleSystem ParticleSystem
        {
            get { return CounterNinjaScreen.ParticleSystem; }
        }

        #endregion 

        /// <summary>
        /// Any post processing done to this after adding 
        /// to UnitSystem should be done here.
        /// </summary>
        public virtual void AddToUnitSystem(int id)
        {
            this.id = id;
        }

        /// <summary>
        /// Any processing that needs to be done on game update 
        /// goes here
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime)
        {
        }

        /// <summary>
        /// Any processing when the game is paused goes here.
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void UpdatePause(GameTime gameTime)
        {
        }

        /// <summary>
        /// Any draw processing goes here.
        /// </summary>
        public virtual void Draw()
        {
        }

        /// <summary>
        /// Dispose code goes here.
        /// This gets called before it is removed from UnitSystem.
        /// Do not use this method to dispose.
        /// UnitSystem calls this in its DisposeUnit method.
        /// </summary>
        public virtual void Dispose()
        {
        }

        public Player ToPlayer()
        {
            return this is Player ? (Player)this : null;
        }

        public Enemy ToEnemy()
        {
            return this is Enemy ? (Enemy)this : null;
        }
    }
}
