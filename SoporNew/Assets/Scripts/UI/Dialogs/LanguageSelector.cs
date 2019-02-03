using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageSelector : MonoBehaviour 
{
    public UISprite Flag;
    public GameObject RightButton;
    public GameObject LeftButton;

    private Dictionary<string, string> _langToIcon;
    private List<string> _languageKeyList;
    private string _currentLanguage;
    private int _currentLanguageId;

    private GameManager _gameManager;

    public void Init(GameManager gameManager)
    {
        _gameManager = gameManager;

        _langToIcon = new Dictionary<string, string>();
        _langToIcon["Russian"] = "flag_russia";
        _langToIcon["English"] = "flag_us_eng";
        _langToIcon["German"] = "flag_germany";
        _langToIcon["Italian"] = "flag_italy";
        _langToIcon["French"] = "flag_france";
        _langToIcon["Spanish"] = "flag_spanish";
        _langToIcon["Japanese"] = "flag_japan";
        _langToIcon["Chinesetrad"] = "flag_china";
        _langToIcon["Chinesesimple"] = "flag_china";
        _langToIcon["Korean"] = "flag_korea";
        _langToIcon["Portuguese"] = "flag_portugalia";

        _languageKeyList = new List<string>() { "English", "Russian", "Italian", "German", "French", "Spanish", "Chinesetrad", "Chinesesimple", "Japanese", "Korean", "Portuguese" };

        _currentLanguageId = _languageKeyList.IndexOf(_gameManager.CurrentLanguage);
        Flag.spriteName = _langToIcon[_gameManager.CurrentLanguage];

        UIEventListener.Get(RightButton).onClick += ChangeLanguageRight;
        UIEventListener.Get(LeftButton).onClick += ChangeLanguageLeft;
    }

    private void ChangeLanguageRight(GameObject go)
    {
        _currentLanguageId++;
        if (_currentLanguageId >= _languageKeyList.Count)
            _currentLanguageId = 0;

        _currentLanguage = _languageKeyList[_currentLanguageId];
        PlayerPrefs.SetString(WorldConsts.CurrentLanguage, _currentLanguage);
        Localization.language = _currentLanguage;
        _gameManager.CurrentLanguage = _currentLanguage;
        Flag.spriteName = _langToIcon[_currentLanguage];

        SoundManager.PlaySFX(WorldConsts.AudioConsts.ButtonClick);
    }

    private void ChangeLanguageLeft(GameObject go)
    {
        _currentLanguageId--;
        if (_currentLanguageId < 0)
            _currentLanguageId = _languageKeyList.Count - 1;

        _currentLanguage = _languageKeyList[_currentLanguageId];
        PlayerPrefs.SetString(WorldConsts.CurrentLanguage, _currentLanguage);
        Localization.language = _currentLanguage;
        _gameManager.CurrentLanguage = _currentLanguage;
        Flag.spriteName = _langToIcon[_currentLanguage];

        SoundManager.PlaySFX(WorldConsts.AudioConsts.ButtonClick);
    }
}
