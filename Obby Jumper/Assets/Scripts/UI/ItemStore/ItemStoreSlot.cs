using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ItemStoreSlot : MonoBehaviour
{
    public UnityEvent<ItemInfo> ItemPicked
    {
        get => _itemPicked;
    }

    [Header("Picked Colors")]
    [SerializeField] private Color _bgPickedColor;
    [SerializeField] private Color _innerBgPickedColor;
    [SerializeField] private Color _topPatternPickedColor;
    [SerializeField] private Color _bottomPatternPickedColor;
    [SerializeField] private Color _innerLinePickedColor;
    [Header("Not Picked Colors")]
    [SerializeField] private Color _bgNotPickedColor;
    [SerializeField] private Color _innerBgNotPickedColor;
    [SerializeField] private Color _topPatternNotPickedColor;
    [SerializeField] private Color _bottomPatternNotPickedColor;
    [SerializeField] private Color _innerLineNotPickedColor;
    [Header("Objects To Change Color")]
    [SerializeField] private Image _bg;
    [SerializeField] private Image _innerBg;
    [SerializeField] private Image _topPattern;
    [SerializeField] private Image _bottomPattern;
    [SerializeField] private Image _innerLine;
    [SerializeField] private Image _preview;
    [SerializeField] private TextMeshProUGUI _priceLabel;
    [SerializeField] private UnityEvent<ItemInfo> _itemPicked;

    private ItemInfo _slotInfo;
    private int _itemId;

    public void OnClick()
    {
        _itemPicked.Invoke(_slotInfo);
    }


    public void OnItemPickedId(ItemInfo info)
    {
        if (_slotInfo != info)
        {
            _bg.color = _bgNotPickedColor;
            _innerBg.color = _innerBgNotPickedColor;
            _topPattern.color = _topPatternNotPickedColor;
            _bottomPattern.color = _bottomPatternNotPickedColor;
            _innerLine.color = _innerLineNotPickedColor;
            //_image.sprite = _notPickedSlotSprite;
        }
        else 
        {
            _bg.color = _bgPickedColor;
            _innerBg.color = _innerBgPickedColor;
            _topPattern.color = _topPatternPickedColor;
            _bottomPattern.color = _bottomPatternPickedColor;
            _innerLine.color = _innerLinePickedColor;
        }
    }

    public void HidePrice()
    {
        _priceLabel.gameObject.SetActive(false);
    }

    public void InitFrom(ItemInfo slotInfo)
    {
        _slotInfo = slotInfo;
        _itemId = slotInfo.ItemId;
        _priceLabel.text = slotInfo.Price.ToString();
        _preview.sprite = slotInfo.IconPreview;

        if (slotInfo is TrailInfo)  
        {
            _preview.color = (slotInfo as TrailInfo).Color;
        }
    }
}
