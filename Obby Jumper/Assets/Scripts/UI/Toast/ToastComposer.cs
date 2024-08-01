using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.WSA;

public class ToastComposer : MonoBehaviour
{
    public Vector3 TargetPositionOffset
    {
        set => _targetPositionOffset = value;
    }

    [SerializeField] private GameObject _toastPrefab;
    [SerializeField] private RectTransform _rTransform;
    [SerializeField] private float _pathDuration;
    [SerializeField] private float _pauseDuration;
    [SerializeField] private Vector2 _targetPositionOffset;

    private int _currentToastsCount;
    private Vector3 _startPosition;
    private Vector3 _targetPosition;

    private void Start()
    {
        _startPosition = _rTransform.anchoredPosition;
        StartCoroutine(AnimationRoutine());
    }

    private IEnumerator AnimationRoutine()
    {
        GameObject toast = Instantiate(_toastPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        //TextMeshProUGUI text_toast = toast.GetComponent<TMP_Text>;
        // toast.transform.position.y = toast.rect.size.y/2;
        
        _currentToastsCount += 1;
        _targetPosition = (_rTransform.anchoredPosition - (Vector2.up * _rTransform.rect.size.y) - _targetPositionOffset)*_currentToastsCount;

        float timeElapsed = 0;
        while (timeElapsed < _pathDuration) 
        {
            float t = timeElapsed / _pathDuration; 
            t = t * t * (3f - 2f * t);
            _rTransform.anchoredPosition = Vector2.Lerp(_startPosition, _targetPosition, t);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(_pauseDuration);
        timeElapsed = 0;

        while (timeElapsed < _pathDuration)
        {
            float t = timeElapsed / _pathDuration;
            t = t * t * (3f - 2f * t);
            _rTransform.anchoredPosition = Vector2.Lerp(_targetPosition, _startPosition, t);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        _currentToastsCount -= 1;
        Destroy(gameObject);
    }
}
