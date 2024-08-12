using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using YG;

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

    public float SensitivityCoef
    {
        get => _sensitivityCoef;
    }

    [SerializeField] private float _rotationSpeed;
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private CinemachineFreeLook _camera;
    [SerializeField] private EventSystem _eventSystem;
    [SerializeField] private Vector2 _defaultValues;
    [SerializeField] private float _sensitivityCoef  = 1.2f;

    private bool _locked;
    private bool _isMobile = false;

    private Vector2 _axisSensitivity;

    private void Start()
    {
        _isMobile = FindObjectOfType<PlayerProfile>().RunOnMobile();


        _axisSensitivity.y = _camera.m_YAxis.m_MaxSpeed * _sensitivityCoef;
        _axisSensitivity.x = _camera.m_XAxis.m_MaxSpeed * _sensitivityCoef;

        _camera.m_YAxis.m_MaxSpeed = 0;
        _camera.m_XAxis.m_MaxSpeed = 0;
    }

    public void ReturnToDefault()
    {
        _camera.m_XAxis.Value = _defaultValues.x;
        _camera.m_YAxis.Value = _defaultValues.y;
    }

    public void SetSensetivityCoef(float value)
    {
        _sensitivityCoef = Mathf.Clamp(value, 0.4f, 3);
    }
    public void SetDamping(float value)
    {
        for(int i = 0; i < 3; i++) 
        {
            _camera.GetRig(i).GetCinemachineComponent<CinemachineTransposer>().m_XDamping = value;
            _camera.GetRig(i).GetCinemachineComponent<CinemachineTransposer>().m_YDamping = value;
            _camera.GetRig(i).GetCinemachineComponent<CinemachineTransposer>().m_ZDamping = value;
        }
    }
    public void OnSaveEvent()
    {
        YandexGame.savesData.sensitivityCoef = _sensitivityCoef;
    }

    public void OnLoadEvent()
    {
        _sensitivityCoef = YandexGame.savesData.sensitivityCoef;
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
                _camera.m_YAxis.m_MaxSpeed = _axisSensitivity.y * _sensitivityCoef;
                _camera.m_XAxis.m_MaxSpeed = _axisSensitivity.x * _sensitivityCoef;
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
