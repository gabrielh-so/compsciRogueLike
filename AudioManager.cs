using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajorProject
{
    public class AudioManager
    {

        private static AudioManager instance;

        public static AudioManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AudioManager();
                    instance.SoundVolume = PlayerPreferences.Instance.SoundVolume;
                    instance.MusicVolume = PlayerPreferences.Instance.MusicVolume;
                    instance.MasterVolume = PlayerPreferences.Instance.MasterVolume;
                }

                return instance;
            }
        }

        public float SoundVolume;
        public float MusicVolume;
        public float MasterVolume;

        public void UpdateSoundPreferences()
        {
            instance.SoundVolume = PlayerPreferences.Instance.SoundVolume;
            instance.MusicVolume = PlayerPreferences.Instance.MusicVolume;
            instance.MasterVolume = PlayerPreferences.Instance.MasterVolume;
        }

        public AudioManager()
        {

        }



    }
}
