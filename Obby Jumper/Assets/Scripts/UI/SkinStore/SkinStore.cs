using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SkinStore : MonoBehaviour
{
    [SerializeField] private SlotInfo[] _slotsInformation;
    [SerializeField] private GameObject _slotPrefab;
    [SerializeField] private GameObject _purchaseConfirmationPrefab;
    [SerializeField] private GameObject _toastPrefab;
    [SerializeField] private GameObject _slotsContainer;
    [SerializeField] private Image _bigSlotPreview;
    [SerializeField] private float toastSpawnDelay;
    [SerializeField] private PlayerProfile _profile;
    [SerializeField] private SkinChanger _skinChanger;
    [SerializeField] private GameObject _buyButton;
    [SerializeField] private GameObject _equipButton;

    private SlotInfo _pickedSlotInfo = null;
    private UnityEvent<int> _skinPicked;
    private Dictionary<int, SkinStoreSlot> _slots;

    private GameObject _toast;

    private void Start()
    {
        _skinPicked = new UnityEvent<int>();
        _slots = new Dictionary<int, SkinStoreSlot>();

        foreach (SlotInfo slotInfo in _slotsInformation)
        {
            SkinStoreSlot slot = Instantiate(_slotPrefab, _slotsContainer.transform).GetComponent<SkinStoreSlot>();
            if (_pickedSlotInfo == null)
            {
                _pickedSlotInfo = slotInfo;
            }
            slot.InitFrom(slotInfo);
            slot.SkinPicked.AddListener(OnSkinPicked);
            _slots[slotInfo.SkinId] = slot;
            if (_profile.IsSkinOpened(slotInfo.SkinId))
            {
                slot.HidePrice();
            }
            _skinPicked.AddListener(slot.OnSkinPickedId);
        }

        OnSkinPicked(_pickedSlotInfo);
    }

    private void OnDisable()
    {
        if (_toast != null)
        {
            Destroy(_toast);
            _toast = null;
        }
    }

    public void OnSkinPicked(SlotInfo info)
    {
        _pickedSlotInfo = info;
        _skinPicked.Invoke(info.SkinId);
        _bigSlotPreview.sprite = info.IconPreview;
        if (_profile.IsSkinOpened(info.SkinId))
        {
            _buyButton.SetActive(false);
            _equipButton.SetActive(true);
            _slots[info.SkinId].HidePrice();
        }
        else
        {
            _buyButton.SetActive(true);
            _equipButton.SetActive(false);
        }
    }

    public void OnEquipButtonUp()
    {
        _skinChanger.SetSkin(_pickedSlotInfo.SkinId);   
    }

    public void TryBuy()
    {
        if (_profile.CanBuy(_pickedSlotInfo.Price)) 
        {
            _purchaseConfirmationPrefab.SetActive(true);
        }
        else
        {
            _toast = Instantiate(_toastPrefab, transform);
        }
    }

    public void OnBuyButtonUp()
    {
        if (_profile.Buy(_pickedSlotInfo.Price))
        {
            _buyButton.SetActive(false);
            _equipButton.SetActive(true);
            _skinChanger.SetSkin(_pickedSlotInfo.SkinId);
            _profile.MarkSkinAsOpened(_pickedSlotInfo.SkinId);
            _slots[_pickedSlotInfo.SkinId].HidePrice();
        }
    }
}
