using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;


namespace Deimos
{

    class SoundManager
    {
        //Attributes
        ContentManager ContentManager;

        private Dictionary<string, SoundEffectObject> SoundEffect = new Dictionary<string, SoundEffectObject>();
        private Dictionary<string, SongObject> SongDic = new Dictionary<string, SongObject>();

        //Constructor
        public SoundManager(ContentManager contentManager)
        {
            ContentManager = contentManager;
        }

        //Methods

        //SoundEffectObjects
        public void AddSoundEffect(string name, string path)
        {
            SoundEffectObject sound = new SoundEffectObject(ContentManager, path);
            sound.name = name;
            SoundEffect.Add(name, sound);
        }

        public void Play3D(string name, Vector3 posReceiver, Vector3 posEmitter)
        {
            SoundEffect[name].Play3D(posEmitter, posReceiver);
        }

        public void Play(string name)
        {
            SoundEffect[name].Play();
        }


        //SongObjects

        public void AddSongObject(string name, string path)
        {
            SongObject song = new SongObject(ContentManager, path);
            song.name = name;
            SongDic.Add(name, song);
        }

        public void PauseSong(string name)
        {
            SongDic[name].Pause();
        }

        public void StopSong(string name)
        {
            SongDic[name].Stop();
        }

        public void PlaySong(string name)
        {
            SongDic[name].Play();
        }

        public void Resume(string name)
        {
            SongDic[name].Resume();
        }

        public void Mute(string name, bool state)
        {
            SongDic[name].MuteEffect(state);
        }

        public void Loop(string name, bool state)
        {
            SongDic[name].LoopEffect(state);
        }

        public void Next(string name)
        {
            SongDic[name].NextSong();
        }

        public void Previous(string name)
        {
            SongDic[name].PreviousSong();
        }
    }
}