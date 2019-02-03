using System.Collections;
using Assets.Scripts.Controllers;
using Assets.Scripts.Models;
using UnityEngine;
using Assets.Scripts.Controllers.Fauna;
using Assets.Scripts.Tutorial;
using Assets.Scripts.UI.Interactive;
using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public enum TerrainId
    {
        Terrain1,
        Terrain2
    }

    public class GameManager : MonoBehaviour
    {
        public IapStoreManager IapManager;
        public ServerGameSettings ServerGameSettings;
        public UIRoot UiRoot;
        public bool ClearProgress;
        public DisplayManager DisplayManager;
        public PlayerController Player;
        public PlacementItemsManager PlacementItemsController;
        public SpawnZonesAgregator SpawnZonesAgragator;
        public FaunaController Fauna;
        public WorldController World;
        public IntroManager Intro;
        public TutorialMananger Tutorial;
        public CarInteractive CarInteractive;
        public AstarPath Pathfinder;
        public Terrain Terrain1;
        public Terrain Terrain2;
        public float AutoSaveInterval = 60.0f;

        public Terrain CurrentTerain { get; set; }

        public Player PlayerModel { get; set; }
        public string CurrentLanguage { get; set; }

        void Awake()
        {
            DisplayManager.ShowSplash();

            QualityManager.SetQuality(this);
            SetupLanguage();
        }

        void Start () 
        {
            Application.targetFrameRate = 60;

            if (ClearProgress)
            {
                ProgressManager.DeleteProgress();
            }

            BaseObjectFactory.SetVariants();
            PlayerModel = new Player();
            PlayerModel.Init(this);
	        ProgressManager.LoadProgress(this);

            Player.Init(this);
            IapManager.Init(this);

            SpawnZonesAgragator.Init(this);
            Fauna.Init(this);

            StartCoroutine(AutoSave());

            Pathfinder.Scan();

#if FACEBOOK
            FbManager.Init();
#endif
#if UNITY_IOS
            GameCenterManager.Authenticate();
#endif
#if UNITY_ANDROID
            GooglePlayServicesController.Init();
#endif
        }

        public void StartNewGame()
        {
            ProgressManager.DeleteProgress();
            RestartGame();
        }

        public void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void SetupLanguage()
        {
            if (PlayerPrefs.HasKey(WorldConsts.CurrentLanguage))
            {
                CurrentLanguage = PlayerPrefs.GetString(WorldConsts.CurrentLanguage);
            }
            else
            {
                if (Application.systemLanguage == SystemLanguage.Russian)
                    CurrentLanguage = "Russian";
                else if (Application.systemLanguage == SystemLanguage.Italian)
                    CurrentLanguage = "Italian";
                else if (Application.systemLanguage == SystemLanguage.French)
                    CurrentLanguage = "French";
                else if (Application.systemLanguage == SystemLanguage.German)
                    CurrentLanguage = "German";
                else if (Application.systemLanguage == SystemLanguage.Spanish)
                    CurrentLanguage = "Spanish";
                else if (Application.systemLanguage == SystemLanguage.Chinese || Application.systemLanguage == SystemLanguage.ChineseTraditional)
                    CurrentLanguage = "Chinesetrad";
                else if (Application.systemLanguage == SystemLanguage.ChineseSimplified)
                    CurrentLanguage = "Chinesesimple";
                else if (Application.systemLanguage == SystemLanguage.Japanese)
                    CurrentLanguage = "Japanese";
                else if (Application.systemLanguage == SystemLanguage.Korean)
                    CurrentLanguage = "Korean";
                else if (Application.systemLanguage == SystemLanguage.Portuguese)
                    CurrentLanguage = "Portuguese";
                else
                    CurrentLanguage = "English";
                PlayerPrefs.SetString(WorldConsts.CurrentLanguage, CurrentLanguage);
                Localization.language = CurrentLanguage;
            }
        }

        void OnApplicationPause(bool pauseStatus)
        {
            if (PlayerModel != null)
            {
                if (PlayerModel.Dead)
                    return;
                if (PlayerModel.Energy < 5.0f)
                    return;
            }
  
            if (pauseStatus)
                ProgressManager.SaveProgress(this);
        }

        private IEnumerator AutoSave()
        {
            while(true)
            {
                yield return new WaitForSeconds(AutoSaveInterval);
                ProgressManager.SaveProgress(this);
            }
        }

        void OnDestroy()
        {
            StopAllCoroutines();
        }
    }
}
