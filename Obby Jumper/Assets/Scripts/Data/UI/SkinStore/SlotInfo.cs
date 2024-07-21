using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Slot", menuName = "SkinStore/SlotInfo", order = 51)]

public class SlotInfo : ScriptableObject
{
    public int SkinId
    {
        get => _skinId;
    }
    public int Price
    {
        get => _price ;
    }
    public Sprite IconPreview
    {
        get => _iconPreview;
    }

    [SerializeField] private int _skinId;
    [SerializeField] private int _price;
    [SerializeField] private Sprite _iconPreview;
}