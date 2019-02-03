using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Controllers.Fauna;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public enum UFOState
    {
        InSpace,
        OnIsland,
        MoveToIsland,
        MoveToSpace
    }

    public class UFOController : MonoBehaviour
    {
        public float Speed;
        public List<Alien> Aliens;
        public List<AngryAlien> AngryAliens;
        public float RangeSpawn;
        public AudioSource Audio;
        public AudioClip IdleSound;
        public AudioClip PalarmSound;

        public Vector3 StartPosition { get; set; }
        public UFOState CurrentState { get; private set; }

        private bool _huntingOnPlayer;
        private Coroutine _huntingCoroutine;
        private GameManager _gameManager;
        private Terrain _currentTerrain;

        void Awake()
        {
            StartPosition = transform.position;
            CurrentState = UFOState.InSpace;
        }

        void Start ()
        {
            Audio.clip = IdleSound;
            Audio.Stop();
        }

        public void Init(GameManager gameManager)
        {
            _gameManager = gameManager;

            foreach (var alien in Aliens)
                alien.Init(this, _gameManager);

            foreach (var alien in AngryAliens)
                alien.Init(this, _gameManager);

            _currentTerrain = _gameManager.Terrain1;
        }

        void Update ()
        {
		
        }

        public IEnumerator MoveToPoint(Vector3 point)
        {
            CurrentState = UFOState.MoveToIsland;
            Audio.clip = IdleSound;
            Audio.volume = 0.75f;
            Audio.Play();

            while (transform.position != point)
            {
                transform.position = Vector3.MoveTowards(transform.position, point, Speed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }

            CurrentState = UFOState.OnIsland;
            CheckCurrentTerrain();
            OnMovedToIsland();
        }

        public void StartMoveAliensToUfo()
        {
            foreach (var alien in Aliens)
                alien.MoveToUfo();

            foreach (var alien in AngryAliens)
                alien.MoveToUfo();
        }

        public void MoveAlienToUfo(Alien alien)
        {
            var allIn = true;
            foreach (var al in Aliens)
            {
                if (!al.InUfo && !al.IsDead)
                    allIn = false;
            }
            foreach (var al in AngryAliens)
            {
                if (!al.InUfo && !al.IsDead)
                    allIn = false;
            }

            if (allIn)
                StartCoroutine(MoveToSpace());
        }

        public IEnumerator MoveToSpace()
        {
            CurrentState = UFOState.MoveToSpace;

            StopHunting();

            while (transform.position != StartPosition)
            {
                transform.position = Vector3.MoveTowards(transform.position, StartPosition, Speed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            CurrentState = UFOState.InSpace;
            Audio.Stop();
        }

        private void OnMovedToIsland()
        {
            foreach (var alien in Aliens)
            {
                var additionalPos = Random.Range(-RangeSpawn, RangeSpawn);
                var pos = new Vector3(transform.position.x + additionalPos, transform.position.y, transform.position.z + additionalPos);
                alien.GetOffFromUfo(pos, _currentTerrain);
            }
        }

        public void FriendlyAlienKilled()
        {
            _huntingCoroutine = StartCoroutine(StartHunting());
        }

        public void AngryKilled()
        {
            
        }

        private IEnumerator StartHunting()
        {
            if (_huntingOnPlayer)
                yield break;

            _huntingOnPlayer = true;

            Audio.clip = PalarmSound;
            Audio.volume = 1.0f;
            Audio.Play();

            foreach (var angryAlien in AngryAliens)
            {
                yield return new WaitForSeconds(6.0f);
                var additionalPos = Random.Range(-RangeSpawn, RangeSpawn);
                var pos = new Vector3(transform.position.x + additionalPos, transform.position.y, transform.position.z + additionalPos);
                angryAlien.GetOffFromUfo(pos, _currentTerrain);
            }
        }

        private void StopHunting()
        {
            if(!_huntingOnPlayer)
                return;

            Audio.clip = IdleSound;
            Audio.volume = 0.75f;
            Audio.Play();

            _huntingOnPlayer = false;

            if (_huntingCoroutine != null)
                StopCoroutine(_huntingCoroutine);
        }

        private void CheckCurrentTerrain()
        {
                RaycastHit hit;
                Physics.Raycast(transform.position, -Vector3.up, out hit, 200);
                if (hit.collider != null && hit.collider.gameObject != null)
                {
                    if (hit.collider.gameObject.name == "Terrain1")
                        _currentTerrain = _gameManager.Terrain1;

                    if (hit.collider.gameObject.name == "Terrain2")
                        _currentTerrain = _gameManager.Terrain2;
                }
        }
    }
}