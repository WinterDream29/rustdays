using Assets.Scripts.Ui;
#if FACEBOOK
using Facebook.Unity;
#endif
using UnityEngine;

namespace Assets.Scripts.UI.Dialogs
{
    public class FbDialog : View
    {
        public GameObject FbLoginButton;
        public GameObject FbLogOutButton;
        public GameObject InviteFriendsButton;
        public GameObject InviteFriendsWithCustomImageButton;
        public GameObject RequestFriendsButton;
        public GameObject AddfriendsRewardObject;

        public override void Init(GameManager gameManager)
        {
            base.Init(gameManager);

            UIEventListener.Get(FbLoginButton).onClick += OnLoginClick;
            UIEventListener.Get(FbLogOutButton).onClick += OnLogOutClick;
            UIEventListener.Get(InviteFriendsButton).onClick += OnInviteFriendsClick;
            UIEventListener.Get(InviteFriendsWithCustomImageButton).onClick += OnInviteFriendsWithImageClick;
            UIEventListener.Get(RequestFriendsButton).onClick += OnRequestClick;
        }

        public override void Show()
        {
            base.Show();
#if FACEBOOK
            FbLoginButton.SetActive(!FB.IsLoggedIn);
            AddfriendsRewardObject.SetActive(PlayerPrefs.GetInt("is_add_friends", 0) == 0);
#endif
        }

        private void OnInviteFriendsClick(GameObject go)
        {
#if FACEBOOK
            FbManager.InviteFriends();
#endif
        }

        private void OnInviteFriendsWithImageClick(GameObject go)
        {
#if FACEBOOK
            FbManager.InviteFriendsWithCustomImage();
#endif
        }

        private void OnRequestClick(GameObject go)
        {
#if FACEBOOK
            if (FB.IsLoggedIn)
            {
                FbManager.InviteFriendsRequest(result =>
                {
                    if (!string.IsNullOrEmpty(result.RawResult))
                    {
                        if (PlayerPrefs.GetInt("is_add_friends", 0) == 0)
                        {
                            CurrencyManager.AddCurrency(100);
                            PlayerPrefs.SetInt("is_add_friends", 1);  
                            AddfriendsRewardObject.SetActive(false);
                        }
                    }
                });
            }
            else
            {
                FbManager.Login();
            }
#endif
        }

        private void OnLogOutClick(GameObject go)
        {
#if FACEBOOK
            FbManager.LogOut();
#endif
        }

        private void OnLoginClick(GameObject go)
        {
#if FACEBOOK
            FbManager.Login();
#endif
        }
    }
}
