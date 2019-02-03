using System.Collections;
using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine;

namespace Assets.Scripts
{
    public class ServerGameSettings : MonoBehaviour
    {
        public ObscuredString ConfigureFilePath;

        public const string ConfigurationNotLoadedString = "configuration_not_loaded";

        private ServerGameSettingsData _configurations;
        public bool ConfigurationIsLoaded { get; private set; }

        void Awake()
        {
            StartCoroutine(LoadConfigureFile());
        }

        private IEnumerator LoadConfigureFile()
        {
            //var www = new WWW(ConfigureFilePath);
            //yield return www;
            //_configurations = JsonUtility.FromJson<ServerGameSettingsData>(www.text);

            ConfigurationIsLoaded = true;
            yield return null;
        }

        public void ReloadConfigureFile()
        {
            StartCoroutine(LoadConfigureFile());
        }

        public string GetValidatePurchasePath()
        {
            if (ConfigurationIsLoaded && _configurations != null)
                return _configurations.ValidatePurchasePath;

            return ConfigurationNotLoadedString;
        }

        public string GetGameVersion()
        {
            if (ConfigurationIsLoaded && _configurations != null)
                return _configurations.GameVersion;
            return ConfigurationNotLoadedString;
        }
    }
}
