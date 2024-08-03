using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Inventory : MonoBehaviour
{
    [SerializeField] private RectTransform _rTransform;
    [SerializeField] private GameObject _slotPrefab;
    [SerializeField] private GameObject _grid;
    [SerializeField] private PlayerInventory _linkedInventory;
    [SerializeField] private UnityEvent<int> _itemPickedFromPopup;
    [SerializeField] private GameObject _popup;
    [SerializeField] private Vector2 _popupOffsetPercent;
    [SerializeField] private bool _canDeleteAllItems;
    [SerializeField] private GameObject _errorToastPrefab;
    [SerializeField] private TextMeshProUGUI _sizeInfo;

    private Dictionary<int, InventorySlot> _slots = new Dictionary<int, InventorySlot>();
    private InventorySlot _pickedSlot;
    private GameObject _toast;
    private Vector3 _popupOffset;

    private void Start()
    {
        if (_popup != null)
        {
            _popupOffset = new Vector2(_rTransform.rect.width * -(_popupOffsetPercent.x / 100f), _rTransform.rect.height * (_popupOffsetPercent.y / 100f));
        }

        UpdateSizeInfo();
    }

    private void OnEnable()
    {
        if (_popup != null)
        {
            _popup.SetActive(false);
        }

        foreach (var slot in _slots)
        {
            slot.Value.Picked = false;
        }
    }

    private void UpdateSizeInfo()
    {
        _sizeInfo.text = $"{_linkedInventory.BucketSize}/{_linkedInventory.BucketCap}";
    }

    public void OnItemAdded(int id, BaseInventoryItem item)
    {
        UpdateSizeInfo();
        InventorySlot slot = Instantiate(_slotPrefab, _grid.transform).GetComponent<InventorySlot>();

        if (_popup != null)
        {
            slot.PickedId.AddListener(OnItemPicked);
        }
        
        slot.Initialize(id, item.Icon);

        _slots[id] = slot;
    }

    public void MarkAsPicked(int id)
    {
        foreach (KeyValuePair<int, InventorySlot> kv in _slots)
        {
            kv.Value.MarkAsUnpicked();
        }

        _slots[id].MarkAsPicked();
    }

    public void OnItemPicked(int id)
    {
        _popup.SetActive(true);
        _pickedSlot = _slots[id];
        _popup.transform.position = _pickedSlot.transform.position + _popupOffset;
    }

    public void OnItemPickedPopup()
    {
        _itemPickedFromPopup.Invoke(_pickedSlot.Id);
    }

    public void OnItemRemoved(int id, BaseInventoryItem item)
    {
        UpdateSizeInfo();
        if (_popup != null)
        {
            _popup.SetActive(false);
        }

        if (!_slots.ContainsKey(id))
        {
            Debug.LogError($"Inventory Front OnItemRemoved: slot {id} not exists");
        }
        Destroy(_slots[id].gameObject);
        _slots.Remove(id);
    }

    private void OnDisable()
    {
        if (_toast != null)
        {
            Destroy(_toast);
            _toast = null;
        }
    }


    public void OnDeleteItems()
    {
        List<int> idsToDelete = new List<int>();
        foreach (var kv in  _slots)
        {
            InventorySlot slot = kv.Value;
            if (slot.Picked)
            {
                idsToDelete.Add(kv.Key);
            }
        }

        if ((_slots.Count == 1 || idsToDelete.Count == _slots.Count) && !_canDeleteAllItems)
        {
            if (_errorToastPrefab != null) 
            {
                _toast = Instantiate(_errorToastPrefab, transform);
            }
            return;
        }

        foreach (int id in idsToDelete)
        {
            _linkedInventory.RemoveById(id);
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
}
