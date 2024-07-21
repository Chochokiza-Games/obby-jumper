using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerIncreaseEffect : MonoBehaviour
{
    [SerializeField] private RectTransform _rTransform;
    [SerializeField] private GameObject _previewPrefab;
    [SerializeField] private float _flyDuration;
    [SerializeField] private GameObject _target;
    [SerializeField] private AnimationCurve _scaleCurve;

    public void ShowEffect()
    {
        Vector3 randomPos = new Vector3(Random.Range(0, Screen.width), 
            Random.Range(0, Screen.height));
        StartCoroutine(
            FlyRoutine(
                Instantiate(_previewPrefab, randomPos, Quaternion.identity, transform).transform)
            );
    }

    private IEnumerator FlyRoutine(Transform preview)
    {
        float timeElapsed = 0;
        Vector3 startScale = preview.localScale;
        Vector3 startPosition = preview.position;
        while (timeElapsed < _flyDuration)
        {
            preview.localScale = startScale * _scaleCurve.Evaluate(timeElapsed / _flyDuration);
            preview.position = Vector3.Lerp(startPosition, _target.transform.position, timeElapsed / _flyDuration);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        Destroy(preview.gameObject);
    }
}
