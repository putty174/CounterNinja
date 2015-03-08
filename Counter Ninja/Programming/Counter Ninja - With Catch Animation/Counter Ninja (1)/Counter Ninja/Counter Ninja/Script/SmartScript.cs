using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using CounterNinja;
using CounterNinja.Particle;
using CounterNinja.Sound;
using CounterNinja.Unit;
using CounterNinja.Subtitle;


namespace CounterNinja.Script
{
    public enum ScriptCommand
    {
        #region MusicSystem Commands
        PlaySong,
        StopSong,
        StopSongImmediately,
        SetMusicSystemVolume,
        PauseSong,
        ResumeSong,
        #endregion
        #region Player Commands
        SetPlayerPosition,
        SetPlayerFaceDirection,
        EnablePlayerControl,
        #endregion
        #region Camera Commands
        CameraTrackBody,
        CameraTrackPlayer,
        SetCameraPosition,
        #endregion
        #region SoundSystem Commands
        PlaySound,
        #endregion
        #region ScriptSystem Commands
        AddSmartScript,
        #endregion
        #region SubtitleSystem Commands
        AddSimpleSubtitle,
        #endregion
        #region Level Loading Commands
        LoadNextLevel
        #endregion
    }

    public class SmartScript
    {
        private CounterNinjaScreen screen;
        private ScriptCommand scriptCommand;
        private string scriptName;
        private object[] parameter;

        public SmartScript(CounterNinjaScreen screen, ScriptCommand scriptCommand, string scriptName, object[] parameter)
        {
            this.screen = screen;
            this.scriptCommand = scriptCommand;
            this.scriptName = scriptName;
            this.parameter = parameter;
        }

        #region Get/Set Accessor Methods

        public CounterNinjaScreen Screen
        {
            get { return screen; }
        }

        public World World
        {
            get { return screen.World; }
        }

        public ScriptSystem ScriptSystem
        {
            get { return screen.ScriptSystem; }
        }

        public ScriptCommand ScriptCommand
        {
            get { return scriptCommand; }
        }

        public string ScriptName
        {
            get { return scriptName; }
            set { scriptName = value; }
        }

        public object[] Parameter
        {
            get { return parameter; }
            set { parameter = value; }
        }

        #endregion

        /// <summary>
        /// Script's virtual update method.
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime)
        {
        }

        /// <summary>
        /// Script's Update method during a game pause.
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void UpdatePause(GameTime gameTime)
        {
        }

        /// <summary>
        /// Dispose the script.
        /// </summary>
        public void Dispose()
        {
            ScriptSystem.Dispose(scriptName);
        }

        /// <summary>
        /// Process the script command
        /// </summary>
        public void ProcessScript()
        {
            switch (scriptCommand)
            {
                #region MusicSystem Scripts
                case ScriptCommand.PlaySong:
                    PlaySong();
                    break;
                case ScriptCommand.StopSong:
                    StopSong();
                    break;
                case ScriptCommand.StopSongImmediately:
                    StopSongImmediately();
                    break;
                case ScriptCommand.PauseSong:
                    PauseSong();
                    break;
                case ScriptCommand.ResumeSong:
                    ResumeSong();
                    break;
                case ScriptCommand.SetMusicSystemVolume:
                    SetMusicSystemVolume();
                    break;
                #endregion
                #region Player Scripts
                case ScriptCommand.SetPlayerPosition:
                    SetPlayerPosition();
                    break;
                case ScriptCommand.SetPlayerFaceDirection:
                    SetPlayerFaceDirection();
                    break;
                case ScriptCommand.EnablePlayerControl:
                    EnablePlayerControl();
                    break;
                #endregion
                #region Camera Scripts
                case ScriptCommand.CameraTrackPlayer:
                    CameraTrackPlayer();
                    break;
                case ScriptCommand.CameraTrackBody:
                    CamaraTrackBody();
                    break;
                case ScriptCommand.SetCameraPosition:
                    SetCameraPosition();
                    break;
                #endregion
                #region SoundSystem Scripts
                case ScriptCommand.PlaySound:
                    PlaySound();
                    break;
                #endregion
                #region ScriptSystem Scripts
                case ScriptCommand.AddSmartScript:
                    AddSmartScript();
                    break;
                #endregion
                #region SimpleSubtitle Scripts
                case ScriptCommand.AddSimpleSubtitle:
                    AddSimpleSubtitle();
                    break;
                #endregion
                #region Level Loading Scripts
                case ScriptCommand.LoadNextLevel:
                    LoadNextLevel();
                    break;
                #endregion 
            }
        }

