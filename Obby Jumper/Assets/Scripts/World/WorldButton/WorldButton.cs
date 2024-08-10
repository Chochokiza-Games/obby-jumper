using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class WorldButton : MonoBehaviour
{
    [SerializeField] private LanguageTranslator _translator;
    [SerializeField] private UnityEvent _buttonClicked;
    [SerializeField] private GameObject _worldButtonObject;
    [SerializeField] private Button _worldButton;
    [SerializeField] private TextMeshProUGUI _worldButtonLabel;
    [SerializeField] private string _ruLabel;
    [SerializeField] private string _enLabel;
    [SerializeField] private Camera _camera;

    private bool _active;

    public void OnPlayerEnter()
    {
        _worldButtonLabel.text = _translator.CurrentLangunage == LanguageTranslator.Languages.Russian ? _ruLabel : _enLabel;
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
    }

    public void OnPlayerExit()
    {
        _active = false;
        _worldButton.onClick.RemoveAllListeners();
        _worldButtonObject.gameObject.SetActive(false);
    }
}
