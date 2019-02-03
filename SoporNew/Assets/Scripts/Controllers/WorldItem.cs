using UnityEngine;
using System;
using Assets.Scripts.Models;
using Assets.Scripts.Models.ResourceObjects;
using Assets.Scripts;
using Assets.Scripts.Models.MiningObjects.Bushes;
using Assets.Scripts.Models.Food;
using Assets.Scripts.Models.Seedlings.Carrot;
using Assets.Scripts.Models.Seedlings.Corn;
using Assets.Scripts.Models.Seedlings.Pumpkin;

public enum WorldItemType
{
    Log,
    Stone,
    MedicinalPlant,
	Bush,
	Apple,
	Banana,
    BottleWater,
    SeedCarrot,
    SeedPumpkin,
    SeedCorn,
    Manure,
    Soda
}

public class WorldItem : MonoBehaviour
{
    public WorldItemType ObjectType;
    public int Amount;
    public bool SetupByStart = true;
    [Range(0, 200)]
    public float CanGetDistance = 100.0f;

    private Type _typeItem;
    private int? _currentDurability;
    private string _soundName;
    private GameManager _gameManager;
    private float _updateTimer;
    [HideInInspector]
    public float SqrDistToPlayer;

    void Start ()
    {
        if (SetupByStart)
            SetItemType();
    }

    void Update()
    {
        UpdateData();
    }

    private void SetItemType()
    {
        _soundName = WorldConsts.AudioConsts.PickUp2;
        switch (ObjectType)
        {
            case WorldItemType.Log:
                _typeItem = typeof(WoodResource);
                break;
            case WorldItemType.Stone:
                _typeItem = typeof(StoneResource);
                break;
            case WorldItemType.MedicinalPlant:
                _typeItem = typeof(MedicinalPlant);
                _soundName = WorldConsts.AudioConsts.CurBush2;
                break;
			case WorldItemType.Bush:
				_typeItem = typeof(FiberResource);
                _soundName = WorldConsts.AudioConsts.CurBush2;
				break;
			case WorldItemType.Apple:
				_typeItem = typeof(Apple);
				break;
			case WorldItemType.Banana:
				_typeItem = typeof(Banana);
				break;
            case WorldItemType.BottleWater:
                _typeItem = typeof(BottleWater);
                break;
            case WorldItemType.SeedCarrot:
                _typeItem = typeof(CarrotSeed);
                break;
            case WorldItemType.SeedPumpkin:
                _typeItem = typeof(PumpkinSeed);
                break;
            case WorldItemType.SeedCorn:
                _typeItem = typeof(CornSeed);
                break;
            case WorldItemType.Manure:
                _typeItem = typeof(Manure);
                break;
            case WorldItemType.Soda:
                _typeItem = typeof(Soda);
                break;
        }
    }

    public void SetItem(Type type, int amount, int? currentDurability)
    {
        _typeItem = type;
        Amount = amount;
        _currentDurability = currentDurability;
    }

    public void Get(GameManager gameManager)
    {
		GetItem (gameManager);
    }

	void OnMouseDown() 
	{
		GetItem();
	}

	private void GetItem(GameManager gameManager = null)
	{
        if(_gameManager == null)
            _gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();

        if (_gameManager == null)
            return;

        var playerDistance = (transform.localPosition - _gameManager.Player.transform.localPosition).sqrMagnitude;
        if (playerDistance > CanGetDistance)
            return;

		var item = HolderObjectFactory.GetItem(_typeItem, Amount, _currentDurability);
		if(_gameManager.PlayerModel.Inventory.AddItem(item))
        {
            _gameManager.Player.MainHud.ShowAddedResource(item.Item.IconName, item.Amount, item.Item.LocalizationName);
            _gameManager.PlacementItemsController.RemovePlacedItem(gameObject);
            gameObject.SetActive(false);
            SoundManager.PlaySFX(_soundName);
        }
        else
        {
            _gameManager.Player.MainHud.ShowHudText(Localization.Get("no_place_in_inventory"), HudTextColor.Red);
        }
	}

    protected virtual void UpdateData()
    {
        if (_updateTimer <= 0)
        {
            if (_gameManager == null)
                _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

            SqrDistToPlayer = (_gameManager.Player.transform.localPosition - transform.localPosition).sqrMagnitude;
            if (SqrDistToPlayer < 40.0f)
            {
                _gameManager.Player.UpdateNearWorldItem(SqrDistToPlayer, this);
            }

            _updateTimer = 0.3f;
        }
        _updateTimer -= Time.deltaTime;
    }
}
