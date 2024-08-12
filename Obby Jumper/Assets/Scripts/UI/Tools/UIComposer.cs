using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIComposer : MonoBehaviour
{
    public bool IsSomeUIOpened
    {
        get => _isSomeUIOpened;
    }
    [SerializeField] private GameObject _homeUI;
    [SerializeField] private PlayerMovement _movement;

    private  bool _isSomeUIOpened = false;

    public void OpeningSomeUI()
    {   
        _movement.Lock();
        _isSomeUIOpened = true;
        _homeUI.SetActive(false);
    }
    public void ClosingSomeUI()
    {
        _movement.Unlock();
        _isSomeUIOpened = false;
        _homeUI.SetActive(true);
    }   
}
