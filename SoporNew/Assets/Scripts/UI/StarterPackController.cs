using Assets.Scripts;
using Assets.Scripts.Ui;
using System.Collections;
using Assets.Scripts.UI.ShopNew;
using UnityEngine;

public class StarterPackController : View 
{
    public GameObject OpenButton;
    public UILabel TimeLabel;
    public View StarterPackView;

    private float _tickTime = 0.0f;
    private bool _initializaed;

    public override void Init(GameManager gameManager)
    {
        base.Init(gameManager);

        GameManager.IapManager.OnBuyCurrency += CurrencyByued;

        //if (gameManager.IapManager.IsBuyStarterPack)
        //{
        //    Hide();
        //    return;
        //}

        StarterPackView.Init(GameManager);

        UIEventListener.Get(OpenButton).onClick += OnOpenClick;

        _initializaed = true;
    }

    public override void Hide()
    {
        GameManager.IapManager.OnBuyCurrency -= CurrencyByued;
        base.Hide();
    }

    private void OnOpenClick(GameObject go)
    {
        GameManager.Player.MainHud.ShopPanel.Show(NewShopCategory.Offers);
        //StarterPackView.Show();
    }

    private void CurrencyByued(string id)
    {
        //if(id == IapStoreManager.STARTER_PACK)
        //    Hide();
    }

    void Update()
    {
        if (!_initializaed)
            return;

        //_tickTime += Time.deltaTime;
        //if(_tickTime >= 1.0f)
        //{
        //    TimeLabel.text = GameManager.IapManager.GetStarterPackTime();
        //    _tickTime = 0.0f;
        //}
    }

    void OnDestroy()
    {
        StopAllCoroutines();
    }
}
