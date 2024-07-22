using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;
using YG;

public class PlayerProfile : MonoBehaviour
{
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

    public int BagUpgradesCount
    {
        get => _bagUpgradesBought;
    }

    public int Power
    {
        get 
        {
            return _power;
        }
    }

    public int EducationShowCountCurrent
    {
        get => _educationShowCountCurrent;
    }

    public int EducationShowCountMax
    {
        get => _educationShowCountMax;
    }

    [SerializeField] private UnityEvent<int> _moneyChanged;
    [SerializeField] private UnityEvent<int> _powerChanged;
    [SerializeField] private int[] _petDropOrder;
    [SerializeField] private PlayerInventory _petInventory;
    [SerializeField] private UnityEvent _saveEvent;
    [SerializeField] private UnityEvent _loadEvent;
    [SerializeField] private int _educationShowCountMax;
    [SerializeField] private float _saveDelay = 20f;
    [SerializeField] private GameObject _skinStoreToastEducationPrefab;
    [SerializeField] private UnityEvent _skinStoreToastOpened;
    [SerializeField] private Transform _field;
    [SerializeField] private SlotInfo _firstSkin;

    private int _currentSkinId = 0;
    private int _money = 0;
    private int _power = 10;
    private int _bagUpgradesBought = 0;

    private bool[] _openedSkins;

    private int _petDropOrderCurrentId = 0;
    private int _educationShowCountCurrent = 0;

    private GameObject _skinStoreToastEducation;
    private GameObject _storeToastEducation;
    private bool _skinStoreToastEducationShowed = false;
    private bool _storeToastEducationShowed = false;

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
        _loadEvent.Invoke();

        _money = YandexGame.savesData.money;
        _power = YandexGame.savesData.power;
        _bagUpgradesBought = YandexGame.savesData.bagUpgradesBought;

        _petDropOrderCurrentId = YandexGame.savesData.petDropOrderCurrentId;

        _educationShowCountCurrent = YandexGame.savesData.educationPassedCount;
        _skinStoreToastEducationShowed = YandexGame.savesData.skinStoreToastEducationShowed;
        _storeToastEducationShowed = YandexGame.savesData.storeToastEducationShowed;

        _moneyChanged.Invoke(_money);
        _powerChanged.Invoke(_power);

        LoadInventory(_petInventory, ref YandexGame.savesData.petInventoryItems);

        FindObjectOfType<LanguageTranslator>().InitLanguage(YandexGame.lang);
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

        YandexGame.savesData.money = _money;
        YandexGame.savesData.power = _power;
        YandexGame.savesData.bagUpgradesBought = _bagUpgradesBought;
        YandexGame.savesData.petDropOrderCurrentId = _petDropOrderCurrentId;
        YandexGame.savesData.openedSkins = _openedSkins;

        YandexGame.savesData.educationPassedCount = _educationShowCountCurrent;
        YandexGame.savesData.skinStoreToastEducationShowed = _skinStoreToastEducationShowed;
        YandexGame.savesData.storeToastEducationShowed = _storeToastEducationShowed;

        SaveInventory(_petInventory, ref YandexGame.savesData.petInventoryItems);

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

    public void UpgradeBag()
    {
        _bagUpgradesBought++;
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
    public void MarkSkinAsOpened(int id)
    {
        _openedSkins[id] = true;
    }

    public void IncreaseMoney(int amount)
    {
        _money += amount;
        _moneyChanged.Invoke(_money);

        if (_money >= _firstSkin.Price && !_skinStoreToastEducationShowed)
        {
            if (_skinStoreToastEducation == null)
            {
                _skinStoreToastEducation = Instantiate(_skinStoreToastEducationPrefab, _field);
                _skinStoreToastOpened.Invoke();
                _skinStoreToastEducationShowed = true;
            }
        }
    }

    public void IncreasePower(int amount)
    {
        _power += amount;
        _powerChanged.Invoke(_power);
    }

    public void DeleteSkinStoreEducationTost()
    {
        if (_skinStoreToastEducation != null)
        {
            Destroy(_skinStoreToastEducation);
            _skinStoreToastEducation = null;
        }
    }

    public void DeleteStoreEducationTost()
    {
        if (_storeToastEducation != null)
        {
            Destroy(_storeToastEducation);
            _storeToastEducation = null;
        }
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


    public void IncreasePetDropOrderCurrentId()
    {
        _petDropOrderCurrentId += 1;
    }


    public void IncreaseEducationShowCount()
    {
        _educationShowCountCurrent += 1;
    }
}
