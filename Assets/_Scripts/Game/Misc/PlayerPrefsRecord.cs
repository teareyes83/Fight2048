using Fight2048.Game.Model;
using UnityEngine;
using UnityEngine.Internal;

namespace Fight2048.Game.Misc
{
    public class PlayerPrefsRecord : IRecord
    {
        public void Save(string key, int value)
        {
            PlayerPrefs.SetInt(key, value);
            PlayerPrefs.Save();
        }

        public int Load(string key, int defaultValue)
        {
            if (!PlayerPrefs.HasKey(key))
            {
                return defaultValue;
            }

            return PlayerPrefs.GetInt(key);
        }
    }
}