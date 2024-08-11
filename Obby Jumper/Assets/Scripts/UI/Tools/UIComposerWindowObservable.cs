using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class UIComposerWindowObservable : MonoBehaviour
{
    private UIComposer _uiComposer;
    
    // private void OnEnable()
    // {
    //     if (_uiComposer == null)
    //     {
    //         _uiComposer = FindObjectOfType<UIComposer>();
    //     }

    //     _uiComposer.OpeningSomeUI();
    // }


    private void Update()
    {
        if (_uiComposer == null)
        {
            _uiComposer = FindObjectOfType<UIComposer>();
        }
        
        _uiComposer.OpeningSomeUI();
    }
    private void OnDisable()
    {
        _uiComposer.ClosingSomeUI();
    }
}
