using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SkinStoreSlot : MonoBehaviour
{
    public UnityEvent<ItemInfo> SkinPicked
    {
        get => _skinPicked;
    }

    [SerializeField] private Image _image;
    [SerializeField] private Sprite _pickedSlotSprite;
    [SerializeField] private Sprite _notPickedSlotSprite;
    [SerializeField] private Image _preview;
    [SerializeField] private TextMeshProUGUI _priceLabel;
    [SerializeField] private UnityEvent<ItemInfo> _skinPicked;
    [SerializeField] private GameObject _counter;

    private ItemInfo _slotInfo;
    private int _skinId;

    public void OnClick()
    {
        _image.sprite = _pickedSlotSprite;
        _skinPicked.Invoke(_slotInfo);
    }

    public void OnSkinPickedId(int skinId)
    {
        if (_skinId != skinId)
        {
            _image.sprite = _notPickedSlotSprite;
        }
    }

    public void HidePrice()
    {
        _counter.SetActive(false);
    }

    public void InitFrom(ItemInfo slotInfo)
    {
        _slotInfo = slotInfo;
        _skinId = slotInfo.ItemId;
        _priceLabel.text = slotInfo.Price.ToString();
        _preview.sprite = slotInfo.IconPreview;
    }
}
