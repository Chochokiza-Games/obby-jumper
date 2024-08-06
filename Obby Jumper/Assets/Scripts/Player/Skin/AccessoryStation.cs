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
    [SerializeField] private Transform _hatPlaceholder;
    [SerializeField] private Transform _wingsPlaceholder;
    [SerializeField] private GameObject _currentHat;
    [SerializeField] private GameObject _currentWings;

    private void Start() 
    {
        if (Debug.isDebugBuild)
        {
            if (_debugHat != null)
            {
                SetAccessory(_debugHat);
            }

            if (_debugWings != null)
            {
                SetAccessory(_debugWings);
            }
        }
    }


    public void SetAccessory(AccessoryInfo info)
    {
        switch (info.Type)
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
