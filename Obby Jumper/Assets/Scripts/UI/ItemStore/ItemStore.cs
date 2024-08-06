using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ItemStore : MonoBehaviour
{
    [SerializeField] protected ItemInfo[] _slotsInformation;
    [SerializeField] protected GameObject _slotPrefab;
    [SerializeField] protected GameObject _purchaseConfirmation;
    [SerializeField] protected GameObject _slotsContainer;
    [SerializeField] protected Image _bigSlotPreview;

    [SerializeField] protected PlayerProfile _profile;
    [SerializeField] protected GameObject _buyButton;
    [SerializeField] protected GameObject _equipButton;

    protected Dictionary<int, ItemStoreSlot> _slots;
    protected ItemInfo _pickedSlotInfo = null;
    
    protected UnityEvent<int> _skinPicked;

    private void Start()
    {
        _skinPicked = new UnityEvent<int>();
        _slots = new Dictionary<int, ItemStoreSlot>();

        foreach (ItemInfo itemInfo in _slotsInformation)
        {
            ItemStoreSlot slot = Instantiate(_slotPrefab, _slotsContainer.transform).GetComponent<ItemStoreSlot>();
            if (_pickedSlotInfo == null)
            {
                _pickedSlotInfo = itemInfo;
            }
            slot.InitFrom(itemInfo);
            slot.ItemPicked.AddListener(OnItemPicked);
            _slots[itemInfo.ItemId] = slot;
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
        _skinPicked.Invoke(info.ItemId);
        _bigSlotPreview.sprite = info.IconPreview;
    }

    public void OnBuyButtonUp()
    {
        if (_profile.Buy(_pickedSlotInfo.Price))
        {
            _buyButton.SetActive(false);
            _equipButton.SetActive(true);
            //_skinChanger.SetSkin(_pickedSlotInfo.ItemId);
            //_profile.MarkSkinAsOpened(_pickedSlotInfo.ItemId);
            _slots[_pickedSlotInfo.ItemId].HidePrice();
        }
    }
}
