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
        //SoundEffect mySound;
        ContentManager ContentManager;

        private Dictionary<string, SoundEffect> SoundEffect = new Dictionary<string, SoundEffect>();
        private Dictionary<string, Song> Song = new Dictionary<string, Song>();

        //Constructor
        public SoundManager(ContentManager contentManager)
        {
            ContentManager = contentManager;
        }

        //Methods
        public void LoadSoundEffect(string path)
        {
            SoundEffect currentEffect;
            currentEffect = ContentManager.Load<SoundEffect>(path);
        }

        public void PlayEffect(SoundEffect mySound)
        {
            mySound.Play();

        }

    }
}