using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
namespace Deimos
{
    class Music
    {
        // Attributes
        private SoundEffect MySong;
        private SoundEffectInstance MySongEffect;

        // Constructor
        public Music(ContentManager contentManager, string path)
        {
            MySong = contentManager.Load<SoundEffect>(path);
            MySongEffect = MySong.CreateInstance();
        }

        // Methods
        public void Play(Vector3 emitterLocation, Vector3 cameraPosition)
        {
            AudioEmitter emitter = new AudioEmitter();
            AudioListener listener = new AudioListener();
            emitter.Position = emitterLocation;
            listener.Position = cameraPosition;
            MySongEffect.Apply3D(listener, emitter);
        }

    }
}