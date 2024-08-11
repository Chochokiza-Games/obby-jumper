using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;
using YG;

public class PlayerProfile : MonoBehaviour
{
    public int CurrentLevel
    {
        get => _currentLevel;
    }

    public int[] PetDropOrder
    {
        get => _petDropOrder;
    }

    public int PetDropOrderCurrentId
    {
        get => _petDropOrderCurrentId;
    }
    public int CurrentSkinId
    {
        get => _currentSkinId;
        set => _currentSkinId = value;
    }

    public int CurrentTrailId
    {
        get => _currentTrailId;
        set => _currentTrailId = value;
    }

    public int Power
    {
        get 
        {
            return _power;
        }
    }

    [SerializeField] private UnityEvent<int> _moneyChanged;
    [SerializeField] private UnityEvent<int> _powerChanged;
    [SerializeField] private int _powerCap;
    [SerializeField] private int[] _petDropOrder;
    [SerializeField] private PlayerInventory _petInventory;
    [SerializeField] private PlayerInventory _petEggsInventory;
    [SerializeField] private UnityEvent _saveEvent;
    [SerializeField] private UnityEvent _loadEvent;
    [SerializeField] private float _saveDelay = 20f;
    [SerializeField] private UnityEvent<int> _levelChanged;

    private int _currentSkinId = 0;
    private int _currentTrailId = 0;
    private int _money = 0;
    private int _power = 500;
    private int _currentLevel = 1;

    private bool[] _openedTrails;
    private bool[] _openedSkins;
    private bool[] _openedAccessories;

    private int _petDropOrderCurrentId = 0;
    private bool _shouldChangeLevel = false;


    private void OnEnable() => YandexGame.GetDataEvent += LoadCloud;

    private void OnDisable() => YandexGame.GetDataEvent -= LoadCloud;

    public bool RunOnMobile() 
    {
#if !UNITY_EDITOR && UNITY_WEBGL
            return IsMobile();
#endif
        return false;
    }


    [DllImport("__Internal")]
    private static extern bool IsMobile();

    private void SaveInventory(PlayerInventory inventory, ref int[] array)
    {
        BaseInventoryItem[] items = inventory.GetAllItems();

        List<int> itemsToSave = new List<int>();

        foreach (BaseInventoryItem item in items)
        {
            itemsToSave.Add(item.Id);
        }

        array = itemsToSave.ToArray();
    }

    public void LoadCloud()
    {
        YandexGame.ResetSaveProgress();

        _loadEvent.Invoke();

        _money = YandexGame.savesData.money = 0;
        _power = YandexGame.savesData.power;

        _currentLevel = YandexGame.savesData.level = 1;

        _petDropOrderCurrentId = YandexGame.savesData.petDropOrderCurrentId;

        _moneyChanged.Invoke(_money);
        _powerChanged.Invoke(_power);

        //LoadInventory(_petInventory, ref YandexGame.savesData.petInventoryItems);
        //LoadInventory(_petEggsInventory, ref YandexGame.savesData.petEggsInventoryItems);

        FindObjectOfType<LanguageTranslator>().InitLanguage(YandexGame.lang = "ru");

        _petInventory.PushItem(BaseInventoryItem.ItemId.PetCat);
        _petInventory.PushItem(BaseInventoryItem.ItemId.PetBear);
        _petInventory.PushItem(BaseInventoryItem.ItemId.PetBunny);
        _petInventory.PushItem(BaseInventoryItem.ItemId.PetPumkin);
        _petInventory.PushItem(BaseInventoryItem.ItemId.PetBurger);
        _petInventory.PushItem(BaseInventoryItem.ItemId.PetDemon);
        //_petEggsInventory.PushItem(BaseInventoryItem.ItemId.Egg);
    }

    private void LoadInventory(PlayerInventory inventory, ref int[] array)
    {
        foreach (int i in array)
        {
            inventory.PushItem((BaseInventoryItem.ItemId)(i));
        }
    }

