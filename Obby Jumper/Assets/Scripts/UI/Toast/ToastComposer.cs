using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.WSA;

public class ToastComposer : MonoBehaviour
{
    public enum ToastsType
    {
        StoreErr,
    }
    public Vector3 TargetPositionOffset
    {
        set => _targetPositionOffset = value;
    }

    [SerializeField] private GameObject _toastPrefab;
    [SerializeField] private RectTransform _startPositionPoint;
    [SerializeField] private float _pathDuration;
    [SerializeField] private float _pauseDuration;
    [SerializeField] private Vector2 _targetPositionOffset;
    [SerializeField] private Dictionary<ToastsType, string> _toastMap;
    [SerializeField] private LanguageTranslator _language;

    private int _currentToastsCount;
    private Vector3 _startPosition;
    private Vector3 _targetPosition;

    private void Start()
    {
        _startPosition = _startPositionPoint.anchoredPosition;
        _toastMap[ToastsType.StoreErr] = _language.CurrentLangunage == LanguageTranslator.Languages.Russian ?  "Недостаточно денег" :  "Not enough money";
        ToastSpawn(ToastsType.StoreErr);
    }

    public void ToastSpawn(ToastsType toastType)
    {
        Debug.LogError("ffs");
        StartCoroutine(AnimationRoutine(toastType));
    }
    private IEnumerator AnimationRoutine(ToastsType toastType)
    {        
        _targetPosition = (_startPositionPoint.anchoredPosition - (Vector2.up * _startPositionPoint.rect.size.y) - _targetPositionOffset)*_currentToastsCount;
        TextMeshProUGUI toast = Instantiate(_toastPrefab, _targetPosition, Quaternion.identity).GetComponent<TextMeshProUGUI>();
        toast.SetText(_toastMap[toastType]);

        _currentToastsCount += 1;

        float timeElapsed = 0;
        while (timeElapsed < _pathDuration) 
        {
            float t = timeElapsed / _pathDuration; 
            t = t * t * (3f - 2f * t);
            _startPositionPoint.anchoredPosition = Vector2.Lerp(_startPosition, _targetPosition, t);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(_pauseDuration);
        timeElapsed = 0;

        while (timeElapsed < _pathDuration)
        {
            float t = timeElapsed / _pathDuration;
            t = t * t * (3f - 2f * t);
            _startPositionPoint.anchoredPosition = Vector2.Lerp(_targetPosition, _startPosition, t);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        _currentToastsCount -= 1;
        Destroy(toast);
    }
}
