using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toast : MonoBehaviour
{
    public Vector3 TargetPositionOffset
    {
        set => _targetPositionOffset = value;
    }

    [SerializeField] private RectTransform _rTransform;
    [SerializeField] private float _pathDuration;
    [SerializeField] private float _pauseDuration;
    [SerializeField] private Vector2 _targetPositionOffset;

    private Vector3 _startPosition;
    private Vector3 _targetPosition;

    private void Start()
    {
        _startPosition = _rTransform.anchoredPosition;
        _targetPosition = _rTransform.anchoredPosition - (Vector2.up * _rTransform.rect.size.y) - _targetPositionOffset;

        StartCoroutine(AnimationRoutine());
    }

    private IEnumerator AnimationRoutine()
    {
        float timeElapsed = 0;
        while (timeElapsed < _pathDuration) 
        {
            float t = timeElapsed / _pathDuration; 
            t = t * t * (3f - 2f * t); // https://youtu.be/-VwNSKeQNm4?si=BWS5oio3QO2EX8G1
            _rTransform.anchoredPosition = Vector2.Lerp(_startPosition, _targetPosition, t);
            timeElapsed += Time.deltaTime;
            yield return null; //wait for next frame
        }

        yield return new WaitForSeconds(_pauseDuration);
        timeElapsed = 0;

        while (timeElapsed < _pathDuration)
        {
            float t = timeElapsed / _pathDuration;
            t = t * t * (3f - 2f * t); // https://youtu.be/-VwNSKeQNm4?si=BWS5oio3QO2EX8G1
            _rTransform.anchoredPosition = Vector2.Lerp(_targetPosition, _startPosition, t);
            timeElapsed += Time.deltaTime;
            yield return null; //wait for next frame
        }

        Destroy(gameObject);
    }
}
