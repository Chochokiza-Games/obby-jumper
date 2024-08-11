using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AccessoryInfo", menuName = "Accessories/Info", order = 52)]
public class AccessoryInfo : ItemInfo 
{
    public AccessoryStation.AccessoryType AccessoryType
    {
        get => _accessoryType;
    }

    public GameObject Prefab
    {
        get => _prefab;
    }

    [SerializeField] private GameObject _prefab;
    [SerializeField] private AccessoryStation.AccessoryType _accessoryType;
}