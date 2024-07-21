using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Store Product", menuName = "Store Product", order = 51)]
public class StoreProduct : ScriptableObject
{
    public int Price
    {
        get => _price;
        set => _price = value;
    }

    public Store.StoreProducts Id
    {
        get => _id;
    }

    [SerializeField] private int _price;
    [SerializeField] private Store.StoreProducts _id;
}