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

        public void SetEmiter(string name, Vector3 pos)
        {
            SoundEffect[name].SetEmiter(pos);
        }
        public void SetListener(string name, Vector3 pos)
        {
                SoundEffect[name].SetListener(pos);
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


        // MISC
        public void Update()
        {
            foreach (KeyValuePair<string, SoundEffectObject> item in SoundEffect)
            {
                item.Value.Update();
            }
        }

        public byte GetSoundByte(string name)
        {
            switch (name)
            {
                case "s1":
                    return 0x00;
                case "s2":
                    return 0x01;
                case "s3":
                    return 0x02;
                case "s4":
                    return 0x03;
                case "l1":
                    return 0x04;
                case "l2":
                    return 0x05;
                case "fall":
                    return 0x06;
                case "w_sw1":
                    return 0x07;
                case "w_sw2":
                    return 0x08;
                case "w_sw3":
                    return 0x09;
                case "w_sw4":
                    return 0x0A;
                case "gun":
                    return 0x0B;
                case "rifle":
                    return 0x0C;
                case "rocket":
                    return 0x0D;
                case "noammo":
                    return 0x0E;
                case "w_c":
                    return 0x0F;
                case "w_sel1":
                    return 0x10;
                case "w_sel2":
                    return 0x11;
                case "w_sel3":
                    return 0x12;
                case "w_sel4":
                    return 0x13;
                case "w_sel5":
                    return 0x14;
                case "w_sel6":
                    return 0x15;
                case "w_pu":
                    return 0x16;
                case "speed":
                    return 0x17;
                case "gravity":
                    return 0x18;
                case "heal":
                    return 0x19;
                case "effectoff":
                    return 0x1A;
                case "explosion":
                    return 0x1B;

                default:
                    return 0x1D;
            }
        }

        public string GetSoundName(byte id)
        {
            switch (id)
            {
                case 0x00:
                    return "s1";
                case 0x01:
                    return "s2";
                case 0x02:
                    return "s3";
                case 0x03:
                    return "s4";
                case 0x04:
                    return "l1";
                case 0x05:
                    return "l2";
                case 0x06:
                    return "fall";
                case 0x07:
                    return "w_sw1";
                case 0x08:
                    return "w_sw2";
                case 0x09:
                    return "w_sw3";
                case 0x0A:
                    return "w_sw4";
                case 0x0B:
                    return "gun";
                case 0x0C:
                    return "rifle";
                case 0x0D:
                    return "rocket";
                case 0x0E:
                    return "noammo";
                case 0x0F:
                    return "w_c";
                case 0x10:
                    return "w_sel1";
                case 0x11:
                    return "w_sel2";
                case 0x12:
                    return "w_sel3";
                case 0x13:
                    return "w_sel4";
                case 0x14:
                    return "w_sel5";
                case 0x15:
                    return "w_sel6";
                case 0x16:
                    return "w_pu";
                case 0x17:
                    return "speed";
                case 0x18:
                    return "gravity";
                case 0x19:
                    return "heal";
                case 0x1A:
                    return "effectoff";
                case 0x1B:
                    return "explosion";

                default:
                    return "";
            }
        }
    }
}