using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TrailStore : MonoBehaviour
{
    [SerializeField] private TrailInfo[] _slotsInformation;
    [SerializeField] private GameObject _slotPrefab;
    [SerializeField] private GameObject _toastPrefab;
    [SerializeField] private GameObject _slotsContainer;
    [SerializeField] private GameObject _purchaseConfirmationWindow;
    [SerializeField] private Image _bigSlotPreview;
    [SerializeField] private float toastSpawnDelay;
    [SerializeField] private PlayerProfile _profile;
    [SerializeField] private TrailChanger _trailChanger;
    [SerializeField] private GameObject _buyButton;
    [SerializeField] private GameObject _equipButton;

    private TrailInfo _pickedItemInfo = null;
    private UnityEvent<int> _trailPicked;
    private Dictionary<int, TrailStoreSlot> _slots;

    private GameObject _toast;

    private void Start()
    {
        _trailPicked = new UnityEvent<int>();
        _slots = new Dictionary<int, TrailStoreSlot>();

        foreach (TrailInfo trailInfo in _slotsInformation)
        {
            TrailStoreSlot slot = Instantiate(_slotPrefab, _slotsContainer.transform).GetComponent<TrailStoreSlot>();
            if (_pickedItemInfo == null)
            {
                _pickedItemInfo = trailInfo;
            }
            slot.InitFrom(trailInfo);
            slot.TrailPicked.AddListener(OnTrailPicked);
            _slots[trailInfo.ItemId] = slot;
        }

        OnTrailPicked(_pickedItemInfo);
    }

    private void OnDisable()
    {
        if (_toast != null)
        {
            Destroy(_toast);
            _toast = null;
        }
    }

    public void OnTrailPicked(TrailInfo info)
    {
        _pickedItemInfo = info;
        _trailPicked.Invoke(info.ItemId);
        _bigSlotPreview.color = info.Color;
        if (_profile.IsTrailOpened(info.ItemId))
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
        _trailChanger.SetTrail(_pickedItemInfo.ItemId);   
    }

    public void TryBuy()
    {
        if (_profile.CanBuy(_pickedItemInfo.Price)) 
        {
            _purchaseConfirmationWindow.SetActive(true);
        }
        else
        {
            _toast = Instantiate(_toastPrefab, transform);
        }
    }

    public void OnBuyButtonUp()
    {
        if (_profile.Buy(_pickedItemInfo.Price))
        {
            _buyButton.SetActive(false);
            _equipButton.SetActive(true);
            _trailChanger.SetTrail(_pickedItemInfo.ItemId);
            _profile.MarkTrailAsOpened(_pickedItemInfo.ItemId);
        }
    }
}
