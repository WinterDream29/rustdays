using UnityEngine;
using System.Collections;

namespace Alien2
{
    public class GameManager : MonoBehaviour
    {


        public static string currentName;
        // Use this for initialization
        void Start()
        {
            GameObject current = Instantiate(Resources.Load("Alien2")) as GameObject;
            string start = "Alien2";
            SetCurrent(start);
            Debug.Log("Current model set to " + start);
        }

        public static string GetCurrent()
        {
            return currentName;
        }

        public static void SetCurrent(string newCurrentName)
        {
            currentName = newCurrentName;
        }
    }
}