    public void SaveCloud()
    {
        _saveEvent.Invoke();
        YandexGame.savesData.level = _currentLevel;
        YandexGame.savesData.money = _money;
        YandexGame.savesData.power = _power;
        YandexGame.savesData.petDropOrderCurrentId = _petDropOrderCurrentId;
        YandexGame.savesData.openedSkins = _openedSkins;
        YandexGame.savesData.openedAccessories = _openedAccessories;
        YandexGame.savesData.openedTrails = _openedTrails;

        SaveInventory(_petInventory, ref YandexGame.savesData.petInventoryItems);
        SaveInventory(_petEggsInventory, ref YandexGame.savesData.petEggsInventoryItems);

        YandexGame.SaveProgress();
    }
    private IEnumerator SaveRoutine() 
    {
        while (true) 
        {
            yield return new WaitForSeconds(_saveDelay);

            SaveCloud();
        }
    }

    private void Awake()
    {
        StartCoroutine(SaveRoutine());
    }

    public void InitSkins(int skinsCount)
    {
        _openedSkins = YandexGame.savesData.openedSkins;
        if (_openedSkins.Length == 0)
        {
            _openedSkins = new bool[skinsCount];
            for (int i = 0; i < skinsCount; i++)
            {
                _openedSkins[i] = false;
            }

            _openedSkins[0] = true;
        }
    }
    public void InitTrails(int trailsCount)
    {
        _openedTrails = YandexGame.savesData.openedTrails;
        if (_openedTrails.Length == 0)
        {
            _openedTrails = new bool[trailsCount];
            for (int i =0; i < trailsCount; i++)
            {
                _openedTrails[i] = false;
            }
            _openedTrails[0] = true;
        }
    }

    public void InitAccessories(int accessoriesCount)
    {
        _openedAccessories = YandexGame.savesData.openedAccessories;
        if (_openedAccessories.Length == 0)
        {
            _openedAccessories = new bool[accessoriesCount];
            for (int i =0; i < accessoriesCount; i++)
            {
                _openedAccessories[i] = false;
            }
            _openedAccessories[0] = true;
        }
    }

    public void ShouldChangeLevel()
    {
        _shouldChangeLevel = true;
    }

    public void TryChangeLevel()
    {
        if (_shouldChangeLevel)
        {
            IncreaseLevel();
            _shouldChangeLevel = false;
        }
    }

    public void IncreaseLevel()
    {
        _currentLevel++;
        _power = 10;
        _powerChanged.Invoke(_power);
        _levelChanged.Invoke(_currentLevel);
        _petEggsInventory.PushItem(BaseInventoryItem.ItemId.Egg);
        SaveCloud();

    }

    public void MarkSkinAsOpened(int id)
    {
        _openedSkins[id] = true;
    }

    public void MarkAccessoryAsOpened(int id)
    {
        _openedAccessories[id] = true;
    }

    public void MarkTrailAsOpened(int id)
    {
        _openedTrails[id] = true;
    }

    public void IncreaseMoney(int amount)
    {
        _money += amount;
        _moneyChanged.Invoke(_money);
    }

    public void IncreasePower(int amount)
    {
        _power += amount;
        if (_power > _powerCap)
        {
            _power = _powerCap;
        }
        _powerChanged.Invoke(_power);
    }
    public void DecreaseMoney(int amount)
    {
        _money -= amount;
        if (_money < 0)
        {
            _money = 0;
        }

        _moneyChanged.Invoke(_money);
    }

    public bool CanBuy(int price)
    {
        return _money >= price;
    }

    public bool Buy(int price)
    {
        if (_money >= price)
        {
            DecreaseMoney(price);
            return true;
        }

        return false;
    }

    public bool IsSkinOpened(int id)
    {
        return _openedSkins[id];
    }

    public bool IsAccessoryOpened(int id)
    {
        return _openedAccessories[id];
    }

    public bool IsTrailOpened(int id)
    {
        return _openedTrails[id];
    }

    public void IncreasePetDropOrderCurrentId()
    {
        _petDropOrderCurrentId += 1;
    }
}
