using Assets.Scripts.Ui;
using UnityEngine;

namespace Assets.Scripts.UI.Dialogs
{
    public class OptionsDialog : View
    {
        public GameObject NewGameButton;
        public GameObject SaveButton;
        public GameObject CloseObject;
        public GameObject MoveCarButton;
        public GameObject MoveCarConfirm;
        public GameObject MoveCarConfrimOk;
        public GameObject MoveCarConfrimCancel;
        public GameObject PlayerRespwanButton;
        public GameObject PlayerRespawnConfrimDialog;
        public GameObject PlayerRespawnConfrimYes;
        public GameObject PlayerRespawnConfrimNo;
        public UILabel TimeLabel;
        public GameObject NewGameConfirm;
        public GameObject NewGameYesButton;
        public GameObject NewGameNoButton;
        public LanguageSelector LanguageDialog;
        public FbDialog FbDialog;
        public UISlider ControlSensitivitySlider;
        public UISlider GraphicQuality;
        public UISlider WaterQuality;
        public UILabel GraphicLevelLabel;
        public UILabel WaterLevelLabel;
        public GameObject ExitGameButton;
        public GameObject ExitGameConfirmDialog;
        public GameObject ExitGameConfirm;
        public GameObject ExitGameCancel;
        public GameObject AchievementsButton;
        public GameObject LeaderboardsButton;
        public GameObject ResetAchievementsButton;
        public GameObject CloudSaveButton;
        public GameObject LoadFromCloud;

        private long _score = 0;
        private long _achi = 0;

        public override void Init(GameManager gameManager)
        {
            base.Init(gameManager);

            LanguageDialog.Init(gameManager);
            FbDialog.Init(gameManager);

            UIEventListener.Get(NewGameButton).onClick += OnNewGameButtonClick;
            UIEventListener.Get(SaveButton).onClick += OnSaveClick;
            UIEventListener.Get(CloseObject).onClick += OnCloseClick;
            UIEventListener.Get(NewGameYesButton).onClick += OnNewGameYesButtonClick;
            UIEventListener.Get(NewGameNoButton).onClick += OnNewGameNoButtonClick;
            UIEventListener.Get(MoveCarButton).onClick += OnMoveCarClick;
            UIEventListener.Get(MoveCarConfrimOk).onClick += OnMoveCarConfrimClick;
            UIEventListener.Get(MoveCarConfrimCancel).onClick += OnMoveCarCancelClick;
            UIEventListener.Get(PlayerRespwanButton).onClick += OnPlayerRespawnClick;
            UIEventListener.Get(PlayerRespawnConfrimYes).onClick += OnPlayerRespawnConfrimClick;
            UIEventListener.Get(PlayerRespawnConfrimNo).onClick += OnPlayerRespawnNoClick;
            UIEventListener.Get(ExitGameButton).onClick += OnExitButtonClick;
            UIEventListener.Get(ExitGameConfirm).onClick += OnExitGameConfirm;
            UIEventListener.Get(ExitGameCancel).onClick += OnExitGameCancel;
            UIEventListener.Get(AchievementsButton).onClick += OnAchievementsClick;
            UIEventListener.Get(LeaderboardsButton).onClick += OnLeaderboarsClick;
            UIEventListener.Get(ResetAchievementsButton).onClick += OnResetAchievmentsClick;
            UIEventListener.Get(CloudSaveButton).onClick += OnSaveCloudClick;
            UIEventListener.Get(LoadFromCloud).onClick += OnLoadFromCloudClick;

            var defalutSensitivity = 0.12f;
            ExitGameButton.SetActive(false);
#if UNITY_ANDROID
            defalutSensitivity = 0.12f;
            ExitGameButton.SetActive(true);
#endif
            var sensitivity = PlayerPrefs.GetFloat("LookPadSensitivity", defalutSensitivity);
            SetSensitivity(sensitivity, true);

            GraphicQuality.value = (float)QualityManager.CurrentQuality / 3.0f;
            SetGraphicLevelText((int) QualityManager.CurrentQuality, GraphicLevelLabel);

            WaterQuality.value = (float) QualityManager.CurrentWaterQuality / 2.0f;
            SetGraphicLevelText((int)QualityManager.CurrentWaterQuality, WaterLevelLabel);

            ControlSensitivitySlider.onDragFinished += OnSensitivity;
            GraphicQuality.onDragFinished += OnChangeGraphicQuality;
            WaterQuality.onDragFinished += OnChangeWaterQuality;

#if UNITY_ANDROID
            CloudSaveButton.SetActive(true);
            LoadFromCloud.SetActive(true);
#else
            CloudSaveButton.SetActive(false);
            LoadFromCloud.SetActive(false);
#endif
        }

        public override void Show()
        {
            base.Show();
            GameManager.Player.MainHud.SetActiveButtons(false);
            GameManager.Player.MainHud.SetActiveControls(false);

            var time = TOD_Sky.Instance.Cycle.DateTime.ToShortTimeString();
            TimeLabel.text = time.ToString();

            FbDialog.Show();
        }

