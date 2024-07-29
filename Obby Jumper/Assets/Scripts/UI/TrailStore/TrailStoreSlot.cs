using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TrailStoreSlot : MonoBehaviour
{
    public UnityEvent<TrailInfo> TrailPicked
    {
        get => _trailPicked;
    }

    [SerializeField] private Image _image;
    [SerializeField] private Sprite _pickedSlotSprite;
    [SerializeField] private Sprite _notPickedSlotSprite;
    [SerializeField] private Image _preview;
    [SerializeField] private TextMeshProUGUI _priceLabel;
    [SerializeField] private UnityEvent<TrailInfo> _trailPicked;
    [SerializeField] private GameObject _counter;

    private TrailInfo _trailInfo;
    private int _trailId;

    public void OnClick()
    {
        _image.sprite = _pickedSlotSprite;
        _trailPicked.Invoke(_trailInfo);
    }

    public void OnTrailPickedId(int trailId)
    {
        if (_trailId != trailId)
        {
            _image.sprite = _notPickedSlotSprite;
        }
    }

    public void HidePrice()
    {
        _counter.SetActive(false);
    }

    public void InitFrom(TrailInfo trailInfo)
    {
        _trailInfo = trailInfo;
        _trailId = trailInfo.ItemId;
        _priceLabel.text = trailInfo.Price.ToString();
        _preview.sprite = trailInfo.IconPreview;
    }
}
