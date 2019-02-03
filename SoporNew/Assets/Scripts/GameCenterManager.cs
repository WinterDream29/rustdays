using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
#if UNITY_IOS
using UnityEngine.SocialPlatforms.GameCenter;
#endif

namespace Assets.Scripts
{
    public static class GameCenterManager
    {
        public const string AchRabbitLoverId = "rabbit_lover";
        public const string AchBeginnerHunterId = "beginner_hunter";
        public const string AchProfessionalHunterId = "professional_hunter";
        public const string AchRichmanId = "richman";
        public const string AchEarnerFireId = "earner_fire";

        private static readonly Dictionary<string, int> AchievementSteps = new Dictionary<string, int>()
        {
            {AchRabbitLoverId, 3},
            {AchBeginnerHunterId, 6},
            {AchProfessionalHunterId, 6},
            {AchRichmanId, 1},
            {AchEarnerFireId, 1},
        };

        public const string RatingAmountLivedDays = "amount_lived_days";

#if UNITY_IOS
        private static GameCenterPlatform _platform;
#endif
        private static bool _initialized;
        public static void Authenticate()
        {
            if (!_initialized)
            {
#if UNITY_IOS
                _platform = new GameCenterPlatform();
                _platform.localUser.Authenticate(OnAuthenticate);
                GameCenterPlatform.ShowDefaultAchievementCompletionBanner(true);
                _initialized = true;
#endif
            }
        }

        private static void OnAuthenticate(bool success)
        {
            if (success)
            {
                Social.LoadAchievements(OnAchievementsLoaded);
            }
            else
            {
                Debug.Log("cannot connect GC");
            }

            //Debug.Log("authenticate result " + result);
        }

        private static void OnAchievementsLoaded(IAchievement[] achievements)
        {
            if (achievements.Length == 0)
                Debug.Log("Error: no achievements found");
            else
                Debug.Log("Got " + achievements.Length + " achievements");
        }

        public static void ProgressAchievement(string id)
        {
#if UNITY_IOS
            if (_platform.localUser.authenticated)
            {
                var curProgress = PlayerPrefs.GetFloat(id, 0);
                if (curProgress < 99)
                {
                    curProgress += 100.0f / AchievementSteps[id];
                    if (curProgress > 99)
                        curProgress = 100.0f;

                    PlayerPrefs.SetFloat(id, curProgress);

                    Social.ReportProgress(id, curProgress, success => { Debug.Log("write achievemtn " + success); });
                }
            }
#endif
        }

        public static void ReportScore(string id, int score)
        {
#if UNITY_IOS
            if (_platform.localUser.authenticated)
            {
                var curScore = PlayerPrefs.GetInt(id, 0);
                curScore += score;
                PlayerPrefs.SetInt(id, curScore);

                Social.ReportScore(curScore, id, success => { Debug.Log("write score " + success); });
            }
#endif
        }

        public static void ShowAchievements()
        {
#if UNITY_IOS
            if (_platform.localUser.authenticated)
                Social.ShowAchievementsUI();
            else
                _platform.localUser.Authenticate(OnAuthenticate);
#endif
        }

        public static void ShowAchievements2()
        {
#if UNITY_IOS
            _platform.ShowAchievementsUI();
#endif
        }

        public static void ShowLeaderboards()
        {
#if UNITY_IOS
            if (_platform.localUser.authenticated)
                Social.ShowLeaderboardUI();
            else
                _platform.localUser.Authenticate(OnAuthenticate);
#endif
        }
        public static void ShowLeaderboards2()
        {
#if UNITY_IOS
            _platform.ShowLeaderboardUI();
#endif
        }

        public static void ResetAchievments()
        {
#if UNITY_IOS
            GameCenterPlatform.ResetAllAchievements(success => { Debug.Log("Reset achieve,emts" + success);});
#endif
        }
    }
}