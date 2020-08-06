using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Audio;

namespace MajorProject
{
    public class AudioManager
    {

        private static AudioManager instance;

        Dictionary<string, SoundEffectInstance> SoundInstances;
        Dictionary<string, SoundEffectInstance> MusicInstances;

        public static AudioManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AudioManager();
                }

                return instance;
            }
        }

        public float SoundVolume;
        public float MusicVolume;
        public float MasterVolume;

        float prevSoundVolume;
        public float prevMusicVolume;
        public float prevMasterVolume;

        public void UpdateSoundPreferences()
        {
            SoundVolume = PlayerPreferences.Instance.SoundVolume;
            MusicVolume = PlayerPreferences.Instance.MusicVolume;
            MasterVolume = PlayerPreferences.Instance.MasterVolume;
        }

        public void Update()
        {
            string[] str = SoundInstances.Keys.ToArray<string>();
            foreach (string s in str)
            {
                if (SoundInstances[s].State == SoundState.Stopped)
                {
                    SoundInstances[s].Dispose();
                    SoundInstances.Remove(s);
                }
            }

            if (prevMasterVolume != MasterVolume)
            {
                SoundEffect.MasterVolume = MasterVolume;
            }

            if (prevMusicVolume != MusicVolume)
            {
                foreach (string s in SoundInstances.Keys)
                {
                    SoundInstances[s].Volume = SoundVolume;
                }
            }
            if (prevSoundVolume != SoundVolume)
            {
                foreach (string s in SoundInstances.Keys)
                {
                    SoundInstances[s].Volume = SoundVolume;
                }
            }

            prevSoundVolume = SoundVolume;
            prevMusicVolume = MusicVolume;
            prevMasterVolume = MasterVolume;
        }

        public AudioManager()
        {
            UpdateSoundPreferences();
            SoundInstances = new Dictionary<string, SoundEffectInstance>();
            MusicInstances = new Dictionary<string, SoundEffectInstance>();
        }

        public bool PlaySoundInstance(SoundEffectInstance soundEffectInstance, string instanceName, bool isMusic)
        {

            if (isMusic)
            {
                if (MusicInstances.ContainsKey(instanceName)) return false;
                soundEffectInstance.Volume = MusicVolume;
                soundEffectInstance.IsLooped = true;
                
                MusicInstances.Add(instanceName, soundEffectInstance);
                MusicInstances[instanceName].Play();

                return true;
            }
            if (SoundInstances.ContainsKey(instanceName)) return false;
            soundEffectInstance.Volume = SoundVolume;

            SoundInstances.Add(instanceName, soundEffectInstance);
            SoundInstances[instanceName].Play();
            return true;
        }

        public void PlaySoundInstance(SoundEffectInstance soundEffectInstance, string instanceName)
        {
            PlaySoundInstance(soundEffectInstance, instanceName, false);
        }

        
    }
}
