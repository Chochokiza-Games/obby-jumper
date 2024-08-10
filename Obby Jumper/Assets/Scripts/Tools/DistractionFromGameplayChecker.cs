using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistractionFromGameplayChecker : MonoBehaviour
{
    [SerializeField] private UIComposer _composer;
    [SerializeField] private LoadingScreen _loadingScreen;
    [SerializeField] private Education _education;

    private bool _playerInHub = true;

    public void OnFlyStarted()
    {
        _playerInHub = false;
    }

    public void OnFlyEnded()
    {
        _playerInHub = true;
    }

    public bool CanDistractFromGameplay()
    {
        return _loadingScreen.Opened == false
        && _composer.IsSomeUIOpened == false
        && _education.EducationInProgress == false
        && _playerInHub == true;
    }
}
