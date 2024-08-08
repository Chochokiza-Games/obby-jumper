using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CustomizationStore : MonoBehaviour
{
    public enum Type
    {
        Skin,
        Trail,
        Accessory
    }

    [SerializeField] private ToastComposer _toastComposer;
    [SerializeField] private ItemInfo[] _slotsInformation;
    [SerializeField] private GameObject _slotPrefab;
    [SerializeField] private GameObject _purchaseConfirmation;
    [SerializeField] private GameObject _slotsContainer;
    [SerializeField] private Image _bigSlotPreview;

    [SerializeField] private PlayerProfile _profile;
    [SerializeField] private GameObject _buyButton;
    [SerializeField] private GameObject _equipButton;

    [Header("Changers")]
    [SerializeField] private SkinChanger _skinChanger;
    [SerializeField] private TrailChanger _trailChanger;
    [SerializeField] private AccessoryStation _accessoryStation;

    private Dictionary<ItemInfo, CustomizationStoreSlot> _slots;
    private ItemInfo _pickedSlotInfo = null;
    
    private UnityEvent<ItemInfo> _skinPicked;

    private void Start()
    {
        _skinPicked = new UnityEvent<ItemInfo>();
        _slots = new Dictionary<ItemInfo, CustomizationStoreSlot>();

        foreach (ItemInfo itemInfo in _slotsInformation)
        {
            CustomizationStoreSlot slot = Instantiate(_slotPrefab, _slotsContainer.transform).GetComponent<CustomizationStoreSlot>();
            if (_pickedSlotInfo == null)
            {
                _pickedSlotInfo = itemInfo;
            }
            slot.InitFrom(itemInfo);
            slot.ItemPicked.AddListener(OnItemPicked);
            _slots[itemInfo] = slot;
            switch (_pickedSlotInfo.Type)
            {
                case Type.Skin:
                    if (_profile.IsSkinOpened(itemInfo.ItemId))
                    {
                        slot.HidePrice();
                    }
                    break;
                case Type.Accessory:
                    if (_profile.IsAccessoryOpened(itemInfo.ItemId))
                    {
                        slot.HidePrice();
                    }
                    break;
                case Type.Trail:
                    if (_profile.IsTrailOpened(itemInfo.ItemId))
                    {
                        slot.HidePrice();
                    }
                    break;
            } 
            // if (_profile.IsSkinOpened(itemInfo.ItemId))
            // {
            //     slot.HidePrice();
            // }
            _skinPicked.AddListener(slot.OnItemPickedId);
        }

        OnItemPicked(_pickedSlotInfo);
    }

    public void OnItemPicked(ItemInfo info)
    {
        _pickedSlotInfo = info;
        _skinPicked.Invoke(info);
        _bigSlotPreview.sprite = info.IconPreview;
        _bigSlotPreview.color = Color.white;

        switch (info.Type)
        {
            case Type.Skin:
                if (_profile.IsSkinOpened(info.ItemId))
                {
                    _buyButton.SetActive(false);
                    _equipButton.SetActive(true);
                    _slots[info].HidePrice();
                }
                else
                {
                    _buyButton.SetActive(true);
                    _equipButton.SetActive(false);
                }
                break;
            case Type.Accessory:
                if (_profile.IsAccessoryOpened(info.ItemId))
                {
                    _buyButton.SetActive(false);
                    _equipButton.SetActive(true);
                    _slots[info].HidePrice();
                }
                else
                {
                    _buyButton.SetActive(true);
                    _equipButton.SetActive(false);
                }
                break;
            case Type.Trail:
                _bigSlotPreview.color = (info as TrailInfo).Color;
                if (_profile.IsTrailOpened(info.ItemId))
                {
                    _buyButton.SetActive(false);
                    _equipButton.SetActive(true);
                    _slots[info].HidePrice();
                }
                else
                {
                    _buyButton.SetActive(true);
                    _equipButton.SetActive(false);
                }
                break;
        }
    }

    public void OnEquipButtonUp()
    {
        switch (_pickedSlotInfo.Type)
        {
            case Type.Skin:
                _skinChanger.SetSkin(_pickedSlotInfo.ItemId);
                break;
            case Type.Accessory:
                _accessoryStation.SetAccessory(_pickedSlotInfo.ItemId);
                break;
            case Type.Trail:
                _trailChanger.SetTrail(_pickedSlotInfo.ItemId);
                break;
        } 
    }

    public void OnBuyButtonUp()
    {
        if (_profile.Buy(_pickedSlotInfo.Price))
        {
            _buyButton.SetActive(false);
            _equipButton.SetActive(true);
            _slots[_pickedSlotInfo].HidePrice();
            switch (_pickedSlotInfo.Type)
            {
                case Type.Skin:
                    _skinChanger.SetSkin(_pickedSlotInfo.ItemId);
                    _profile.MarkSkinAsOpened(_pickedSlotInfo.ItemId);
                    break;
                case Type.Accessory:
                    _accessoryStation.SetAccessory(_pickedSlotInfo.ItemId);
                    _profile.MarkAccessoryAsOpened(_pickedSlotInfo.ItemId);
                    break;
                case Type.Trail:
                    _trailChanger.SetTrail(_pickedSlotInfo.ItemId);
                    _profile.MarkTrailAsOpened(_pickedSlotInfo.ItemId);
                    break;
            }
        }
        else
        {
            _toastComposer.ToastSpawn(ToastComposer.Type.StoreErr);
        }
    }
}
