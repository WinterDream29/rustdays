using UnityEngine;
using System;
#if UNITY_ANDROID && GPGS
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
#endif
#if UNITY_IOS
using UnityEngine.SocialPlatforms.GameCenter;
#endif

namespace Assets.Scripts
{
    public class GooglePlayServicesController : MonoBehaviour
    {
        public static bool IsInitialized { get; private set; }
        public static bool LoadProgress { get; private set; }
        public static byte[] SaveData { get; private set; }

        public static Action<bool> OnGameLoaded;
        public static Action<bool> OnGameSaved;

        public static Action<bool> OnLoaded;

        private static Texture2D ScreenShot;

        private static bool _initialized;

        void Start()
        {
#if UNITY_ANDROID && GPGS
            ScreenShot = new Texture2D(1024, 700);
#endif
        }

        public static void Init()
        {
#if UNITY_ANDROID && GPGS
            if(_initialized)
                return;

            //PlayGamesPlatform.DebugLogEnabled = true; 
            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().EnableSavedGames().Build();
            PlayGamesPlatform.InitializeInstance(config);
            PlayGamesPlatform.Activate();

            if (!Social.localUser.authenticated)
            {
                Social.localUser.Authenticate((bool success) =>
                {
                    if (success)
                    {
                        IsInitialized = true;
                    }
                });
            }

            _initialized = true;
#endif
        }

        public static void ShowAchievmentsUI()
        {
#if UNITY_ANDROID && GPGS
            if (!Social.localUser.authenticated)
                return;

            ((PlayGamesPlatform)Social.Active).ShowAchievementsUI();
#endif
        }

        public static void ShowLeaderBoardUI()
        {
#if UNITY_ANDROID && GPGS
            if (!Social.localUser.authenticated)
                return;

            ((PlayGamesPlatform)Social.Active).ShowLeaderboardUI();
#endif
        }

        public static void ProgressAchievement(string id, int amount = 1)
        {
#if UNITY_ANDROID && GPGS
            if (!Social.localUser.authenticated)
                return;

            PlayGamesPlatform.Instance.IncrementAchievement(id, amount, success => { });
#endif
        }

        public static void Unlock(string achievementsId)
        {
#if UNITY_ANDROID && GPGS
            if (!Social.localUser.authenticated)
                return;

            Social.ReportProgress(achievementsId, 100.0f, (bool success) =>
            {
                // handle success or failure
            });
#endif
        }
        //        public static void ResetAchievements()
        //        {
        //#if UNITY_ANDROID
        //            GameCenterPlatform.ResetAllAchievements((bool success) =>
        //            {
        //                // handle success or failure
        //            });
        //#endif
        //        }

        public static void ReportScore(string id, int score)
        {
#if UNITY_ANDROID && GPGS
            if (Social.localUser.authenticated)
            {
                var curScore = PlayerPrefs.GetInt("android_" + id, 0);
                curScore += score;
                PlayerPrefs.SetInt("android_" + id, curScore);

                Social.ReportScore(curScore, id, success => { Debug.Log("write score " + success); });
            }
#endif
        }

        public static void SaveGame(byte[] saveData)
        {
#if UNITY_ANDROID && GPGS
            if (!IsInitialized)
            {
                Debug.Log("Google Play Services Not Initialized");
                return;
            }
            SaveData = saveData;
            ShowSelectUI(false);
#endif
        }

        public static void LoadGame(Action<bool> onLoaded)
        {
#if UNITY_ANDROID && GPGS
            OnLoaded = onLoaded;

            if (!IsInitialized)
            {
                Debug.Log("Google Play Services Not Initialized");
                return;
            }
            ShowSelectUI(true, false);
#endif
        }

