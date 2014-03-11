using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;

namespace Deimos
{
    class SongObject
    {
        //Attributes
        public Song _Song;
        public string name;

        //Constructor 
        public SongObject(ContentManager contentManager, String path)
        {
            _Song = contentManager.Load<Song>(path);
        }

        //Methods
        public void Play()
        {
            MediaPlayer.Play(_Song);
        }

        public void Pause()
        {
            MediaPlayer.Pause();
        }

        public void Resume()
        {
            MediaPlayer.Resume();
        }

        public void Stop()
        {
            MediaPlayer.Stop();
        }

        public void LoopEffect(bool state)
        {

            MediaPlayer.IsRepeating = state;
        }

        public void MuteEffect(bool state)
        {

            MediaPlayer.IsMuted = state;
        }

        public void NextSong()
        {

            MediaPlayer.MoveNext();
        }

        public void PreviousSong()
        {

            MediaPlayer.MovePrevious();
        }

    }
}
