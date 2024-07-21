using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public bool Picked
    {
        get => _picked;
        set
        {
            _picked = value;
            _mark.gameObject.SetActive(value);
        }
    }

    public int Id
    {
        get => _id;
    }

    public UnityEvent<int> PickedId
    {
        get => _pickedId;
    }

    [SerializeField] private Image _bg;
    [SerializeField] private Image _icon;
    [SerializeField] private Image _mark;
    [SerializeField] private TextMeshProUGUI _additionalInfoLabel;
    [SerializeField] private Sprite _pickedBg;
    [SerializeField] private Sprite _unpickedBg;

    private UnityEvent<int> _pickedId = new UnityEvent<int>();
    private bool _picked = false;
    private int _id;

    public void Initialize(int id, Sprite preview)
    {
        _icon.sprite = preview;
        _id = id;
        _additionalInfoLabel.gameObject.SetActive(false);
    }

    public void Initialize(int id, Sprite preview, string additionalText)
    {
        _icon.sprite = preview;
        _id = id;
        _additionalInfoLabel.text = additionalText;
    }
    
    public void OnPick()
    {
        _picked = !_picked;
        _pickedId.Invoke(_id);
        _mark.gameObject.SetActive(_picked);
    }

    public void MarkAsPicked()
    {
        _bg.sprite = _pickedBg;
    }

    public void MarkAsUnpicked()
    {
        _bg.sprite = _unpickedBg;
    }
}
