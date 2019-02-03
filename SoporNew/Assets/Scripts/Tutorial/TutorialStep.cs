using System;
using UnityEngine;

namespace Assets.Scripts.Tutorial
{
    public class TutorialStep : MonoBehaviour
    {
        public GameObject CloseCollider;

        private GameManager _gameManager;
        private TutorialMananger _tutorialManager;
        private TutorialStep _nextStep;
        public void Init(GameManager gameManager, TutorialMananger tutorialManager, TutorialStep nextStep)
        {
            _gameManager = gameManager;
            _tutorialManager = tutorialManager;
            _nextStep = nextStep;

            UIEventListener.Get(CloseCollider).onClick += OnCloseStepClick;
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void OnCloseStepClick(GameObject go)
        {
            if (_nextStep != null)
                _nextStep.Show();
            else
                _tutorialManager.PanelObject.SetActive(false);

            Hide();
        }
    }
}
