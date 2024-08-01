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

    private  bool _isSomeUIOpened = false;

    public void OpeningSomeUI()
    {   
        _isSomeUIOpened = true;
        HideHomeUI();
    }
    public void ClosingSomeUI()
    {
        _isSomeUIOpened = false;
        ShowHomeUI();
    }

    private void HideHomeUI()
    {
        _homeUI.SetActive(false);
    }
    private void ShowHomeUI()
    {
        _homeUI.SetActive(true);
    }
}
