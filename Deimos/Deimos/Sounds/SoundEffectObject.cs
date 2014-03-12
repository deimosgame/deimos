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
        private Vector3 Emiter;
        private Vector3 Listener;
        private AudioEmitter AudioEmitter;
        private AudioListener AudioListener;
        public string name;
        private List<SoundEffectInstance> SoundInstances = new List<SoundEffectInstance>();
        private List<SoundEffectInstance> Sound3DInstances = new List<SoundEffectInstance>();

        // Constructor
        public SoundEffectObject(ContentManager contentManager, string path)
        {
            MySong = contentManager.Load<SoundEffect>(path);
            SoundEffect.DistanceScale = 50;
        }

        // Methods
        public void Play3D(Vector3 emitterLocation, Vector3 cameraPosition)
        {
            Emiter = emitterLocation;
            Listener = cameraPosition;
            AudioEmitter = new AudioEmitter();
            AudioListener = new AudioListener();
            AudioEmitter.Position = emitterLocation;
            AudioListener.Position = cameraPosition;
            SoundEffectInstance nSound = MySong.CreateInstance();
            Sound3DInstances.Add(nSound);
            nSound.Apply3D(AudioListener, AudioEmitter);
            nSound.Play();
        }

        public void Play()
        {
            SoundEffectInstance nSound = MySong.CreateInstance();
            SoundInstances.Add(nSound);
            nSound.Play();
        }

        public void SetEmiter(Vector3 pos)
        {
            Emiter = pos;
        }
        public void SetListener(Vector3 pos)
        {
            Listener = pos;
        }

        public void Update()
        {
            for (int i = 0; i < SoundInstances.Count; i++)
            {
                if (SoundInstances[i].State != SoundState.Playing)
                {
                    SoundInstances.Remove(SoundInstances[i]);
                }
            }
            for (int i = 0; i < Sound3DInstances.Count; i++)
            {
                if (Sound3DInstances[i].State != SoundState.Playing)
                {
                    Sound3DInstances[i].Dispose();
                    Sound3DInstances.Remove(Sound3DInstances[i]);
                }
                else
                {
                    AudioListener.Position = Listener;
                    AudioEmitter.Position = Emiter;
                    Sound3DInstances[i].Apply3D(AudioListener, AudioEmitter);
                }
            }
        }

    }
}