using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class LoadingScene : MonoBehaviour
    {
        public string LoadLevelName;

        void Start ()
        {
            StartCoroutine(Load());
        }

        private IEnumerator Load()
        {
            yield return new WaitForSeconds(2.0f);
            SceneManager.LoadScene(LoadLevelName);
        }
    }
}