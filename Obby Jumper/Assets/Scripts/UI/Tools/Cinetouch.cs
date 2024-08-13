using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cinetouch : MonoBehaviour
{
    [SerializeField] private CinemachineFreeLook _cinecam;
    [SerializeField] private CameraPivot _pivot;
    [SerializeField] private TouchField _touchField;
    [SerializeField] private Vector2 _sensivity;

    private bool _isMobile = false;

    private void Start()
    {
        _isMobile = FindObjectOfType<PlayerProfile>().RunOnMobile();
        if (_isMobile)
        {
            _cinecam.m_XAxis.m_InputAxisName = "";
            _cinecam.m_YAxis.m_InputAxisName = "";
        }
    }

    private void Update()
    {
        if (_isMobile)
        {
            _cinecam.m_XAxis.Value += ((_touchField.TouchDist.x * 200 * _sensivity.x) * _pivot.SensitivityCoef) * Time.deltaTime;
            _cinecam.m_YAxis.Value -= ((_touchField.TouchDist.y * _sensivity.y) * _pivot.SensitivityCoef) * Time.deltaTime;
        }
    }
}
