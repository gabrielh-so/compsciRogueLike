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
        public bool LoadSavedGame;

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

        public enum difficultyLevel
        {
            easy,
            normal,
            hard,
            insane
        }

        public enum fontSizeLevel
        {
            small,
            medium,
            large
        }



        public static Dictionary<difficultyLevel, float> enemyDamageScalars = new Dictionary<difficultyLevel, float>()
        {
            { difficultyLevel.easy, 1.5f },
            { difficultyLevel.normal, 1.0f },
            { difficultyLevel.hard, 0.75f },
            { difficultyLevel.insane, 0.5f }
        };
        public static Dictionary<difficultyLevel, float> playerDamageScalars = new Dictionary<difficultyLevel, float>()
        {
            { difficultyLevel.easy, 0.5f },
            { difficultyLevel.normal, 1.0f },
            { difficultyLevel.hard, 1.5f },
            { difficultyLevel.insane, 2.0f }
        };


        public Dictionary<int, Dictionary<string, int>> weaponDamages;

        public Dictionary<int, Dictionary<string, int>> enemyHealth;



        public difficultyLevel difficulty;
        public fontSizeLevel fontSize;

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
            // load in values for enemy health, weapon cooldown speed and damage

            
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

            instance.enemyHealth = new Dictionary<int, Dictionary<string, int>>();
            instance.weaponDamages = new Dictionary<int, Dictionary<string, int>>();


            for (int i = 0; i < 5; i++)
            {
                Dictionary<string, int> weaponDamage = new Dictionary<string, int>();

                weaponDamage.Add("Sword", 75 * (i + 1));
                weaponDamage.Add("Spear", 150 * (i + 1));
                weaponDamage.Add("Rifle", 100 * (i + 1));
                weaponDamage.Add("Slingshot", 50 * (i + 1));

                instance.weaponDamages.Add(i, weaponDamage);
            }

            for (int i = 0; i < 5; i++)
            {
                Dictionary<string, int> enemyHealths = new Dictionary<string, int>();

                enemyHealths.Add("Goblin", 75 * (i + 1));
                enemyHealths.Add("Flyer", 50 * (i + 1));
                enemyHealths.Add("Slime", 150 * (i + 1));
                enemyHealths.Add("Boss", 1000 * (i + 1));

                instance.enemyHealth.Add(i, enemyHealths);
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
            InputManager.Instance.UpdateKeyDictionary();
        }
        public void SetDefaultVolume()
        {
            instance.SoundVolume = instance.MusicVolume = instance.MasterVolume = 0.5f;
        }
    }
}
