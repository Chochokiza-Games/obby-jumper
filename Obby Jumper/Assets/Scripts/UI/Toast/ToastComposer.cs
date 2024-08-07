using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.WSA;

public class ToastComposer : MonoBehaviour
{
    public enum Type
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
    [SerializeField] private LanguageTranslator _language;
    private Dictionary<Type, string> _toastMap = new Dictionary<Type, string>();

    private int _currentToastsCount;
    private Vector3 _startPosition;

    private void Start()
    {
        _startPosition = _startPositionPoint.anchoredPosition;
        _toastMap[Type.StoreErr] = _language.CurrentLangunage == LanguageTranslator.Languages.Russian ?  "Недостаточно денег" :  "Not enough money";
    }

    public void ToastSpawn(Type toastType)
    {
        TextMeshProUGUI toast = Instantiate(_toastPrefab, _startPositionPoint.position, Quaternion.identity, transform).GetComponent<TextMeshProUGUI>();
        toast.SetText(_toastMap[toastType]);

        StartCoroutine(AnimationRoutine(toast.GetComponent<RectTransform>()));
        
    }
    public void ToastSpawn(Type toastType, Color color)
    {
        TextMeshProUGUI toast = Instantiate(_toastPrefab, _startPositionPoint.position, Quaternion.identity, transform).GetComponent<TextMeshProUGUI>();
        toast.SetText(_toastMap[toastType]);
        toast.color = color;
        StartCoroutine(AnimationRoutine(toast.GetComponent<RectTransform>()));
    }
    private IEnumerator AnimationRoutine(RectTransform toast)
    {
         _currentToastsCount++;
        Vector3 targetPosition = (_startPositionPoint.anchoredPosition - (Vector2.up * _startPositionPoint.rect.size.y) - _targetPositionOffset)*_currentToastsCount;

        float timeElapsed = 0;
        while (timeElapsed < _pathDuration) 
        {
            float t = timeElapsed / _pathDuration; 
            t = t * t * (3f - 2f * t);
            toast.anchoredPosition = Vector2.Lerp(_startPosition, targetPosition, t);            timeElapsed += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(_pauseDuration);
        timeElapsed = 0;

        while (timeElapsed < _pathDuration)
        {
            float t = timeElapsed / _pathDuration;
            t = t * t * (3f - 2f * t);
            toast.anchoredPosition = Vector2.Lerp(targetPosition, _startPosition, t);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        _currentToastsCount --;
        Destroy(toast.gameObject);
    }
}
