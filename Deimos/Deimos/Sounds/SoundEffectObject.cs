using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
namespace Deimos
{
    class SoundEffectObject
    {
        // Attributes
        private SoundEffect MySong;
        DeimosGame Game;
        public string name;

        // Constructor
        public SoundEffectObject(ContentManager contentManager, string path)
        {
            MySong = contentManager.Load<SoundEffect>(path);
        }

        // Methods
        public void Play3D(Vector3 emitterLocation, Vector3 cameraPosition)
        {
            AudioEmitter emitter = new AudioEmitter();
            AudioListener listener = new AudioListener();
            emitter.Position = emitterLocation;
            listener.Position = cameraPosition;
            MySong.CreateInstance().Apply3D(listener, emitter);
        }

        public void Play()
        {
            MySong.CreateInstance().Play();
        }

    }
}