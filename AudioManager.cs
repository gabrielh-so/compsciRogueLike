﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace MajorProject
{
    public class AudioManager
    {

        private static AudioManager instance;

        ContentManager content;

        Dictionary<string, SoundEffectInstance> SoundInstances;

        //Dictionary<string, SoundEffectInstance> MusicInstances;

        SoundEffect MusicData;
        string MusicFileName = "";
        SoundEffectInstance MusicInstance;
        bool MusicSet = false;

        public static AudioManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AudioManager();
                    instance.content = new ContentManager(
                           ScreenManager.Instance.Content.ServiceProvider, "Content");
                }

                return instance;
            }
        }

        public float SoundVolume;
        public float MusicVolume;
        public float MasterVolume;

        float prevSoundVolume;
        float prevMusicVolume;
        float prevMasterVolume;

        // reads sound preferences from PlayerPreferences and sets local variables
        public void UpdateSoundPreferences()
        {
            SoundVolume = PlayerPreferences.Instance.SoundVolume;
            MusicVolume = PlayerPreferences.Instance.MusicVolume;
            MasterVolume = PlayerPreferences.Instance.MasterVolume;
        }

        // iterates over every sound effect instance and sets respective volumes
        public void UpdateVolumeValues()
        {
            SoundEffect.MasterVolume = MasterVolume;
            MusicInstance.Volume = MusicVolume;
            foreach (string s in SoundInstances.Keys)
            {
                SoundInstances[s].Volume = SoundVolume;
            }
        }

        public void Update()
        {
            //loops through soundinstance dictionary and removes/disposes ended events
            
            string[] str = SoundInstances.Keys.ToArray();

            // remove expired / stopped sound instances
            foreach (string s in str)
            {
                if (SoundInstances[s].State == SoundState.Stopped)
                {
                    // WE SHOULD DISPOSE - THE OBJECTS CREATE SOUND INSTANCES THAT ARE PASSED ONTO THIS CLASS AND ARE SUBSEQUENTLY OUT OF THEIR SCOPE
                    // THEREFORE IF THEY ARE NOT DISPOSED HERE, THE MEMORY IS NOT FREED

                    SoundInstances[s].Dispose();
                    SoundInstances.Remove(s);
                }
            }

            // checks for changes in volume values and updates
            if (prevMasterVolume != MasterVolume)
            {
                SoundEffect.MasterVolume = MasterVolume;
            }

            if (MusicSet)
                if (prevMusicVolume != MusicVolume)
                {
                    MusicInstance.Volume = MusicVolume;
                }
            if (prevSoundVolume != SoundVolume)
            {
                foreach (string s in SoundInstances.Keys)
                {
                    SoundInstances[s].Volume = SoundVolume;
                }
            }
            
            // sets old values for next update loop comparison
            prevSoundVolume = SoundVolume;
            prevMusicVolume = MusicVolume;
            prevMasterVolume = MasterVolume;
        }


        public AudioManager()
        {
            UpdateSoundPreferences();
            SoundInstances = new Dictionary<string, SoundEffectInstance>();
            //MusicInstances = new Dictionary<string, SoundEffectInstance>();
        }


        public bool StopSoundInstance(string InstanceName, bool isMusic)
        {
            if (isMusic)
            {
                MusicInstance.Stop();
                MusicInstance.Dispose();

                MusicSet = false;

                return true;
            } else
            {
                if (SoundInstances.ContainsKey(InstanceName))
                {
                    SoundInstances[InstanceName].Dispose();
                    SoundInstances.Remove(InstanceName);
                    return true;
                }
                else return false;
            }

            

            return true;
        }

        public bool PlayMusic(string FileName)
        {
            // checks that that music isn't already being played
            if (MusicFileName == FileName && MusicSet) return true;

            // unloads any playing music
            if (MusicInstance != null)
            {
                content.Unload();
            }

            if (MusicData != null) MusicData.Dispose();

            // load new music directly from file - don't need to worry about efficiency because it's only one
            MusicFileName = FileName;
            MusicData = content.Load<SoundEffect>(FileName);
            MusicInstance = MusicData.CreateInstance();
            MusicInstance.IsLooped = true;
            MusicInstance.Play();
            MusicSet = true;


            UpdateVolumeValues();

            return true;
        }


        public bool PlaySoundInstance(SoundEffectInstance soundEffectInstance, string instanceName, bool isMusic)
        {
            
            if (isMusic)
            {
                // sets the sound instance as music

                /*
                if (MusicInstances.ContainsKey(instanceName)) return false;
                soundEffectInstance.Volume = MusicVolume;
                soundEffectInstance.IsLooped = true;
                
                MusicInstances.Add(instanceName, soundEffectInstance);
                MusicInstances[instanceName].Play();
                */

                if (MusicSet)
                {
                    MusicInstance.Stop();
                    MusicInstance.Dispose();
                }
                MusicInstance = soundEffectInstance;
                MusicInstance.IsLooped = true;
                MusicInstance.Play();

                MusicSet = true;

                if (MusicData != null) MusicData.Dispose();
                MusicFileName = "";

                UpdateVolumeValues();

                return true;
            }

            // can't have duplicate names
            if (SoundInstances.ContainsKey(instanceName)) return false;
            soundEffectInstance.Volume = SoundVolume;

            SoundInstances.Add(instanceName, soundEffectInstance);
            SoundInstances[instanceName].Play();

            UpdateVolumeValues();

            return true;
        }

        // shorthand playsoundinstance override
        public void PlaySoundInstance(SoundEffectInstance soundEffectInstance, string instanceName)
        {
            PlaySoundInstance(soundEffectInstance, instanceName, false);
        }

        
    }
}
