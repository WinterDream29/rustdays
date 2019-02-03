namespace Assets.Scripts.Controllers.Fauna
{
    public class Predator : Animal
    {
        public float Damage;

        protected override void PlayerDetected()
        {
            _playerNear = true;
            SetState(AnimalStates.Run);
            if (GameManager.Player.InHouse)
                SetTarget();
            else
            {
                PlayDetectedPlayerSfx();
                MoveByPlayer();
            }
        }

        protected override void LoosePlayer()
        {
            base.LoosePlayer();
            target = null;
            canMove = true;
        }

        protected virtual void MoveByPlayer()
        {
            target = GameManager.Player.transform;
        }

        protected override void AttackPlayer(bool atttack)
        {
            if (IsDead || GameManager.PlayerModel.Dead)
                return;

            if (CurrentState == AnimalStates.Attack || CurrentState == AnimalStates.Attack2Legs || CurrentState == AnimalStates.Attack4Legs)
            {
                if (atttack)
                {
                    transform.LookAt(GameManager.Player.transform);
                }
                else
                {
                    canMove = true;
                    PlayerDetected();
                }
            }
            else
            {
                if(atttack)
                {
                    SetAttackState();
                }
            }
        }

        protected virtual void SetAttackState()
        {
            target = null;
            canMove = false;
            SetState(AnimalStates.Attack);
        }

        protected override void OnGetDamageWhileCalmness()
        {
            PlayerDetected();
        }

        public void OnAttack()
        {
            GameManager.PlayerModel.Damage(Damage);

            if (GameManager.PlayerModel.Dead)
            {
                _playerNear = false;
                canMove = true;
                SetTarget();
                SetState(AnimalStates.Walk);
            }

            if (!string.IsNullOrEmpty(AttackSoundName))
                SoundManager.PlaySFX(AttackSoundName);
        }
    }
}
