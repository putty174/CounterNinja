using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using CounterNinja;

namespace CounterNinja.Sound
{
    public class SoundSystem
    {
        private CounterNinjaScreen screen;
        private Dictionary<string, SoundEffect> soundBank;
        private List<SoundEffectInstance> soundInstance;

        /// <summary>
        /// Creates the sound system.
        /// </summary>
        /// <param name="screen">Screen this belongs to.</param>
        public SoundSystem(CounterNinjaScreen screen)
        {
            this.screen = screen;
            soundBank = new Dictionary<string, SoundEffect>();
            soundInstance = new List<SoundEffectInstance>();
        }

        /// <summary>
        /// Unloads this system.
        /// Stops all sounds from playing too.
        /// </summary>
        public void UnloadContent()
        {
            soundBank.Clear();
            foreach (SoundEffectInstance sei in soundInstance)
            {
                sei.Stop();
                sei.Dispose();
            }
            soundInstance.Clear();
        }

        /// <summary>
        /// Loads a sound if it doesn't exist yet.
        /// </summary>
        /// <param name="soundFile">Filename of sound</param>
        public void LoadSound(string soundFile)
        {
            if (!soundBank.ContainsKey(soundFile))
            {
                SoundEffect soundEffect = screen.ScreenManager.Content.Load<SoundEffect>(soundFile);
                soundBank.Add(soundFile, soundEffect);
            }
        }

        /// <summary>
        /// Removes a sound from soundfile
        /// </summary>
        /// <param name="soundFile">Name of file to remove</param>
        public void RemoveSound(string soundFile)
        {
            soundBank.Remove(soundFile);
        }

        /// <summary>
        /// Plays a sound.
        /// If it isn't loaded, load it.
        /// Creates a SoundEffectInstance from the sound bank to play.
        /// </summary>
        /// <param name="soundFile">Filename of the soundfile</param>
        public void PlaySound(string soundFile)
        {
            LoadSound(soundFile);
            SoundEffectInstance sei = soundBank[soundFile].CreateInstance();
            sei.Play();
            soundInstance.Add(sei);
            //soundBank[soundFile].Play();
        }

        /// <summary>
        /// Pause all sounds that are not stopped.
        /// </summary>
        public void PauseAll()
        {
            foreach (SoundEffectInstance sei in soundInstance)
                if (sei.State != SoundState.Stopped)
                    sei.Pause();
        }

        /// <summary>
        /// Resume all sounds that are not stopped.
        /// </summary>
        public void ResumeAll()
        {
            foreach (SoundEffectInstance sei in soundInstance)
                if (sei.State != SoundState.Stopped)
                    sei.Resume();
        }

        /// <summary>
        /// Dispose of SoundEffectInstance that are finished playing
        /// </summary>
        public void Update()
        {
            List<SoundEffectInstance> dispose = new List<SoundEffectInstance>();
            foreach (SoundEffectInstance sei in soundInstance)
                if (sei.State == SoundState.Stopped)
                    dispose.Add(sei);
            foreach (SoundEffectInstance sei in dispose)
            {
                sei.Dispose();
                soundInstance.Remove(sei);
            }
        }
    }
}
