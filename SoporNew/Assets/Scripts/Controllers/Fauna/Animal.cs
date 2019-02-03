using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Controllers.Fauna
{
    public enum AnimalType
    {
        Deer,
        Bear,
        Boar,
        Rabbit,
        Chicken,
        Fox,
        Sheep,
        Wolf
    }

    public enum AnimalStates
    {
        Idle,
        Idle2,
        Walk,
        Run,
        GetHit,
        Attack,
        Death,

        Idle4Legs,
        Idle2Legs,
        To4Legs,
        To2Legs,
        Attack4Legs,
        Attack4LegsClawL,
        Attack4LegsClawR,
        Attack2Legs,
        Attack2LegsClawL,
        Attack2LegsClawR,
        Roar2Legs,
        Roar4Legs,
        Death4Legs,
        Death2Legs
    }

    [RequireComponent(typeof(Seeker))]
    public class Animal : AIPath
    {
        public AnimalType AnimalType;
        public float AnimalHeight;
        public Animator Animator;
        public BoxCollider MiningObjectCollider;
        public int Health;
        public float WalkSpeed;
        public float RunSpeed;
        public float FearPlayerDistance;
        public float StopFearDistance;
        public float AttackPlayerDistance;
        public GameObject BloodSplatPrefab;
        public bool FearPlayer = true;
        public string DeadSoundName;
        public string AttackSoundName;

        public bool IsDead { get; private set; }
        public Action OnDeath;

        protected GameManager GameManager;
        protected bool _playerNear;
        protected AnimalStates CurrentState;
        private bool _canAttackDistance;

        protected Dictionary<AnimalStates, string> AnimatorTriggers;
        private FaunaZone _zone;
        private Transform _targetObject;
        private Quaternion _toRotation;
        private float _rotationTime;
        private bool _runAwayFromPlayer;
        private bool _initializaed;

        protected override void Awake()
        {
            base.Awake();

            AnimatorTriggers = new Dictionary<AnimalStates, string>
            {
                { AnimalStates.Idle, "Idle"},
                { AnimalStates.Idle2, "Idle2"},
                { AnimalStates.Walk, "Walk"},
                { AnimalStates.Run, "Run"},
                { AnimalStates.Attack, "Attack"},
                { AnimalStates.GetHit, "GetHit"},
                { AnimalStates.Death, "Death"}
            };

            CurrentState = AnimalStates.Idle;
            MiningObjectCollider.enabled = false;
        }

        protected override void Start()
        {
            base.Start();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            StartCoroutine(CheckPlayerDistance());
        }

        public void Init(GameManager gameManager, FaunaZone zone, Transform targetPoint)
        {
            GameManager = gameManager;
            _zone = zone;
            _targetObject = targetPoint;
            _initializaed = true;
        }

        protected void SetState(AnimalStates state)
        {
            CurrentState = state;
            foreach (var animatorTrigger in AnimatorTriggers)
                Animator.ResetTrigger(animatorTrigger.Value);

            Animator.SetTrigger(AnimatorTriggers[state]);

            if (state == AnimalStates.Walk)
                speed = WalkSpeed;
            else if (state == AnimalStates.Run)
                speed = RunSpeed;
        }

        protected IEnumerator CheckPlayerDistance()
        {
            while (true)
            {
                if (GameManager != null && !GameManager.PlayerModel.Dead && !IsDead)
                {
                    var distance = (transform.localPosition - GameManager.Player.transform.localPosition).sqrMagnitude;
                    if (FearPlayer && distance <= FearPlayerDistance && !_playerNear && !GameManager.Player.InHouse && CurrentState != AnimalStates.Run)
                    {
                        PlayerDetected();
                    }

                    if (_playerNear && distance >= StopFearDistance)
                    {
                        LoosePlayer();
                    }

                    AttackPlayer(distance <= AttackPlayerDistance && !GameManager.Player.InHouse);
                }

                yield return new WaitForSeconds(1.0f);
            }
        }

        protected virtual void PlayerDetected()
        {
            _playerNear = true;
            SetTarget(true);
            SetState(AnimalStates.Run);
        }

        protected virtual void LoosePlayer()
        {
            _playerNear = false;
        }

        protected virtual void AttackPlayer(bool atttack)
        {
        }

        public override void Update()
        {
            if (IsDead || !_initializaed)
                return;

            base.Update();

            //var posY = Terrain.activeTerrain.SampleHeight(transform.localPosition) + AnimalHeight;
            var posY = _zone.CurrentTerrain.SampleHeight(transform.localPosition) + AnimalHeight;
            transform.localPosition = new Vector3(transform.localPosition.x, posY, transform.localPosition.z);

            //if(CurrentState == AnimalStates.Run || CurrentState == AnimalStates.Walk)
            //{
            //    if (_rotationTime < 1.0f)
            //    {
            //        //transform.localRotation = Quaternion.Lerp(transform.localRotation, _toRotation, _rotationTime);
            //        _rotationTime += Time.deltaTime;
            //    }

            //    //Ray ray = new Ray(transform.localPosition + Vector3.up, Vector3.down);
            //    //RaycastHit hit = new RaycastHit();
            //    //if (Physics.Raycast(ray, out hit))
            //    //{
            //    //    transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.localRotation;
            //    //}
            //}
        }

        public override void OnTargetReached()
        {
            _playerNear = false;
            target = null;
            SetState(AnimalStates.Idle);
        }

        public virtual void OnIdleEnd()
        {
            var value = UnityEngine.Random.Range(0, 2);
            switch(value)
            {
                case 0:
                    if(_initializaed)
                    {
                        SetState(AnimalStates.Walk);
                        SetTarget();
                    }
                    else
                    {
                        SetState(AnimalStates.Idle2);
                    }
                    break;
                case 1:
                    SetState(AnimalStates.Idle2);
                    break;
            }
        }

        protected void SetTarget(bool runFromPlayer = false)
        {
            var nextPoint =  _zone.GetPointInside();
            if (runFromPlayer)
            {
                for (int i = 0; i < 10; i++)
                {
                    var distance = Vector3.Distance(nextPoint, transform.position);
                    if (distance < 100)
                        nextPoint = _zone.GetPointInside();
                    else break;
                }
            }
            var moveDirection = (nextPoint - transform.position).normalized;
            _toRotation = Quaternion.LookRotation(moveDirection);
            _rotationTime = 0.0f;
            _targetObject.position = nextPoint;

            target = _targetObject;
        }

        public void SetDamage(int damage, Vector3? hitPosition = null)
        {
            if (IsDead)
                return;

            Health -= damage;
            if(Health <= 0)
            {
                IsDead = true;
                MoveToLayer(transform, 0);
                MiningObjectCollider.enabled = true;
                SetDeathState();

                if (OnDeath != null)
                    OnDeath();
            }

            if (BloodSplatPrefab != null && damage > 5)
                StartCoroutine(BloodSplat(hitPosition));

            if (CurrentState == AnimalStates.Idle || CurrentState == AnimalStates.Idle2 ||
                CurrentState == AnimalStates.Walk)
            {
                OnGetDamageWhileCalmness();
            }
        }

        public void Damage(float damage)
        {
            SetDamage((int)damage);
        }

        protected virtual void OnGetDamageWhileCalmness()
        {
            SetTarget();
            SetState(AnimalStates.Run);
        }

        void MoveToLayer(Transform root, int layer)
        {
            root.gameObject.layer = layer;
            foreach (Transform child in root)
                MoveToLayer(child, layer);
        }

        protected virtual void SetDeathState()
        {
            SetState(AnimalStates.Death);
            if (!string.IsNullOrEmpty(DeadSoundName))
                SoundManager.PlaySFX(DeadSoundName);

            if (AnimalType == AnimalType.Deer)
            {
                GameCenterManager.ProgressAchievement(GameCenterManager.AchBeginnerHunterId);
                GooglePlayServicesController.ProgressAchievement(GPGSIds.achievement_beginner_hunter);
            }
            else if (AnimalType == AnimalType.Rabbit)
            {
                GameCenterManager.ProgressAchievement(GameCenterManager.AchRabbitLoverId);
                GooglePlayServicesController.ProgressAchievement(GPGSIds.achievement_rabbit_lover);
            }
        }
        protected virtual void PlayDetectedPlayerSfx()
        {
            SoundManager.PlaySFX(WorldConsts.AudioConsts.WolfGrowl, false, 0.0f, 0.3f);
        }

        void OnDestory()
        {
            StopAllCoroutines();
        }

        public IEnumerator BloodSplat(Vector3? hitPosition)
        {
            var splat = Instantiate(BloodSplatPrefab);
            splat.transform.position = hitPosition ?? transform.position;
            yield return new WaitForSeconds(0.5f);
            Destroy(splat);
        }

        public void AnimationEmptyForWait()
        {

        }
    }
}
