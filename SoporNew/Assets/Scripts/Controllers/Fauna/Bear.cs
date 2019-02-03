using System.Collections.Generic;
namespace Assets.Scripts.Controllers.Fauna
{
    public class Bear : Predator
    {
        protected override void Start()
        {
            base.Start();

            AnimatorTriggers = new Dictionary<AnimalStates, string>
            {
                { AnimalStates.Idle2Legs, "Idle2Legs"},
                { AnimalStates.Idle4Legs, "Idle4Legs"},
                { AnimalStates.Walk, "Walk"},
                { AnimalStates.Run, "Run"},
                { AnimalStates.To4Legs, "To4Legs"},  
                { AnimalStates.To2Legs, "To2Legs"},  
                { AnimalStates.Attack4Legs, "Attack4Legs"},
                { AnimalStates.Attack4LegsClawL, "Attack4LegsClawL"},
                { AnimalStates.Attack4LegsClawR, "Attack4LegsClawR"},     
                { AnimalStates.Attack2Legs, "Attack2Legs"},  
                { AnimalStates.Attack2LegsClawL, "Attack2LegsClawL"},  
                { AnimalStates.Attack2LegsClawR, "Attack2LegsClawR"},  
                { AnimalStates.Roar4Legs, "Roar4"},  
                { AnimalStates.Roar2Legs, "Roar2"},  
                { AnimalStates.Death4Legs, "4Death"},
                { AnimalStates.Death2Legs, "2Death"}
            };

            CurrentState = AnimalStates.Idle;
            MiningObjectCollider.enabled = false;
        }

        protected override void PlayDetectedPlayerSfx()
        {
            SoundManager.PlaySFX(WorldConsts.AudioConsts.BearGrowl, false, 0.0f, 0.3f);
        }

        public override void OnTargetReached()
        {
            _playerNear = false;
            target = null;
            SetState(AnimalStates.Idle4Legs);
        }

        protected override void SetAttackState()
        {
            target = null;
            canMove = false;
            SetState(AnimalStates.Attack4Legs);
        }

        public override void OnIdleEnd()
        {
            var value = UnityEngine.Random.Range(0, 2);
            switch (value)
            {
                case 0:
                    SetTarget();
                    SetState(AnimalStates.Walk);
                    break;
                case 1:
                    SetState(AnimalStates.To2Legs);
                    break;
            }
        }

        public void OnIdle2LegsEnd()
        {
            SetState(AnimalStates.To4Legs);
        }

        protected override void SetDeathState()
        {
            SetState(AnimalStates.Death4Legs);
            if (!string.IsNullOrEmpty(DeadSoundName))
                SoundManager.PlaySFX(DeadSoundName);

            GameCenterManager.ProgressAchievement(GameCenterManager.AchProfessionalHunterId);
            GooglePlayServicesController.ProgressAchievement(GPGSIds.achievement_professional_hunter, 1);
        }
    }
}
