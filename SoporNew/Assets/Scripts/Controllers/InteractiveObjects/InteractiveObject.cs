using Assets.Scripts.Models;
using UnityEngine;

namespace Assets.Scripts.Controllers.InteractiveObjects
{
    public class InteractiveObject : MonoBehaviour
    {
        protected GameManager GameManager;
        protected HolderObject InteractObject;

        void Start()
        {
            Init();
        }

        protected virtual void Init()
        {
        }

        public virtual void PlayerInteract(HolderObject interactObject, GameManager gameManager, Vector3? hitPosition = null)
        {
            GameManager = gameManager;
            InteractObject = interactObject;
        }

        public virtual void Destroy()
        {
            Destroy(gameObject);
        }
    }
}
