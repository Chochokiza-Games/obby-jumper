using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ReminderPopup : MonoBehaviour
{
    [SerializeField] private float _timeToRemind;
    [SerializeField] private GameObject _window;
    [SerializeField] private DistractionFromGameplayChecker _checker;
    
    private bool _canShow = false;
    private Coroutine _showRoutine;

    private void Start()
    {
        StartCoroutine(WaitRoutine());
    }

    private IEnumerator WaitRoutine()
    {
        _canShow = false;
        yield return new WaitForSeconds(_timeToRemind);
        _canShow = true;
    }

    public void Show()
    {
        if (_showRoutine == null)
        {
            _showRoutine = StartCoroutine(ShowRoutine());
        }
    }

    private IEnumerator ShowRoutine()
    {
        while (_checker.CanDistractFromGameplay() == false || _canShow == false)
        {
            yield return null;
        }
    
        Debug.Log($"Can Distract From gameplay: {_checker.CanDistractFromGameplay()} can show: {_canShow}");

        _window.SetActive(true);

        _canShow = false;
        _showRoutine = null;
        StartCoroutine(WaitRoutine());  
    }
}
