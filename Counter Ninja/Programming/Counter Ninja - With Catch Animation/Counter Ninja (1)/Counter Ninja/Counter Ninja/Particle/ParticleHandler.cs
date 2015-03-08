using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;

namespace CounterNinja.Particle
{
    public enum ParticleRemoveType
    {
        Fade,
        Shrink,
    }

    public class ParticleHandler
    {
        private double rotation;
        private double rotationRate;
        private bool autoRotate;
        private float scale;
        private float scaleRate;
        private float scaleEnd;
        private bool autoScale;
        private double alpha;
        private double alphaRate;
        private double alphaEnd;
        private bool autoFade;
        private bool dispose;
        private ParticleRemoveType type;
        private ParticleSystem particleSystem;
        private string name;
        private Vector2 force;
        private Vector2 position;

        /// <summary>
        /// This handles how the particle moves and gets removed. It can scale, fade, rotate, and includes removal conditions.
        /// 
        /// </summary>
        /// <param name="particleSystem">Particle System this belongs to.</param>
        /// <param name="name">Name of this particle, for debug purpose or naming.</param>
        /// <param name="rotation">Initial rotation in degrees.</param>
        /// <param name="rotationRate">Rate of rotatoin</param>
        /// <param name="autoRotate">Auto rotate true/false</param>
        /// <param name="scale">Initial scale value. 1 is normal scale.</param>
        /// <param name="scaleRate">Scale rate of change</param>
        /// <param name="scaleEnd">Ending scale value</param>
        /// <param name="autoScale">Auto-scale true/false</param>
        /// <param name="alpha">Initial alpha amount. This is the transparancy. 255 is fully opaque.</param>
        /// <param name="alphaRate">Alpha change rate</param>
        /// <param name="alphaEnd">Ending alpha number</param>
        /// <param name="autoFade">Auto-fade true/false</param>
        /// <param name="position">Position on screen.</param>
        /// <param name="type">Removal condition</param>
        public ParticleHandler(ParticleSystem particleSystem, string name, 
            double rotation, double rotationRate, bool autoRotate,
            float scale, float scaleRate, float scaleEnd, bool autoScale,
            double alpha, double alphaRate, double alphaEnd, bool autoFade,
            Vector2 position, ParticleRemoveType type)
        {
            this.particleSystem = particleSystem;
            this.name = name;
            this.rotation = rotation;
            this.rotationRate = rotationRate;
            this.autoRotate = autoRotate;
            this.scale = scale;
            this.scaleRate = scaleRate;
            this.scaleEnd = scaleEnd;
            this.autoScale = autoScale;
            this.alpha = alpha;
            this.alphaRate = alphaRate;
            this.alphaEnd = alphaEnd;
            this.autoFade = autoFade;
            this.type = type;
            this.position = position;
            dispose = false;
        }

        #region Get/Set Accessor Methods

        public Vector2 Force
        {
            get { return force; }
            set { force = value; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public double Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        public double RotationRate
        {
            get { return rotationRate; }
            set { rotationRate = value; }
        }

        public bool AutoRotate
        {
            get { return autoRotate; }
            set { autoRotate = value; }
        }

        public float Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        public float ScaleRate
        {
            get { return scaleRate; }
            set { scaleRate = value; }
        }

        public float ScaleEnd
        {
            get { return scaleEnd; }
            set { scaleEnd = value; }
        }

        public bool AutoScale
        {
            get { return autoScale; }
            set { autoScale = value; }
        }

        public float Alpha
        {
            get { return (float)(alpha / 255.0); }
            set { alpha = value; }
        }

        public double AlphaRate
        {
            get { return alphaRate; }
            set { alphaRate = value; }
        }

        public double AlphaEnd
        {
            get { return alphaEnd; }
            set { alphaEnd = value; }
        }

        public bool AutoFade
        {
            get { return autoFade; }
            set { autoFade = value; }
        }

        public bool Dispose
        {
            get { return dispose; }
            set { dispose = value; }
        }
        
        #endregion

        /// <summary>
        /// Do whatever this particle has to do and check removal conditions.
        /// </summary>
        public void ProcessParticle()
        {
            CheckRemoveCondition();
            if (!dispose)
            {
                if (autoRotate)
                    rotation += rotationRate;
                if (autoScale && scale > scaleEnd)
                    scale -= scaleRate;
                if (autoFade)
                    alpha -= alphaRate;
                if (force != null)
                    position = Vector2.Add(position, force);
            }
        }
        
        /// <summary>
        /// Method that handles checking for removal conditions
        /// </summary>
        public void CheckRemoveCondition()
        {
            switch (type)
            {
                case ParticleRemoveType.Fade:
                    {
                        if (alpha <= alphaEnd)
                            dispose = true;
                        break;
                    }
                case ParticleRemoveType.Shrink:
                    {
                        if (scale <= scaleEnd)
                            dispose = true;
                        break;
                    }
                default:
                    break;
            }
            if (dispose && !particleSystem.DisposeList.Contains(name))
                particleSystem.DisposeList.Add(name);
        }
    }
}
