using System.Collections.Generic;
using Assets.Scripts.Models;
using Assets.Scripts.Models.Clothes.Backpack;
using Assets.Scripts.Ui;
using UnityEngine;
using System.Collections;
using System;

namespace Assets.Scripts.UI.Craft
{
    public class CraftSelectedItemView : View
    {
        public UIGrid RecipeItemsGrid;
        public GameObject RecipeItemPrefab;
        public CraftItem ResultItemView;
        public GameObject CraftButton;
        public UISprite CraftButtonBackground;
        public Color ActiveCraftColor;
        public Color InactiveCraftColor;
        public UILabel Description;
        public BoxCollider CloseCollider;
        public UILabel MaxLabel;
        public BuyResourcesDialog BuyResourcesView;

        public Action<CraftSelectedItemView> OnBiggestBackpack { get; set; }
        public Action NoManyAction { get; set; }

        private List<CraftRecipeItem> _recipeItems = new List<CraftRecipeItem>();
        private bool _canCraft;
        private List<GameObject> _craftedItemsAnimation = new List<GameObject>();

        public override void Init(GameManager gameManager)
        {
            base.Init(gameManager);

            BuyResourcesView.Init(gameManager);
            BuyResourcesView.NoManyAction += NoMany;

            MaxLabel.enabled = false;

            UIEventListener.Get(CraftButton).onClick += OnCraftButtonClick;
            UIEventListener.Get(CloseCollider.gameObject).onClick += go => Hide();
        }

        public void SetView(CraftItem resultItem, bool isMax = false)
        {
            Description.text = Localization.Get(resultItem.ItemModel.Description);

            foreach (var craftRecipeItem in _recipeItems)
                Destroy(craftRecipeItem.gameObject);
            _recipeItems.Clear();

            ResultItemView.Init(GameManager, resultItem.Category, resultItem.ItemModel.GetType(), "+" + resultItem.ItemModel.CraftAmount);

            if(isMax)
            {
                MaxLabel.enabled = true;
            }
            else
            {
                MaxLabel.enabled = false;

                foreach (var recipeItem in ResultItemView.ItemModel.CraftRecipe)
                {
                    var go = NGUITools.AddChild(RecipeItemsGrid.gameObject, RecipeItemPrefab);
                    var item = go.GetComponent<CraftRecipeItem>();

                    var currentAmount = GameManager.PlayerModel.Inventory.GetAmount(recipeItem.Item.GetType());

                    item.Init(recipeItem.Item, recipeItem.Amount, currentAmount, InactiveCraftColor, ActiveCraftColor);
                    _recipeItems.Add(item);
                }
            }

            _canCraft = GameManager.PlayerModel.Inventory.CheckItems(ResultItemView.ItemModel.CraftRecipe);
            if (isMax)
                _canCraft = false;
            CraftButtonBackground.color = _canCraft ? ActiveCraftColor : InactiveCraftColor;

            StartCoroutine(DelayReposition());
        }

        public override void UpdateView()
        {
            _canCraft = GameManager.PlayerModel.Inventory.CheckItems(ResultItemView.ItemModel.CraftRecipe);
            if (ResultItemView.ItemModel is Backpack && GameManager.PlayerModel.CurrentBackpack == BackpackType.Big)
                _canCraft = false;

            CraftButtonBackground.color = _canCraft ? ActiveCraftColor : InactiveCraftColor;

            foreach (var item in _recipeItems)
            {
                var currentAmount = GameManager.PlayerModel.Inventory.GetAmount(item.Item.GetType());
                item.UpdateView(currentAmount);
            }
        }

        public override void Hide()
        {
            StopAllCoroutines();

            foreach (var item in _craftedItemsAnimation)
                Destroy(item);
            _craftedItemsAnimation.Clear();
            GameManager.Player.MainHud.CraftPanel.ItemsMainObject.SetActive(true);
            base.Hide();
        }

        private IEnumerator DelayReposition()
        {
            yield return new WaitForEndOfFrame();

            RecipeItemsGrid.Reposition();
        }

        private void OnCraftButtonClick(GameObject go)
        {
            StartCraft();
        }

        private void StartCraft()
        {
            if (_canCraft)
            {
                if (ResultItemView.ItemModel is Backpack)
                {
                    GameManager.PlayerModel.BiggestBackpack();
                    if (OnBiggestBackpack != null)
                        OnBiggestBackpack(this);
                }
                else
                {
                    GameManager.PlayerModel.Inventory.RemoveItems(ResultItemView.ItemModel.CraftRecipe);
                    var item = HolderObjectFactory.GetItem(ResultItemView.ItemModel.GetType(), ResultItemView.ItemModel.CraftAmount);
                    var isAdd = GameManager.PlayerModel.Inventory.AddItem(item);
                    if(!isAdd)
                        GameManager.PlacementItemsController.DropItemToGround(GameManager, item);
                }

                UpdateView();
                GameManager.Player.MainHud.CraftPanel.UpdateView();
                SoundManager.PlaySFX(WorldConsts.AudioConsts.Craft);
                StartCoroutine(AnimateAddItem());
            }
            else
            {
                int amountChecked = 0;
                var notCheckItems = new List<HolderObject>();
                foreach (var kvPair in ResultItemView.ItemModel.CraftRecipe)
                {
                    var amountHold = GameManager.PlayerModel.Inventory.GetAmount(kvPair.Item.GetType());
                    if (amountHold >= kvPair.Amount)
                        amountChecked++;
                    else
                        notCheckItems.Add(new HolderObject(kvPair.Item.GetType(), kvPair.Amount - amountHold));
                }
                if (notCheckItems.Count > 0)
                {
                    bool showBuyPanel = true;
                    foreach (var notCheckItem in notCheckItems)
                    {
                        if (!notCheckItem.Item.CanBuy)
                            showBuyPanel = false;
                    }

                    if (showBuyPanel)
                    {
                        BuyResourcesView.Show(notCheckItems, this);
                    }
                }
            }
        }

        private IEnumerator AnimateAddItem()
        {
            var cloneItem = Instantiate(ResultItemView.gameObject);
            cloneItem.transform.position = ResultItemView.gameObject.transform.position;
            cloneItem.transform.parent = ResultItemView.gameObject.transform.parent;
            cloneItem.transform.localScale = Vector3.one;

            _craftedItemsAnimation.Add(cloneItem);

            TweenScale.Begin(cloneItem, 0.25f, new Vector3(1.3f, 1.3f, 1.3f));
            yield return new WaitForSeconds(0.25f);
            TweenScale.Begin(cloneItem, 0.25f, new Vector3(1.1f, 1.1f, 1.1f));
            yield return new WaitForSeconds(0.3f);
            TweenPosition.Begin(cloneItem, 0.5f, new Vector3(ResultItemView.gameObject.transform.position.x, ResultItemView.gameObject.transform.position.y - 600f, ResultItemView.gameObject.transform.position.z));
            yield return new WaitForSeconds(0.5f);

            if (cloneItem != null)
            {
                _craftedItemsAnimation.Remove(cloneItem);
                Destroy(cloneItem);
            }
        }

        private void NoMany()
        {
            BuyResourcesView.Hide();
            if (NoManyAction != null)
                NoManyAction();
        }
    }
}
