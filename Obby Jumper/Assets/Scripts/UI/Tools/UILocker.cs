using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILocker : MonoBehaviour
{
    [SerializeField] private bool _shouldInactiveOnStart = true;

    private PlayerMovement _playerMovement;
    private CameraPivot _cameraPivot;
    private ScreenClickRaycaster _screenClickRaycaster;

    private void Start()
    {
        _playerMovement = FindObjectOfType<PlayerMovement>();
        _cameraPivot = FindObjectOfType<CameraPivot>();
        _screenClickRaycaster = FindObjectOfType<ScreenClickRaycaster>();

        if ( _shouldInactiveOnStart)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        if (_playerMovement != null && _cameraPivot != null)
        {
            _playerMovement.Locked = true;
            _cameraPivot.Locked = true;
            _screenClickRaycaster.Locked = true;
        }
    }

    private void OnDisable()
    {
        if (_playerMovement != null && _cameraPivot != null)
        {
            _playerMovement.Locked = false;
            _cameraPivot.Locked = false;
            _screenClickRaycaster.Locked = false;
        }
    }
}
