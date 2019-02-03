#if FACEBOOK
using System;
using Facebook.Unity;
using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts
{
    public static class FbManager
    {
        private static bool _initialized;

        private static Action<IResult> _addFriendsResponse;
        private static Action _loggedResponse;
        public static void Init()
        {
            if (!_initialized)
            {
                FB.Init(OnInitComplete, OnHideUnity);
                _initialized = true;
            }
        }

        public static void Login(Action logged = null)
        {
            _loggedResponse = logged;
            FB.LogInWithReadPermissions(new List<string>() { "public_profile", "email", "user_friends" }, HandleResult);
        }

        public static void LoginForPublish()
        {
            FB.LogInWithPublishPermissions(new List<string>() { "publish_actions" }, HandleResult);
        }

        public static void InviteFriends()
        {
#if UNITY_ANDROID
            FB.Mobile.AppInvite(new Uri("https://fb.me/383629962017604"), callback: HandleResult);
#endif
#if UNITY_IOS
            FB.Mobile.AppInvite(new Uri("https://itunes.apple.com/us/app/survivor-island-survival-game/id1190358180"), callback: HandleResult);
#endif
        }

        public static void InviteFriendsWithCustomImage()
        {
#if UNITY_ANDROID
            FB.Mobile.AppInvite(new Uri("https://fb.me/383629962017604"), new Uri("https://itmages.ru/image/view/5423970/e9e39bb8"), HandleResult);
#endif
#if UNITY_IOS
            FB.Mobile.AppInvite(new Uri("https://itunes.apple.com/us/app/survivor-island-survival-game/id1190358180"), new Uri("https://itmages.ru/image/view/5423970/e9e39bb8"), HandleResult);
#endif
        }

        public static void InviteFriendsRequest(Action<IResult> response = null)
        {
            _addFriendsResponse = response;
            FB.AppRequest("Best survival game", callback: HandleResult);
        }

        public static void InviteNonGameFriendsRequest()
        {
            List<object> filter = new List<object>() { "app_non_users" };

            FB.AppRequest("Best survival gam", null, filter, null, 0, string.Empty, string.Empty, HandleResult);
        }

        public static void InviteInGameFriendsRequest()
        {
            List<object> filter = new List<object>() { "app_users" };

            FB.AppRequest("Best survival gam", null, filter, null, 0, string.Empty, string.Empty, HandleResult);
        }

        public static void LogOut()
        {
            FB.LogOut();
            Debug.Log("FB IS LOGGED IN " + FB.IsLoggedIn);
        }
        private static void OnInitComplete()
        {
            Debug.Log("FB.Init completed: Is user logged in? " + FB.IsLoggedIn + " is intializad " + FB.IsInitialized);
        }
        private static void OnHideUnity(bool isUnityShown)
        {
            Debug.Log("game shown " + isUnityShown);
        }

        private static void HandleResult(IResult result)
        {
            if (result == null)
            {
                Debug.Log("Null Response");
                return;
            }

            if (!string.IsNullOrEmpty(result.Error))
            {
                Debug.Log("Error Response:" + result.Error);
            }
            else if (result.Cancelled)
            {
                Debug.Log("Cancelled Response:" + result.RawResult);
            }
            else if (!string.IsNullOrEmpty(result.RawResult))
            {
                Debug.Log("Success Response:" + result.RawResult);
                if (_loggedResponse != null)
                    _loggedResponse();
            }
            else
            {
                Debug.Log("Empty Response");
            }

            if (_addFriendsResponse != null)
                _addFriendsResponse(result);
        }
    }
}
#endif