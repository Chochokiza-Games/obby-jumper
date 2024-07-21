using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CameraPivot : MonoBehaviour
{
    public bool Locked
    {
        set 
        {
            _locked = value;
            if (_locked)
            {
                _camera.m_YAxis.m_MaxSpeed = 0;
                _camera.m_XAxis.m_MaxSpeed = 0;
            }
        }
    }


    [SerializeField] private float _rotationSpeed;
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private CinemachineFreeLook _camera;
    [SerializeField] private EventSystem _eventSystem;

    private bool _locked;
    private bool _isMobile = false;
    private int _mouseButton = 1;

    private Vector2 _axisSensitivity;

    private void Start()
    {
        _isMobile = FindObjectOfType<PlayerProfile>().RunOnMobile();
        _mouseButton = _isMobile ? 0 : 1;

        _axisSensitivity.y = _camera.m_YAxis.m_MaxSpeed;
        _axisSensitivity.x = _camera.m_XAxis.m_MaxSpeed;

        _camera.m_YAxis.m_MaxSpeed = 0;
        _camera.m_XAxis.m_MaxSpeed = 0;
    }

    private void Update()
    {
        if (_locked)
        {
            return;
        }

        _playerMovement.CameraForwardDirection = _playerMovement.transform.position - new Vector3(transform.position.x, _playerMovement.transform.position.y, transform.position.z);

        if (!_isMobile)
        {
            if (Input.GetMouseButtonDown(1) && !_eventSystem.IsPointerOverGameObject())
            {
                _camera.m_YAxis.m_MaxSpeed = _axisSensitivity.y;
                _camera.m_XAxis.m_MaxSpeed = _axisSensitivity.x;
            }

            if (Input.GetMouseButtonUp(1))
            {
                _camera.m_YAxis.m_MaxSpeed = 0;
                _camera.m_XAxis.m_MaxSpeed = 0;
            }

        }

        /*        if (Input.mouseScrollDelta.magnitude > 0)
                {
                    _cameraOffset.m_Offset.z += Input.mouseScrollDelta.y * _zoomSensitivity * Time.deltaTime;
                    _cameraOffset.m_Offset.z = Mathf.Clamp(_cameraOffset.m_Offset.z, _maxOffset, _minOffset);
                }*/
    }
   
}
