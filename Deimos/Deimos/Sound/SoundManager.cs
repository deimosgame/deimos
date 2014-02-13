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
        private Dictionary<string, Song> Song = new Dictionary<string, Song>();

        //Constructor
        public SoundManager(ContentManager contentManager)
        {
            ContentManager = contentManager;
        }

        //Methods
        public void AddSoundEffect(string name,string path)
        {
            SoundEffectObject song = new SoundEffectObject(ContentManager, path);
            song.name = name;
            SoundEffect.Add(name, song);
        }

        public void Play(string name, Vector3 posReceiver, Vector3 posEmitter)
        {
            SoundEffect[name].Play3D(posEmitter, posReceiver);
        }

        public void Pause(string name)
        {
            SoundEffect[name].Pause();
        }

        public void Stop(string name)
        {
            SoundEffect[name].Stop();
        }

    }
}