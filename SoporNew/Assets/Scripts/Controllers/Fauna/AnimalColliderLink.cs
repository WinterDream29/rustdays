using UnityEngine;

namespace Assets.Scripts.Controllers.Fauna
{
    public class AnimalColliderLink : MonoBehaviour
    {
        public Collider Collider;
        public Animal Animal;
        public Enemy Enemy;

        void Start()
        {
            if(Animal != null)
                Animal.OnDeath += OnDeath;
            if(Enemy != null)
                Enemy.OnDeath += OnDeath;
        }

        private void OnDeath()
        {
            Collider.enabled = false;
        }

        public void SetDamage(int damage, Vector3? hitPosition = null)
        {
            if(Animal != null)
                Animal.SetDamage(damage, hitPosition);
            if(Enemy != null)
                Enemy.SetDamage(damage, hitPosition);
        }
    }
}
