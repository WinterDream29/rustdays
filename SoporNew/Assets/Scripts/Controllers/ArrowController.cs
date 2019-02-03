using Assets.Scripts.Controllers.Fauna;
using System.Collections;
using Assets.Scripts.Models;
using Assets.Scripts.Models.Ammo;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class ArrowController : MonoBehaviour
    {
        public Rigidbody RigidBody;
        public float AdditionalForce = 100;
        public float SphereColliderRadius = 5;

        private bool _isShoot;
        private GameManager _gameManager;
        private bool _initializaed;

        void Awake()
        {
            RigidBody.useGravity = false;
        }

        public void Init(GameManager gameManager)
        {
            _gameManager = gameManager;
            _initializaed = true;
        }

        public void Shoot()
        {
            transform.parent = null;
            RigidBody.AddForce(transform.forward * AdditionalForce);
            RigidBody.useGravity = true;

            StartCoroutine(DelayDestory());

            _isShoot = true;
        }

        void OnTriggerEnter(Collider other)
        {
            if (!_isShoot)
                return;

            if(other.tag == "Player")
                return;

            RigidBody.velocity = Vector3.zero;
            RigidBody.angularVelocity = Vector3.zero;
            RigidBody.useGravity = false;
            var animalLink = other.gameObject.GetComponent<AnimalColliderLink>();
            if (animalLink != null)
            {
                var item = BaseObjectFactory.GetItem(typeof(Arrow));
                animalLink.SetDamage(item.Damage, transform.position);
            }
            else
            {
                Vector3 forward = transform.TransformDirection(Vector3.forward) * SphereColliderRadius;
                Debug.DrawRay(transform.position, forward, Color.green);

                var location = transform.position;
                Collider[] objectsInRange = Physics.OverlapSphere(location, SphereColliderRadius);
                foreach (Collider col in objectsInRange)
                {
                    var enemy = col.GetComponent<AnimalColliderLink>();
                    if (enemy != null)
                    {
                        var item = BaseObjectFactory.GetItem(typeof(Arrow));
                        enemy.SetDamage(item.Damage, transform.position);
                        break;
                    }
                    //if (enemy != null)
                    //{
                    //    float proximity = (location - enemy.transform.position).magnitude;
                    //    float effect = 1 - (proximity / SphereColliderRadius);
                    //}
                    //var player = col.GetComponent<PlayerController>();
                    //if(player != null)
                    //{
                    //    float proximity = (location - col.transform.position).magnitude;
                    //    float effect = 1 - (proximity / SphereColliderRadius);
                    //    player.MakeDamage(100 * effect);
                    //}
                }
            }

            StopAllCoroutines();
            Destroy(gameObject);
        }

        private IEnumerator DelayDestory()
        {
            yield return new WaitForSeconds(5f);
            Destroy(gameObject);
        }

        void FixedUpdate()
        {
            transform.LookAt(transform.position + RigidBody.velocity);
        }
    }
}
