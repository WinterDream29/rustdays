using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Tutorial
{
    public class TutorialMananger : MonoBehaviour
    {
        public GameObject PanelObject;
        public List<TutorialStep> Steps;
        public void Run(GameManager gameManager)
        {
            PanelObject.SetActive(true);

            for(int i = 0; i < Steps.Count; i++)
            {
                if(i != Steps.Count - 1)
                    Steps[i].Init(gameManager, this, Steps[i+1]);
                else
                    Steps[i].Init(gameManager, this, null);

                Steps[i].Hide();
            }

            Steps[0].Show();
        }
    }
}
