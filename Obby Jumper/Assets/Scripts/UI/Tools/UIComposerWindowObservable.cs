using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class UIComposerWindowObservable : MonoBehaviour
{
    private UIComposer _uiComposer;
    
    private void Awake()
    {
        _uiComposer = FindObjectOfType<UIComposer>();
    }
    private void OnEnable()
    {
        _uiComposer.OpeningSomeUI();
    }
    private void OnDisable()
    {
        _uiComposer.ClosingSomeUI();
    }
}
