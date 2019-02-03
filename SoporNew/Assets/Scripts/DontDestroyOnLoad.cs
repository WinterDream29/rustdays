using UnityEngine;

namespace Assets.Scripts
{
    public class DontDestroyOnLoad : MonoBehaviour
    {
        void Start ()
        {
		    DontDestroyOnLoad(gameObject);
        }
    }
}