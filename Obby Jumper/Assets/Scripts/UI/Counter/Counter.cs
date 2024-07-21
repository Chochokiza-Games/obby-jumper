using System.Collections;
using TMPro;
using UnityEngine;

public class Counter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _label;
    [SerializeField] private float _changeTime = 0;
    [SerializeField] private int _changeSteps = 0;
    [SerializeField] private TextMeshProUGUI _increaseInfo;
    [SerializeField] private AnimationCurve _moneyIncreaseInfoCurve;

    [SerializeField] private float _moneyIncreaseInfoLifetime;
    
    private Coroutine _moneyIncreaseInfoCoroutine;
    private float _currentValue = 0;
    private float _neededValue;

    public void OnValueChange(int value)
    {
        if (_changeSteps > 0 && gameObject.activeInHierarchy)
        {
            _neededValue = value;
            StopAllCoroutines();
            StartCoroutine(SmoothChangeValue());
        }
        else
        {
            _label.text = value.ToString();
        }
    }

    public void OnValueChange(float value)
    {
        if (_changeSteps > 0 && gameObject.activeInHierarchy) 
        {
            _neededValue = value;
            StopAllCoroutines();
            StartCoroutine(SmoothChangeValue());
        }
        else
        {
            _label.text = value.ToString();
        }
    }

    private IEnumerator SmoothChangeValue() 
    {
        if (_increaseInfo != null && (_neededValue - _currentValue) > 0)
        {
            if (_moneyIncreaseInfoCoroutine != null)
            {
                StopCoroutine(_moneyIncreaseInfoCoroutine);
            }
            _moneyIncreaseInfoCoroutine = StartCoroutine(MoneyIncreaseInfoRoutine());
        }
        Debug.Log($"current {_currentValue}");
        Debug.Log($"needed {_neededValue}");
        float currentValue = _currentValue;
        float endValue = _neededValue;
        _currentValue = _neededValue;
        int stepValue = Mathf.RoundToInt(endValue - currentValue) / _changeSteps; 
        for (int i = 0; i < _changeSteps; i++) 
        {
            currentValue += stepValue;
            _label.text = ((int)currentValue).ToString();
            yield return new WaitForSeconds(_changeTime / _changeSteps);
        }
        _label.text = ((int)_neededValue).ToString();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        _label.text = ((int)_neededValue).ToString();
        if (_increaseInfo != null) 
        {
            _increaseInfo.alpha = 0;
        }
    }

    private IEnumerator MoneyIncreaseInfoRoutine()
    {
        _increaseInfo.gameObject.SetActive(true);
        _increaseInfo.SetText($"+{(_neededValue - _currentValue).ToString()}");
        float timeElapsed = 0;
        while (timeElapsed < _moneyIncreaseInfoLifetime)
        {
            _increaseInfo.alpha = _moneyIncreaseInfoCurve.Evaluate(timeElapsed / _moneyIncreaseInfoLifetime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        _increaseInfo.gameObject.SetActive(false);
    }
}
