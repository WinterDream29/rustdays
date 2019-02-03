using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Controllers.Fauna
{
    [RequireComponent(typeof(Seeker))]
    public class Enemy : AIPath
    {
        public int Health;
        public BoxCollider MiningObjectCollider;
        public string DeadSoundName;
        public GameObject BloodSplatPrefab;
        public Action OnDeath;

        public bool IsDead { get; protected set; }

        protected int StartHealth { get; set; }

        protected override void Awake()
        {
            base.Awake();

            StartHealth = Health;
        }

        protected override void Start()
        {
            base.Start();

            MiningObjectCollider.enabled = false;
        }

        public virtual void SetDamage(int damage, Vector3? hitPosition = null)
        {
            if (IsDead)
                return;

            Health -= damage;
            if (Health <= 0)
            {
                SetDeathState();
            }

            if (BloodSplatPrefab != null && damage > 5)
                StartCoroutine(BloodSplat(hitPosition));

            if(!IsDead)
                OnGetDamageWhileCalmness();
        }

        //Используется в UFPS через
        public void Damage(float damage)
        {
            SetDamage((int)damage);
        }

        void MoveToLayer(Transform root, int layer)
        {
            root.gameObject.layer = layer;
            foreach (Transform child in root)
                MoveToLayer(child, layer);
        }

        protected virtual void SetDeathState()
        {
            IsDead = true;
            canMove = false;
            MoveToLayer(transform, 0);
            MiningObjectCollider.enabled = true;

            if (OnDeath != null)
                OnDeath();
        }

        protected virtual void OnGetDamageWhileCalmness()
        {
           
        }

        public IEnumerator BloodSplat(Vector3? hitPosition)
        {
            var splat = Instantiate(BloodSplatPrefab);
            splat.transform.position = hitPosition ?? transform.position;
            yield return new WaitForSeconds(0.5f);
            Destroy(splat);
        }
    }
}