        #region Script Commands
        
        #region MusicSystem Scripts

        /// <summary>
        /// Plays a song with the provided song file.
        /// </summary>
        private void PlaySong()
        {
            string songFile = (string)parameter[0];
            screen.MusicSystem.PlaySong(songFile);
        }

        /// <summary>
        /// Stops all songs from the MusicSystem.
        /// </summary>
        private void StopSong()
        {
            screen.MusicSystem.Stop = true;
        }

        /// <summary>
        /// Stops song immediately without fade.
        /// </summary>
        private void StopSongImmediately()
        {
            screen.MusicSystem.StopSongImmediately();
        }

        /// <summary>
        /// Pauses a playing song.
        /// </summary>
        private void PauseSong()
        {
            screen.MusicSystem.PauseSong();
        }

        /// <summary>
        /// Resumes a paused song.
        /// </summary>
        private void ResumeSong()
        {
            screen.MusicSystem.ResumeSong();
        }

        private void SetMusicSystemVolume()
        {
            screen.MusicSystem.TargetVolume = (float)parameter[0];
        }

        #endregion

        #region Player Scripts

        /// <summary>
        /// Set the player's body location.
        /// </summary>
        private void SetPlayerPosition()
        {
            screen.Player.Body.Position = (Vector2)parameter[0];
        }
        
        private void SetPlayerFaceDirection()
        {
            screen.Player.FaceDirection = (FaceDirection)parameter[0];
        }

        private void EnablePlayerControl()
        {
            screen.Player.HasControl = (Boolean)parameter[0];
        }

        #endregion

        #region Camera Scripts

        /// <summary>
        /// Track player with camera depending on true/false parameters.
        /// </summary>
        private void CameraTrackPlayer()
        {
            screen.CameraTrackPlayer((Boolean)parameter[0]);
        }

        /// <summary>
        /// Track a body with the camera.
        /// </summary>
        private void CamaraTrackBody()
        {
            screen.CameraTrackBody((Body)parameter[0]);
        }

        /// <summary>
        /// Set the camera's position.
        /// </summary>
        private void SetCameraPosition()
        {
            screen.CameraTempTargetBody.Position = (Vector2)parameter[0];
            screen.CameraTrackBody(screen.CameraTempTargetBody);
        }

        #endregion

        #region SoundSystem Scripts

        /// <summary>
        /// Plays a soundfile.
        /// </summary>
        private void PlaySound()
        {
            screen.SoundSystem.PlaySound((string)parameter[0]);
        }

        #endregion

        #region ScriptSystem Scripts

        public void AddSmartScript()
        {
            foreach (object script in parameter)
                ScriptSystem.AddScriptQueue((SmartScript)script);
        }

        #endregion 

        #region SubtitleSystem Scripts

        public void AddSimpleSubtitle()
        {
            SimpleSubtitle subtitle = new SimpleSubtitle(screen, screen.SubtitleSystem,
                (string)parameter[0], Convert.ToDouble(parameter[1]), (int)parameter[2], (int)parameter[3]);
            screen.SubtitleSystem.AddSubtitle(subtitle);
        }

        #endregion

        #region Level Loading Scripts

        private void LoadNextLevel()
        {
            screen.ScreenManager.AddScreen((GameScreen)parameter[0]);
            screen.ExitScreen();
        }

        #endregion

        #endregion
    }
}