        private void OnPlayerRespawnNoClick(GameObject go)
        {
            PlayerRespawnConfrimDialog.SetActive(false);
        }

        private void OnPlayerRespawnConfrimClick(GameObject go)
        {
            PlayerRespawnConfrimDialog.SetActive(false);
            GameManager.Player.transform.position = GameManager.Player.RespawnPoint.position;
            Hide();
        }

        private void OnPlayerRespawnClick(GameObject go)
        {
            PlayerRespawnConfrimDialog.SetActive(true);
        }

        private void OnMoveCarCancelClick(GameObject go)
        {
            MoveCarConfirm.SetActive(false);
        }

        private void OnMoveCarConfrimClick(GameObject go)
        {
            MoveCarConfirm.SetActive(false);
            GameManager.CarInteractive.MoveToDefaultPosition();
            Hide();
        }

        private void OnMoveCarClick(GameObject go)
        {
            MoveCarConfirm.SetActive(true);
        }

        private void OnChangeGraphicQuality()
        {
            var value = GraphicQuality.value * 3.0f;
            QualityManager.SetQuality(GameManager, (int)value);
            SetGraphicLevelText((int)value, GraphicLevelLabel);
        }

        private void OnChangeWaterQuality()
        {
            var value = WaterQuality.value * 2.0f;
            QualityManager.SetWaterQuality(GameManager, (int)value);
            SetGraphicLevelText((int)value, WaterLevelLabel);
        }

        private void SetGraphicLevelText(int value, UILabel label)
        {
            switch (value)
            {
                case 0:
                    label.text = Localization.Get("low");
                    break;
                case 1:
                    label.text = Localization.Get("medium");
                    break;
                case 2:
                    label.text = Localization.Get("high");
                    break;
                case 3:
                    label.text = Localization.Get("ultra");
                    break;
            }
        }

        private void OnSensitivity()
        {
            SetSensitivity(ControlSensitivitySlider.value);
        }

        private void SetSensitivity(float sens, bool changeSliderValue = false)
        {
            if (sens <= 0)
                sens = 0.1f;
            if (changeSliderValue)
                ControlSensitivitySlider.value = sens;

            GameManager.Player.MainHud.LookPad.RotationSensitivity = new Vector2(sens * 2.0f, sens * 1.4f);
#if UNITY_ANDROID
            GameManager.Player.MainHud.LookPad.RotationSensitivity = new Vector2(sens * 3.0f, sens * 2.0f);
#endif
            PlayerPrefs.SetFloat("LookPadSensitivity", sens);
        }

        private void OnNewGameButtonClick(GameObject go)
        {
            NewGameConfirm.SetActive(true);
            SoundManager.PlaySFX(WorldConsts.AudioConsts.ButtonClick);
        }

        private void OnNewGameYesButtonClick(GameObject go)
        {
            GameManager.StartNewGame();
            SoundManager.PlaySFX(WorldConsts.AudioConsts.ButtonClick);
        }

        private void OnNewGameNoButtonClick(GameObject go)
        {
            NewGameConfirm.SetActive(false);
            SoundManager.PlaySFX(WorldConsts.AudioConsts.ButtonClick);
        }

        private void OnSaveClick(GameObject go)
        {
            ProgressManager.SaveProgress(GameManager);
            GameManager.Player.MainHud.ShowHudText("Game Saved", HudTextColor.Green);
            Hide();
        }

        private void OnExitGameCancel(GameObject go)
        {
            ExitGameConfirmDialog.SetActive(false);
        }

        private void OnExitGameConfirm(GameObject go)
        {
            ProgressManager.SaveProgress(GameManager);
            Application.Quit();
        }

        private void OnExitButtonClick(GameObject go)
        {
            ExitGameConfirmDialog.SetActive(true);
        }


        private void OnCloseClick(GameObject go)
        {
            Hide();
        }

        private void OnAchievementsClick(GameObject go)
        {
#if UNITY_IOS
            GameCenterManager.ShowAchievements();
#endif
#if UNITY_ANDROID
            GooglePlayServicesController.ShowAchievmentsUI();
#endif
        }

        private void OnLeaderboarsClick(GameObject go)
        {
#if UNITY_IOS
            GameCenterManager.ShowLeaderboards();
#endif
#if UNITY_ANDROID
            GooglePlayServicesController.ShowLeaderBoardUI();
#endif
        }

        private void OnResetAchievmentsClick(GameObject go)
        {
#if UNITY_IOS
            GameCenterManager.ResetAchievments();
            PlayerPrefs.DeleteAll();
#endif
        }

        private void OnSaveCloudClick(GameObject go)
        {
            ProgressManager.SaveProgress(GameManager, true);
        }

        private void OnLoadFromCloudClick(GameObject go)
        {
            GooglePlayServicesController.LoadGame(OnCloudGameLoaded);
        }

        private void OnCloudGameLoaded(bool success)
        {
            if (!success)
                return;

            ProgressManager.LoadProgressFromCloud(GameManager);
        }

        public override void Hide()
        {
            GameManager.Player.MainHud.SetActiveButtons(true);
            GameManager.Player.MainHud.SetActiveControls(true);
            base.Hide();
        }
    }
}