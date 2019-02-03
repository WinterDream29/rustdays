using Assets.Scripts.Controllers.InteractiveObjects.MiningObjects;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class AlienBallController : MonoBehaviour
    {
        public int Damage;
        public RFX4_RaycastCollision Collision;
        public RFX4_PhysicsForceCurves CollisionCurve;
        public float ColorHUE;

        //private float _lifeTime;
        //private float _speed;
        //private float _delayMove;
        //private Vector3 _startPosition;
        private GameManager _gameManager;

        void Start()
        {
        }

        public void Init(GameManager gameManager)
        {
            _gameManager = gameManager;

            ChangeColor();

            if(Collision != null)
                Collision.OnCollisionObject += OnCollisionObject;
            if(CollisionCurve != null)
                CollisionCurve.OnCollisionObject += OnCollisionObject;
        }

        private void OnCollisionObject(GameObject go)
        {
            if (_gameManager.PlayerModel.Dead)
                return;

            if (go.tag == "Player")
            {
                _gameManager.PlayerModel.Damage(Damage);
                return;
            }

            if (go.tag == "Car")
            {
                _gameManager.PlayerModel.Damage(Damage/3);
                return;
            }

            var destObj = go.GetComponent<DestroyableObject>();
            if (destObj != null)
                destObj.ChangeHealth(_gameManager, Damage);
        }

        //private IEnumerator DelayDestory(float time)
        //{
        //    yield return new WaitForSeconds(time);

        //    if (ExploseObject != null)
        //    {
        //        ExploseObject.SetActive(true);
        //        var particles = ExploseObject.GetComponentsInChildren<ParticleSystem>();
        //        foreach (var particle in particles)
        //            particle.Play();
        //        RigidBody.velocity = Vector3.zero;
        //        yield return new WaitForSeconds(0.5f);
        //    }

        //    Destroy(gameObject);
        //}

        //void OnTriggerEnter(Collider other)
        //{
        //    Debug.LogError("trigger " + other.gameObject.tag + "; " + other.gameObject.name);
        //    if (other.gameObject.tag == "Player")
        //    {
        //        var controller = other.gameObject.GetComponent<PlayerController>();

        //        if (_gameManager.PlayerModel.Dead)
        //            return;

        //        //StopAllCoroutines();
        //        _gameManager.PlayerModel.Damage(Damage);
        //        //RigidBody.velocity = Vector3.zero;
        //        //StartCoroutine(DelayDestory(0.2f));
        //    }
        //}

        //public void Go(GameManager gameManager, float lifeTime, float speed, float delayMove, Vector3 position)
        //{
        //    _gameManager = gameManager;
        //    _lifeTime = lifeTime;
        //    _speed = speed;
        //    _delayMove = delayMove;
        //    _startPosition = position;

        //    transform.position = _startPosition;
        //    gameObject.SetActive(true);

        //    StartCoroutine(StartMove());
        //}


        //private IEnumerator StartMove()
        //{
        //    yield return new WaitForSeconds(_delayMove);

        //    RigidBody.AddForce(transform.forward * _speed * 10);

        //    StartCoroutine(DelayDestory(_lifeTime));
        //}

        private void ChangeColor()
        {
            RFX4_ColorHelper.ChangeObjectColorByHUE(gameObject, ColorHUE / 360f);
            var transformMotion = gameObject.GetComponentInChildren<RFX4_TransformMotion>(true);
            if (transformMotion != null)
            {
                transformMotion.HUE = ColorHUE / 360f;
                foreach (var collidedInstance in transformMotion.CollidedInstances)
                {
                    if (collidedInstance != null)
                        RFX4_ColorHelper.ChangeObjectColorByHUE(collidedInstance, ColorHUE / 360f);
                }
            }

            var rayCastCollision = gameObject.GetComponentInChildren<RFX4_RaycastCollision>(true);
            if (rayCastCollision != null)
            {
                rayCastCollision.HUE = ColorHUE / 360f;
                foreach (var collidedInstance in rayCastCollision.CollidedInstances)
                {
                    if (collidedInstance != null)
                        RFX4_ColorHelper.ChangeObjectColorByHUE(collidedInstance, ColorHUE / 360f);
                }
            }
        }
    }
}