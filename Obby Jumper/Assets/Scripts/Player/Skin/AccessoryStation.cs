using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccessoryStation : MonoBehaviour
{
    public enum AccessoryType
    {
        Hat,
        Wings,
        Max
    }

    [Header("Debug Only")]
    [SerializeField] private AccessoryInfo _debugWings;
    [SerializeField] private AccessoryInfo _debugHat;
    [Space]
    [SerializeField] private AccessoryInfo[] _info;
    [SerializeField] private Transform _hatPlaceholder;
    [SerializeField] private Transform _wingsPlaceholder;
    private GameObject _currentHat;
    private GameObject _currentWings;

    private void Start() 
    {
        FindObjectOfType<PlayerProfile>().InitAccessories(_info.Length);
        if (Debug.isDebugBuild)
        {
            if (_debugHat != null)
            {
                SetAccessory(_debugHat.ItemId);
            }

            if (_debugWings != null)
            {
                SetAccessory(_debugWings.ItemId);
            }
        }
    }


    public void SetAccessory(int id)
    {
        AccessoryInfo info = null;

        foreach (AccessoryInfo i in _info)
        {
            if (i.ItemId == id)
            {
                info = i;
            }
        }

        switch (info.AccessoryType)
        {
            case AccessoryType.Hat:
                SetAccessory(
                    info.Prefab,
                    ref _currentHat,
                    _hatPlaceholder);
                break;
            case AccessoryType.Wings:
                SetAccessory(
                    info.Prefab,
                    ref _currentWings,
                    _wingsPlaceholder);
                break;
        }
    }

    private void SetAccessory(GameObject prefab, ref GameObject existsAccessory, Transform placeholder)
    {
        if (existsAccessory != null)
        {
            Destroy(existsAccessory);
            existsAccessory = null;
        }

        existsAccessory = Instantiate(prefab, placeholder);
    }
}
