using UnityEngine;

namespace Assets.Scripts.Controllers.Fauna
{
    public class AngryAlien : Alien
    {
        public override void Update()
        {
            base.Update();

            if (_gameManager.PlayerModel.Dead && !MovingToUfo && !IsDead)
            {
                MoveToUfo();
            }
        }

        public override void GetOffFromUfo(Vector3 position, Terrain terrain)
        {
            _currentTerrain = terrain;
            IsDead = false;
            Health = StartHealth;
            InUfo = false;
            MovingToUfo = false;

            transform.position = position;
            gameObject.SetActive(true);
            SetState(AlienStates.IdleLookAround);
            StartCoroutine(CheckPlayerDistance());

            if(SpawnEffect != null)
                SpawnEffect.SetActive(true);

            StartCoroutine(CheckCurrentTerrain());
        }

        public override void OnIdleEnd()
        {
            if (IsDead)
                return;

            SetState(AlienStates.Run);
            if(_gameManager.Player.InCar)
                SetTarget(_gameManager.CarInteractive.transform);
            else
                SetTarget(_gameManager.Player.transform);
        }

        protected override void PlayerEnterCar(bool enter)
        {
            if(IsDead)
                return;

            if(MovingToUfo)
                return;

            if (enter)
                SetTarget(_gameManager.CarInteractive.transform);
            else
                SetTarget(_gameManager.Player.transform);

            if(!_playerNear)
                SetState(AlienStates.Run);
        }

        protected override void PlayerDetected()
        {
            _playerNear = true;
            target = null;
            canMove = false;
            Attack();
        }

        protected override void LoosePlayer()
        {
            _playerNear = false;
        }

        protected override void OnAttackEnd()
        {
            if(IsDead)
                return;

            if(MovingToUfo)
                return;

            if (_playerNear)
            {
                Attack();
            }
            else
            {
                canMove = true;
                if(_gameManager.Player.InCar)
                    SetTarget(_gameManager.CarInteractive.transform);
                else
                    SetTarget(_gameManager.Player.transform);

                SetState(AlienStates.Run);
            }
        }

        protected override void OnGetDamageWhileCalmness()
        {
            if (_currentState != AlienStates.MagicAttack && !MovingToUfo)
            {
                OnAttackEnd();
            }
        }

        protected override void Killed()
        {
        }
    }
}