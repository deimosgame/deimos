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
        private SoundEffectInstance MySongEffect;
        DeimosGame Game;
        public string name;

        // Constructor
        public SoundEffectObject(ContentManager contentManager, string path)
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
        
        public void Stop()
        {
            MySongEffect.Stop();
        }
        
        public void Pause()
        {
            MySongEffect.Pause();
        }


    }
}