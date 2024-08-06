using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SkinStore : ItemStore
{
    [SerializeField] private SkinChanger _skinChanger;

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
            if (_profile.IsSkinOpened(itemInfo.ItemId))
            {
                slot.HidePrice();
            }
            _skinPicked.AddListener(slot.OnItemPickedId);
        }

        OnItemPicked(_pickedSlotInfo);
    }

    public new void OnItemPicked(ItemInfo info)
    {
        base.OnItemPicked(info);
        if (_profile.IsSkinOpened(info.ItemId))
        {
            _buyButton.SetActive(false);
            _equipButton.SetActive(true);
            _slots[info.ItemId].HidePrice();
        }
        else
        {
            _buyButton.SetActive(true);
            _equipButton.SetActive(false);
        }
    }

    public void OnEquipButtonUp()
    {
        _skinChanger.SetSkin(_pickedSlotInfo.ItemId);   
    }

    public new void OnBuyButtonUp()
    {
        if (_profile.Buy(_pickedSlotInfo.Price))
        {
            _buyButton.SetActive(false);
            _equipButton.SetActive(true);
            _skinChanger.SetSkin(_pickedSlotInfo.ItemId);
            _profile.MarkSkinAsOpened(_pickedSlotInfo.ItemId);
            _slots[_pickedSlotInfo.ItemId].HidePrice();
        }
    }

}
