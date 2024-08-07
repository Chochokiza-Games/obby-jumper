using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item/Info", order = 51)]

public class ItemInfo : ScriptableObject
{
    public ItemStore.Type Type
    {
        get => _type;
    }
    public int ItemId
    {
        get => _itemId;
    }
    public int Price
    {
        get => _price ;
    }
    public Sprite IconPreview
    {
        get => _iconPreview;
    }

    [SerializeField] private int _itemId;
    [SerializeField] private int _price;
    [SerializeField] private Sprite _iconPreview;
    [SerializeField] private ItemStore.Type _type;
}