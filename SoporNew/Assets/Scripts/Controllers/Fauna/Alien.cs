using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Controllers.UsableObjects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Controllers.Fauna
{
    public enum AlienStates
    {
        None,
        Idle,
        Walk,
        WalkBusy,
        Run,
        Grab,
        Petting,
        Dead,
        Attack1,
        GetDamage,
        IdleLookAround,
        Kneel,
        Pointing,
        SittingIdle,
        BlowKiss,
        Dance,
        BootyDance,
        MagicAttack,
        Block,
        LookFear,
        SittingSearch,
        RunLookBack
    }

    public class Alien : Enemy
    {
        public float WalkSpeed;
        public float RunSpeed;
        public Animator Animator;
        public float AdditionalHeigth;
        public Vector2 NextPointFindPerimetr;
        public LayerMask WaterLayerMask;
        public float PlayerDetectedDistance;
        public float PlayerLooseDistance;
        public List<AlienBallController> Weapons;
        public RFX4_EffectEvent WeaponsEvent;
        public GameObject SpawnEffect;
        public GameObject HideEffect;
        public AudioSource Audio;
        public AudioClip DieClip;
        public List<AudioClip> TalkClips;
        public bool CanWalkToCampFire;
        public float MoveToCampFireDelay;

        protected GameManager _gameManager;
        private Dictionary<AlienStates, string> _animatorTriggers;
        protected AlienStates _currentState = AlienStates.None;
        private GameObject _targetObject;
        protected bool _playerNear;
        protected bool MovingToUfo;
        protected Terrain _currentTerrain;
        public bool InUfo { get; protected set; }
        private UFOController _ufo;
        private bool _runFromPlayer;
        private bool _toCampFire;
        private GameObject _currentCampFire;
        private float _moveToCampFireDelayStart;

        protected override void Awake()
        {
            base.Awake();

            _animatorTriggers = new Dictionary<AlienStates, string>
            {
                {AlienStates.Idle, "Idle"},
                {AlienStates.Walk, "Walk"},
                {AlienStates.WalkBusy, "WalkBusy"},
                {AlienStates.Run, "Run"},
                {AlienStates.Grab, "Grab"},
                {AlienStates.Petting, "Petting"},
                {AlienStates.Attack1, "Attack_1"},
                {AlienStates.GetDamage, "GetDamage"},
                {AlienStates.Dead, "Dead_1"},
                {AlienStates.IdleLookAround, "IdleLookAround"},
                {AlienStates.Kneel, "Kneel"},
                {AlienStates.Pointing, "Pointing"},
                {AlienStates.SittingIdle, "SittingIdle"},
                {AlienStates.BlowKiss, "BlowKiss"},
                {AlienStates.Dance, "Dance"},
                {AlienStates.BootyDance, "BootyDance"},
                {AlienStates.Block, "Block"},
                {AlienStates.MagicAttack, "MagicAttack"},
                {AlienStates.LookFear, "LookFear"},
                {AlienStates.RunLookBack, "RunLookBack"},
                {AlienStates.SittingSearch, "SittingSearch"}
            };

            foreach (var animatorTrigger in _animatorTriggers)
                Animator.ResetTrigger(animatorTrigger.Value);

            _targetObject = new GameObject();
            _targetObject.name = "AlienTargetPoint";
            // _targetObject.transform.position = GetPointInside();

            gameObject.SetActive(false);
            if (SpawnEffect != null)
                SpawnEffect.SetActive(false);
            if(HideEffect != null)
                HideEffect.SetActive(false);
            _moveToCampFireDelayStart = MoveToCampFireDelay;
        }

        protected override void Start()
        {
            base.Start();
        }

        public void Init(UFOController ufo, GameManager gameManager)
        {
            _ufo = ufo;
            _gameManager = gameManager;
            InUfo = true;

            foreach (var alienBallController in Weapons)
            {
                alienBallController.Init(_gameManager);
            }

            _gameManager.Player.OnEnterCar += OnPlayerEnterCar;
            _currentTerrain = _gameManager.Terrain1;
        }

        private void OnPlayerEnterCar(bool enter)
        {
            PlayerEnterCar(enter);
        }

        protected virtual void PlayerEnterCar(bool enter)
        {
            
        }

        public virtual void GetOffFromUfo(Vector3 position, Terrain terrain)
        {
            _currentTerrain = terrain;
            IsDead = false;
            Health = StartHealth;
            InUfo = false;
            MovingToUfo = false;
            _toCampFire = false;
            transform.position = position;
            gameObject.SetActive(true);
            SetState(AlienStates.IdleLookAround);
            StartCoroutine(CheckPlayerDistance());
            if (SpawnEffect != null)
                SpawnEffect.SetActive(true);

            MoveToCampFireDelay = _moveToCampFireDelayStart;
            MoveToCampFireDelay -= _moveToCampFireDelayStart / 2;
            StartCoroutine(CheckCurrentTerrain());
        }

        protected void EnterToUfo()
        {
            if (IsDead)
                return;

            InUfo = true;
            SetState(AlienStates.Idle);
            StopAllCoroutines();
            StartCoroutine(DelayHide());
            if (HideEffect != null)
                HideEffect.SetActive(true);
        }

        private IEnumerator DelayHide()
        {
            yield return new WaitForSeconds(2.0f);
            gameObject.SetActive(false);
        }

        protected void SetState(AlienStates state)
        {
            canMove = false;
            _currentState = state;
            Animator.SetTrigger(_animatorTriggers[state]);

            if (_currentState == AlienStates.Run || _currentState == AlienStates.Walk ||
                _currentState == AlienStates.WalkBusy)
                canMove = true;

            if (_currentState == AlienStates.Run)
                speed = RunSpeed;
            else
                speed = WalkSpeed;

            if (_currentState == AlienStates.SittingIdle)
                StartCoroutine(DelaySetRandomState(Random.Range(10.0f, 20.0f)));
        }

        public virtual void OnIdleEnd()
        {
            if (_playerNear)
            {
                SetTarget();
                SetState(AlienStates.Run);
            }
            else
                SetRandomState();
        }

        private void SetRandomState()
        {
            var value = UnityEngine.Random.Range(0, 100);
            if (value > 95)
            {
                SetState(AlienStates.Idle);
            }
            else if (value > 85)
            {
                SetState(AlienStates.IdleLookAround);

                if (TalkClips.Count > 0 && Random.Range(0, 2) == 0)
                    Audio.PlayOneShot(TalkClips[Random.Range(0, TalkClips.Count)]);
            }
            else if(value > 75)
                SetState(AlienStates.SittingIdle);
            else if (value > 40)
            {
                var ran = Random.Range(0, 100);
                if (MoveToCampFireDelay <= 0 && ran < 65)
                {
                    MoveToCampFireDelay = _moveToCampFireDelayStart;
                    MoveToCampFire();
                }
                else
                {
                    SetState(AlienStates.Walk);
                    SetTarget();
                }
            }
            else if (value > 25)
            {
                SetState(AlienStates.Grab);
                if (TalkClips.Count > 0 && Random.Range(0, 2) == 0)
                    Audio.PlayOneShot(TalkClips[Random.Range(0, TalkClips.Count)]);
            }
            else if (value > 10)
            {
                SetState(AlienStates.SittingSearch);

                if(TalkClips.Count > 0 && Random.Range(0,2) == 0)
                    Audio.PlayOneShot(TalkClips[Random.Range(0, TalkClips.Count)]);
            }
            else
            {
                SetState(AlienStates.Petting);

                if (TalkClips.Count > 0)
                    Audio.PlayOneShot(TalkClips[Random.Range(0, TalkClips.Count)]);
            }
        }

        private void MoveToCampFire()
        {
            _runFromPlayer = false;
            CookingInteractive fire = null;
            var campFires = GameObject.FindGameObjectsWithTag("CampFire");
            foreach (var campFire in campFires)
            {
                fire = campFire.GetComponent<CookingInteractive>();
                if (fire != null && fire.IsBurning)
                    break;
            }

            if (fire != null && fire.IsBurning)
            {
                _toCampFire = true;
                _currentCampFire = fire.gameObject;
                var traget = new Vector3(fire.transform.position.x + 2, fire.transform.position.y, fire.transform.position.z);
                _targetObject.transform.position = traget;
                SetTarget(_targetObject.transform);
                SetState(AlienStates.Run);
            }
            else
            {
                SetState(AlienStates.Walk);
                SetTarget();
            }
        }

        public void OnKneelEnd()
        {
            SetState(AlienStates.SittingIdle);
        }

        private IEnumerator DelaySetRandomState(float delayTime)
        {
            yield return new WaitForSeconds(delayTime);
            SetRandomState();
        }

        protected void SetTarget(Transform newTarget = null)
        {
            if (newTarget == null)
            {
                var nextPoint = GetPointInside();
                _targetObject.transform.position = nextPoint;
                target = _targetObject.transform;
            }
            else
                target = newTarget;
        }

        public override void OnTargetReached()
        {
            target = null;

            if(IsDead)
                return;

            if (MovingToUfo)
            {
                EnterToUfo();
                _ufo.MoveAlienToUfo(this);
                MovingToUfo = false;
            }
            else if (_toCampFire)
            {
                _toCampFire = false;
                transform.LookAt(_currentCampFire.transform);
                SetState(AlienStates.Kneel);

                if(TalkClips.Count > 0)
                    Audio.PlayOneShot(TalkClips[Random.Range(0, TalkClips.Count)]);
            }
            else
            {
                if (_playerNear)
                {
                    SetTarget();
                    SetState(AlienStates.Run);
                }
                else
                {
                    _runFromPlayer = false;
                    SetRandomState();
                }
            }
        }

        public override void Update()
        {
            base.Update();

            if(IsDead)
                return;

            var posY = _currentTerrain.SampleHeight(transform.localPosition) + AdditionalHeigth;
            transform.localPosition = new Vector3(transform.localPosition.x, posY, transform.localPosition.z);

            //if (_currentState == AlienStates.Run || _currentState == AlienStates.Walk)
            //{
            //    if (_rotationTime < 1.0f)
            //    {
            //        transform.localRotation = Quaternion.Lerp(transform.localRotation, _toRotation, _rotationTime);
            //        _rotationTime += Time.deltaTime;
            //    }
            //}

            if(CanWalkToCampFire && !_toCampFire && MoveToCampFireDelay > 0)
                MoveToCampFireDelay -= Time.deltaTime;
        }

        public Vector3 GetPointInside()
        {
            var targetPoint = transform.localPosition +
                                new Vector3(
                                    Random.Range(NextPointFindPerimetr.x / 2, -NextPointFindPerimetr.x / 2), 0.0f,
                                    Random.Range(NextPointFindPerimetr.y / 2, -NextPointFindPerimetr.y / 2));

            targetPoint.y = _currentTerrain.SampleHeight(targetPoint);
            targetPoint.y -= 0.1f;

            //RaycastHit hit;
            //Physics.Raycast(targetPoint, -Vector3.up, out hit, 100, WaterLayerMask);
            //if (hit.collider == null || hit.collider.gameObject == null)
            //{
            //    return targetPoint;
            //}
            //if (hit.collider.gameObject.tag == "Water")
            //{
            //    GetPointInside();
            //}

            return targetPoint;
        }

        protected IEnumerator CheckPlayerDistance()
        {
            while (true)
            {
                if (!MovingToUfo)
                {
                    if (_gameManager != null && _gameManager.PlayerModel != null && !_gameManager.PlayerModel.Dead &&
                        !IsDead)
                    {
                        var distance = (transform.position - _gameManager.Player.transform.position).sqrMagnitude;
                        if (_gameManager.Player.InCar)
                            distance =
                                (transform.position - _gameManager.CarInteractive.transform.position).sqrMagnitude;

                        if (distance <= PlayerDetectedDistance && !_playerNear /*&& !_gameManager.Player.InHouse*/&&
                            !_runFromPlayer)
                        {
                            PlayerDetected();
                        }

                        if (_playerNear && distance >= PlayerLooseDistance)
                        {
                            LoosePlayer();
                        }
                        //AttackPlayer(distance <= AttackPlayerDistance && !GameManager.Player.InHouse);
                    }
                }

                yield return new WaitForSeconds(0.3f);
            }
        }

        protected virtual void PlayerDetected()
        {
            canMove = false;
            _playerNear = true;
            _runFromPlayer = true;
            SetState(AlienStates.LookFear);
        }

        protected virtual void LoosePlayer()
        {
            _playerNear = false;
        }

        protected virtual void OnAttackEnd()
        {
            
        }

        protected virtual void Attack()
        {
            //WeaponsEvent.Effect = Weapons[Random.Range(0, Weapons.Count)].gameObject;
            canMove = false;
            SetState(AlienStates.MagicAttack);

            //var pos = gameObject.transform.position;
            //var posSelf = _gameManager.Player.transform.position;
            //var posVector2 = (new Vector2(pos.x, pos.z) - new Vector2(posSelf.x, posSelf.z)).normalized;

            //var enemyAngle = Mathf.Atan2(posVector2.y, posVector2.x) * Mathf.Rad2Deg;
            //gameObject.transform.rotation = Quaternion.Euler(new Vector3(0f, 90 - enemyAngle));
            if(_gameManager.Player.InCar)
                transform.LookAt(_gameManager.CarInteractive.transform);
            else
                transform.LookAt(_gameManager.Player.transform);
            //SpawnMagicBall();
        }

        protected override void SetDeathState()
        {
            base.SetDeathState();

            SetState(AlienStates.Dead);
            Audio.PlayOneShot(DieClip);
            if (!string.IsNullOrEmpty(DeadSoundName))
                SoundManager.PlaySFX(DeadSoundName);
            Killed();
        }

        protected virtual void Killed()
        {
            _ufo.FriendlyAlienKilled();
        }

        protected override void OnGetDamageWhileCalmness()
        {
            if (MovingToUfo)
                return;

            SetTarget();
            SetState(AlienStates.Run);
        }

        public void MoveToUfo()
        {
            if(MovingToUfo)
                return;

            MovingToUfo = true;
            canMove = true;
            SetTarget(_ufo.transform);
            SetState(AlienStates.Run);
        }

        public void OnDeadEnd()
        {
            gameObject.SetActive(false);
            InUfo = true;
            StopAllCoroutines();
        }

        protected IEnumerator CheckCurrentTerrain()
        {
            while (true)
            {
                RaycastHit hit;
                Physics.Raycast(transform.position, -Vector3.up, out hit, 10);
                if (hit.collider != null && hit.collider.gameObject != null)
                {
                    if (hit.collider.gameObject.name == "Terrain1")
                        _currentTerrain = _gameManager.Terrain1;

                    if (hit.collider.gameObject.name == "Terrain2")
                        _currentTerrain = _gameManager.Terrain2;
                }

                yield return new WaitForSeconds(6.0f);
            }
        }

        void OnDestroy()
        {
            StopAllCoroutines();
        }
    }
}