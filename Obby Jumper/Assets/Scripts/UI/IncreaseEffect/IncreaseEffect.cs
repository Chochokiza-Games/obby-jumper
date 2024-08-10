using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseEffect : MonoBehaviour
{
    [SerializeField] private RectTransform _rTransform;
    [SerializeField] private GameObject _previewPrefab;
    [SerializeField] private float _flyDuration;
    [SerializeField] private float _delayBeforeFly;
     [SerializeField] private float _minDelayBeforeSpawn;
    [SerializeField] private float _maxDelayBeforeSpawn;
    [SerializeField] private GameObject _target;
    [SerializeField] private AnimationCurve _scaleCurve;
    [SerializeField] private int _maxEffectsOnScreen;

    private int _oldValue = -1;

    public void ShowEffect()
    {
        StartCoroutine(ShowEffectRoutine());
    }

    public void ShowEffect(int count)
    {
        if (count == 0 || count == _oldValue)
        {
            return;
        }

        _oldValue = count;

        if (count > _maxEffectsOnScreen)
        {
            count = _maxEffectsOnScreen;
        }

        for (int i = 0; i < count; i++)
        {
            ShowEffect();
        }
    }

    private IEnumerator ShowEffectRoutine()
    {
        Vector3 randomPos = new Vector3(Random.Range(0, Screen.width), 
            Random.Range(0, Screen.height));

        float randomTime = Random.Range(_minDelayBeforeSpawn, _maxDelayBeforeSpawn);
        yield return new WaitForSeconds(randomTime);
        Transform t = Instantiate(_previewPrefab, randomPos, Quaternion.identity, transform).transform;
    StartCoroutine(
            FlyRoutine(
                t
            ));
    }

    private IEnumerator FlyRoutine(Transform preview)
    {
        StartCoroutine(ScaleRoutine(preview));
        yield return new WaitForSeconds(_delayBeforeFly);
        float timeElapsed = 0;
        Vector3 startPosition = preview.position;
        while (timeElapsed < _flyDuration)
        {
            preview.position = Vector3.Lerp(startPosition, _target.transform.position, timeElapsed / _flyDuration);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        yield return null;

        Destroy(preview.gameObject);
    }

    private IEnumerator ScaleRoutine(Transform preview) 
    {
        float timeElapsed = 0;
        Vector3 startScale = preview.localScale;
        while (timeElapsed < _flyDuration + _delayBeforeFly && preview != null)
        {
            preview.localScale = startScale * _scaleCurve.Evaluate(timeElapsed / (_flyDuration + _delayBeforeFly));

            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }
}
