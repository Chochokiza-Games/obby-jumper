using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pet Item", menuName = "Invetory Items/PetItem", order = 52)]
public class PetItem : BaseInventoryItem 
{
    public GameObject Prefab
    {
        get => _prefab;
    }

    [SerializeField] private GameObject _prefab;
}
