using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Media;
using CounterNinja;

namespace CounterNinja.Sound
{
    public class MusicSystem
    {
        private CounterNinjaScreen screen;
        private Dictionary<string, Song> songs;
        private Song nextSong;
        private bool stop;
        private float targetVolume = 1.0f;

        /// <summary>
        /// Creates a MusicSystem for handling playing music.
        /// </summary>
        /// <param name="screen">Screen this belongs to.</param>
        public MusicSystem(CounterNinjaScreen screen)
        {
            this.screen = screen;
            songs = new Dictionary<string, Song>();
            nextSong = null;
            stop = false;
        }

        /// <summary>
        /// Unload this system.
        /// </summary>
        public void UnloadContent()
        {
            songs.Clear();
        }

        #region Get/Set Accessor Methods

        /// <summary>
        /// Set/Get bool to repeat or not repeat the song.
        /// </summary>
        public bool Repeat
        {
            get { return MediaPlayer.IsRepeating; }
            set { MediaPlayer.IsRepeating = value; }
        }

        /// <summary>
        /// Set/Get status of the system whether the song is stopped or not.
        /// </summary>
        public bool Stop
        {
            get { return stop; }
            set { stop = value; }
        }

        /// <summary>
        /// Set/Get target volume of the music system.
        /// </summary>
        public float TargetVolume
        {
            get { return targetVolume; }
            set { targetVolume = value; }
        }

        #endregion
        
        /// <summary>
        /// Method that handles song transition logic.
        /// It basically fades in/out the songs. 
        /// You don't need to understand it beyond that.
        /// </summary>
        public void CheckSongTransition()
        {
            if (stop)
            {
                if (MediaPlayer.Volume > 0f)
                    MediaPlayer.Volume -= 0.01f;
                if (MediaPlayer.Volume <= 0f && MediaPlayer.State == MediaState.Playing)
                    MediaPlayer.Stop();
            }
            else
            {
                if (MediaPlayer.Volume < targetVolume && nextSong == null)
                    MediaPlayer.Volume += 0.01f;
                if (MediaPlayer.Volume > targetVolume)
                    MediaPlayer.Volume -= 0.01f;
                if (nextSong != null)
                {
                    if (MediaPlayer.Volume > 0f)
                        MediaPlayer.Volume -= 0.01f;
                    if (MediaPlayer.Volume <= 0f)
                    {
                        MediaPlayer.Stop();
                        MediaPlayer.Play(nextSong);
                        nextSong = null;
                    }
                }
            }

        }

        /// <summary>
        /// Updates the system by calling CheckSongTransition.
        /// </summary>
        public void Update()
        {
            CheckSongTransition();
        }

        /// <summary>
        /// Adds a song to the MusicSystem. 
        /// The song file exists already, don't readd. 
        /// if it's new, load the file.
        /// </summary>
        /// <param name="songFile">Filename of song</param>
        public void AddSong(string songFile)
        {
            Song song;
            if (songs.ContainsKey(songFile))
                song = songs[songFile];
            else
            {
                song = screen.ScreenManager.Content.Load<Song>(songFile);
                songs.Add(songFile, song);
            }
        }

        /// <summary>
        /// Plays a song based on it's filename.
        /// If the song doesn't exist in this music system,
        /// it will be loaded.
        /// Handles all pause/play/fadein/fade out logic.
        /// </summary>
        /// <param name="songFile">Filename of song</param>
        public void PlaySong(string songFile)
        {
            Song song;
            if (songs.ContainsKey(songFile))
                song = songs[songFile];
            else
            {
                song = screen.ScreenManager.Content.Load<Song>(songFile);
                songs.Add(songFile, song);
            }
            if (MediaPlayer.State == MediaState.Playing)
                nextSong = song;
            else
                MediaPlayer.Play(song);
        }

        /// <summary>
        /// Stops playing the song immediately, no fade out.
        /// </summary>
        public void StopSongImmediately()
        {
            MediaPlayer.Stop();
        }

        /// <summary>
        /// Pauses the song.
        /// </summary>
        public void PauseSong()
        {
            MediaPlayer.Pause();
        }

        /// <summary>
        /// Resumes the song. This is a case where the method name explains it all.
        /// </summary>
        public void ResumeSong()
        {
            MediaPlayer.Resume();
        }
    }
}
