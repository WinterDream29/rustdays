using System.Collections;
using Assets.Scripts.Models;
using Assets.Scripts.Models.Food;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public enum FishrodStates
    {
        Available,
        Process,
        GettingFish,
        Peck
    }
    public class FishrodController : MonoBehaviour
    {
        public Animation FishrodAnimation;
        public float BobberOffset = 0.0f;

        public bool CanToFish { get; private set; }

        private GameManager _gameManager;
        public FishrodStates CurrentState;
        private Vector2 _getFishTimeRand = new Vector2(10f, 20f);
        private float _getFishTime;
        private float _passedTime;
        private bool _pecked;

        void Start()
        {
            CurrentState = FishrodStates.Available;
            _getFishTime = Random.Range(_getFishTimeRand.x, _getFishTimeRand.y);
            _gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
            _gameManager.Player.FishrodBobberParent.gameObject.SetActive(false);
        }

        public void Use()
        {
            switch (CurrentState)
            {
                case FishrodStates.Available:
                    ThrowFishrod();
                    break;
                case FishrodStates.Process:
                case FishrodStates.Peck:
                    GetFish();
                    break;
                case FishrodStates.GettingFish:
                    break;
            }
        }

        private void ThrowFishrod()
        {
            FishrodAnimation.Play("ThrowFishrod");
            StartCoroutine(SetBubbleState(true, 0.8f));
        }

        private IEnumerator SetBubbleState(bool state, float delay)
        {
            yield return new WaitForSeconds(delay);
            _gameManager.Player.FishrodBobberParent.gameObject.SetActive(state);
        }

        public void GetFish()
        {
            if (_pecked)
            {
                FishrodAnimation.Play("GetFish");
                AddFish();
            }
            else
                FishrodAnimation.Play("GetWithoutFish");

            CurrentState = FishrodStates.GettingFish;
            StartCoroutine(SetBubbleState(false, 0.3f));
        }

        private void AddFish()
        {
            var holder = HolderObjectFactory.GetItem(typeof(Fish), 1);
            _gameManager.PlayerModel.Inventory.AddItem(holder);
            _gameManager.Player.MainHud.GettingResourceView.ShowItem(holder.Item.IconName, holder.Amount, holder.Item.LocalizationName);
        }

        public void FishGetted()
        {
            CurrentState = FishrodStates.Available;
        }

        public void OnPorcess()
        {
            CurrentState = FishrodStates.Process;
        }

        private IEnumerator Peck()
        {
            CurrentState = FishrodStates.Peck;
            var bobber = _gameManager.Player.FishrodBobber;

            _pecked = true;
            TweenPosition.Begin(bobber.gameObject, 0.2f, new Vector3(bobber.localPosition.x, bobber.localPosition.y - 0.5f, bobber.localPosition.z));
            yield return new WaitForSeconds(0.3f);
            _pecked = false;
            TweenPosition.Begin(bobber.gameObject, 0.2f, new Vector3(bobber.localPosition.x, bobber.localPosition.y + 0.5f, bobber.localPosition.z));
            yield return new WaitForSeconds(UnityEngine.Random.Range(0.8f, 2f));

            _pecked = true;
            TweenPosition.Begin(bobber.gameObject, 0.2f, new Vector3(bobber.localPosition.x, bobber.localPosition.y - 0.6f, bobber.localPosition.z));
            yield return new WaitForSeconds(0.5f);
            _pecked = false;
            TweenPosition.Begin(bobber.gameObject, 0.2f, new Vector3(bobber.localPosition.x, bobber.localPosition.y + 0.6f, bobber.localPosition.z));
            yield return new WaitForSeconds(UnityEngine.Random.Range(0.6f, 1.2f));

            _pecked = true;
            TweenPosition.Begin(bobber.gameObject, 0.1f, new Vector3(bobber.localPosition.x, bobber.localPosition.y - 0.7f, bobber.localPosition.z));
            yield return new WaitForSeconds(0.8f);
            _pecked = false;
            TweenPosition.Begin(bobber.gameObject, 0.2f, new Vector3(bobber.localPosition.x, bobber.localPosition.y + 0.7f, bobber.localPosition.z));
            yield return new WaitForSeconds(0.2f);

            _pecked = false;
            if (CurrentState == FishrodStates.Peck)
                CurrentState = FishrodStates.Process;
            yield break;
        }

        void Update()
        {
            RaycastHit hit;
            Physics.Raycast(_gameManager.Player.FishrodCheckWaterTransform.position, -Vector3.up, out hit, 10);
            if (hit.collider == null || hit.collider.gameObject == null)
                return;

            CanToFish = hit.collider.gameObject.tag == "Water";

            if (CanToFish)
            {
                _gameManager.Player.FishrodBobberParent.position = new Vector3(_gameManager.Player.FishrodBobberParent.position.x, hit.collider.transform.position.y + BobberOffset, _gameManager.Player.FishrodBobberParent.position.z);

                if (CurrentState == FishrodStates.Process && !_gameManager.Player.FishrodBobberParent.gameObject.activeSelf)
                    _gameManager.Player.FishrodBobberParent.gameObject.SetActive(true);
            }
            else
            {
                _gameManager.Player.FishrodBobberParent.gameObject.SetActive(false);
                if (CurrentState == FishrodStates.Process)
                {
                    CurrentState = FishrodStates.Available;
                    FishrodAnimation.Play("GetWithoutFish");
                }
            }

            if (CurrentState == FishrodStates.Process)
            {
                _passedTime += Time.deltaTime;
                if (_passedTime >= _getFishTime && CurrentState != FishrodStates.Peck)
                {
                    StartCoroutine(Peck());
                    _passedTime = 0;
                    _getFishTime = Random.Range(_getFishTimeRand.x, _getFishTimeRand.y);
                }
            }
        }
    }
}