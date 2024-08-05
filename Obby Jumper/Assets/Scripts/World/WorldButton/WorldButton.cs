using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class WorldButton : MonoBehaviour
{
    [SerializeField] private UnityEvent _buttonClicked;
    [SerializeField] private GameObject _worldButtonObject;
    [SerializeField] private Button _worldButton;
    [SerializeField] private TextMeshProUGUI _worldButtonLabel;
    [SerializeField] private string _label;
    [SerializeField] private Camera _camera;


    [Header("Trigger Visualization")]
    [SerializeField] private bool _triggerVisualize = false;
    [SerializeField] private Transform _triggerVisualization;
    [SerializeField] private float _triggerRotateSpeed;

    private bool _active;

    public void OnPlayerEnter()
    {
        _worldButtonLabel.text = _label;
        _active = true;
        _worldButton.onClick.AddListener(OnPlayerClicked);
        _worldButtonObject.gameObject.SetActive(true);
    }

    public void OnPlayerClicked()
    {
        _buttonClicked.Invoke();
    }

    private void Update()
    {
        if (_active)
        {
            _worldButtonObject.transform.position = _camera.WorldToScreenPoint(transform.position); 
        }

        if (_triggerVisualize == true)
        {
            _triggerVisualization.Rotate(Vector3.up * _triggerRotateSpeed * Time.deltaTime);
        }
    }

    public void OnPlayerExit()
    {
        _active = false;
        _worldButton.onClick.RemoveAllListeners();
        _worldButtonObject.gameObject.SetActive(false);
    }
}