using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization.Formatters.Binary;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace MajorProject
{
    [Serializable]
    public class PlayerPreferences
    {
        public Dictionary<InputManager.ActionType, Keys> ActionKeyDict;
        private static Dictionary<InputManager.ActionType, Keys> ActionKeyDictDefault = new Dictionary<InputManager.ActionType, Keys>()
        {
            { InputManager.ActionType.walk_up, Keys.W },
            { InputManager.ActionType.walk_right, Keys.D },
            { InputManager.ActionType.walk_down, Keys.S },
            { InputManager.ActionType.walk_left, Keys.A },
            { InputManager.ActionType.pick_up, Keys.F },
            { InputManager.ActionType.open_inventory, Keys.Q },
            { InputManager.ActionType.use_potion, Keys.E }
        };

        public float SoundVolume;
        public float MusicVolume;
        public float MasterVolume;

        private static PlayerPreferences instance;

        public static PlayerPreferences Instance
        {
            get
            {
                if (instance == null)
                    LoadPreferences();
                return instance;
            }
        }

        public bool UpdateKeyBinding(InputManager.ActionType action, Keys key)
        {

            if (!ActionKeyDict.ContainsValue(key))
            {
                ActionKeyDict.Remove(action);
                ActionKeyDict.Add(action, key);
                return true;
            }
            return false;
        }

        PlayerPreferences()
        {
            
        }

        static void LoadPreferences()
        {
            bool loaded = false;
            if (File.Exists("preferences.bin"))
            {
                using (FileStream fs = new FileStream("preferences.bin", FileMode.Open))
                {
                    BinaryFormatter bs = new BinaryFormatter();
                    try
                    {
                        instance = (PlayerPreferences)bs.Deserialize(fs);
                        loaded = true;
                    }
                    catch { }
                }
            }
            if (!loaded)
            {
                instance = new PlayerPreferences();
                instance.SetDefaultKeys();
                instance.SetDefaultVolume();
            }
        }

        public static void SavePreferences()
        {
            instance.SoundVolume = AudioManager.Instance.SoundVolume;
            instance.MusicVolume = AudioManager.Instance.MusicVolume;
            instance.MasterVolume = AudioManager.Instance.MasterVolume;


            using (FileStream fs = new FileStream("preferences.bin", FileMode.OpenOrCreate))
            {
                BinaryFormatter bs = new BinaryFormatter();
                bs.Serialize(fs, instance);
            }
        }

        public void SetDefaultKeys()
        {
            instance.ActionKeyDict = new Dictionary<InputManager.ActionType, Keys>(ActionKeyDictDefault);
        }
        public void SetDefaultVolume()
        {
            instance.SoundVolume = instance.MusicVolume = instance.MasterVolume = 0.5f;
        }
    }
}
