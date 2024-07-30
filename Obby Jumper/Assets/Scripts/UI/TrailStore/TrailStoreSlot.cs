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

    [SerializeField] private UnityEvent<TrailInfo> _trailPicked;
    [SerializeField] private Image _image;

    private TrailInfo _trailInfo;
    private int _trailId;

    public void OnClick()
    {
        _trailPicked.Invoke(_trailInfo);
    }

    public void OnTrailPickedId(int trailId)
    {
        if (_trailId != trailId)
        {
            
        }
    }

    public void HidePrice()
    {

    }

    public void InitFrom(TrailInfo trailInfo)
    {
        _trailInfo = trailInfo;
        _trailId = trailInfo.ItemId;
        _image.color = trailInfo.Color;
    }
}