        public static void ShowSelectUI(bool loadGame, bool allowCreate = true)
        {
#if UNITY_ANDROID && GPGS
            LoadProgress = loadGame;

            const uint maxNumToDisplay = 5;
            bool allowCreateNew = allowCreate;

            ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
            savedGameClient.ShowSelectSavedGameUI("Select saved game", maxNumToDisplay, allowCreateNew, true, OnSavedGameSelected);
#endif
        }

#if UNITY_ANDROID && GPGS
        public static void OnSavedGameSelected(SelectUIStatus status, ISavedGameMetadata game)
        {
            if (status == SelectUIStatus.SavedGameSelected)
            {
                if (LoadProgress)
                {
                    OpenSavedGame(game.Filename);
                }
                else
                {
                    Debug.Log("!! game " + game);
                    Debug.Log("!! game name " + game.Filename);
                    if (string.IsNullOrEmpty(game.Filename))
                    {
                        var id = PlayerPrefs.GetInt(WorldConsts.CloudSaveId, 0);
                        OpenSavedGame("game" + id);
                        id += 1;
                        PlayerPrefs.SetInt(WorldConsts.CloudSaveId, id);
                    }
                    else
                        OpenSavedGame(game.Filename);
                }
            }
            else
            {
                Debug.Log("Saved game not selected");
            }
        }

        static void SaveGame(ISavedGameMetadata game, byte[] savedData, TimeSpan totalPlaytime)
        {
            ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;

            var builder = new SavedGameMetadataUpdate.Builder();
            builder = builder
                .WithUpdatedPlayedTime(totalPlaytime)
                .WithUpdatedDescription("Saved game at " + DateTime.Now);
            var savedImage = GetScreenshot();
            Debug.Log("Saved image " + savedImage);
            if (savedImage != null)
            {
                byte[] pngData = savedImage.EncodeToPNG();
                builder = builder.WithUpdatedPngCoverImage(pngData);
            }
            SavedGameMetadataUpdate updatedMetadata = builder.Build();
            savedGameClient.CommitUpdate(game, updatedMetadata, savedData, OnSavedGameWritten);
        }

        public static void OnSavedGameWritten(SavedGameRequestStatus status, ISavedGameMetadata game)
        {
            if (status == SavedGameRequestStatus.Success)
            {
                if (OnGameSaved != null)
                    OnGameSaved(true);
            }
            else
            {
                if (OnGameSaved != null)
                    OnGameSaved(false);
            }
        }

        static void OpenSavedGame(string filename)
        {
            Debug.Log("Name " + filename);
            ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
            savedGameClient.OpenWithAutomaticConflictResolution(filename, DataSource.ReadCacheOrNetwork,
                ConflictResolutionStrategy.UseLongestPlaytime, OnSavedGameOpened);
        }

        public static void OnSavedGameOpened(SavedGameRequestStatus status, ISavedGameMetadata game)
        {
            if (status == SavedGameRequestStatus.Success)
            {
                if (LoadProgress)
                    LoadGameData(game);
                else
                {
                    Debug.Log("OnSavedGameOpened SaveGame");
                    SaveGame(game, SaveData, DateTime.Now.TimeOfDay);
                }
            }
            else
            {
                Debug.Log("Saved game not opened");
            }
        }

        static void LoadGameData(ISavedGameMetadata game)
        {
            ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
            savedGameClient.ReadBinaryData(game, OnSavedGameDataRead);
        }

        public static void OnSavedGameDataRead(SavedGameRequestStatus status, byte[] data)
        {
            if (status == SavedGameRequestStatus.Success)
            {
                SaveData = data;
                if (OnGameLoaded != null)
                    OnGameLoaded(true);
                if (OnLoaded != null)
                    OnLoaded(true);
                // handle processing the byte array data
            }
            else
            {
                if (OnGameLoaded != null)
                    OnGameLoaded(false);
                if (OnLoaded != null)
                    OnLoaded(false);
                // handle error
            }
        }

        public static Texture2D GetScreenshot()
        {
            grab = true;
            return ScreenShot;
        }

        public static bool grab;

        void OnPostRender()
        {
            if (grab)
            {
                ScreenShot.ReadPixels(new Rect(0, 0, Screen.width, (Screen.width / 1024) * 700), 0, 0);
                ScreenShot.Apply();
                grab = false;
            }
        }
#endif
    }
}
