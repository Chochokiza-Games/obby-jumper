using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuideHand : MonoBehaviour
{
    [SerializeField] private RectTransform _eggRect;
    [SerializeField] private Image _image;
    [SerializeField] private float _animationDuration;
    [SerializeField] private float _pauseTime;
    [SerializeField] private float _heightOffset;
    [SerializeField] private AnimationCurve _alphaCurve;

    private void Start()
    {
        StartCoroutine(AnimationRoutine());
    }

    private IEnumerator AnimationRoutine()
    {
        while (true)
        {
            transform.position = new Vector3(transform.position.x, _eggRect.position.y + _heightOffset);
            Vector3 startPosition = transform.position;
            Vector3 endPosition = new Vector3(transform.position.x, _eggRect.position.y - _heightOffset);
            float timeElapsed = 0;
            while (timeElapsed < _animationDuration)
            {
                _image.color = new Color(1, 1, 1, _alphaCurve.Evaluate(timeElapsed / _animationDuration));
                transform.position = Vector3.Lerp(startPosition, endPosition, timeElapsed / _animationDuration);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            _image.color = new Color(1, 1, 1, 0);

            yield return new WaitForSeconds(_pauseTime);
        }
    }

    public void Enable()
    {
        StartCoroutine(AnimationRoutine());
    }

    public void Disable()
    {
        StopAllCoroutines();
        _image.color = new Color(0, 0, 0, 0);
    }
}
