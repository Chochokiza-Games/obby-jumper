using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RewardPreview : MonoBehaviour
{
    [SerializeField] private RectTransform _rTransform;
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _label;
    [SerializeField] private AnimationCurve _showCurve;
    [SerializeField] private float _showDuration;
    [SerializeField] private float _pauseDuration;
    [SerializeField] private GameObject _previewBase;
    [SerializeField] private UnityEvent _rewardShowed;

    private void Start()
    {
        _previewBase.transform.position = new Vector3(_previewBase.transform.position.x, -Screen.height / 2);
    }

    private void OnDisable()
    {
        _previewBase.transform.position = new Vector3(_previewBase.transform.position.x, -Screen.height / 2);
    }

    public void Show(Sprite icon, string label)
    {
        _previewBase.transform.position = new Vector3(_previewBase.transform.position.x, -Screen.height / 2);
        //gameObject.SetActive(true);
        _image.sprite = icon;
        _label.text = label;
        StopAllCoroutines();
        StartCoroutine(ShowRoutine());
    }

    private IEnumerator ShowRoutine()
    {
        _rewardShowed.Invoke();

        float timeElapsed = 0;
        Vector3 startPosition = _previewBase.transform.position;
        Vector3 targetPosition = new Vector3(_previewBase.transform.position.x, Screen.height / 2);
        while (timeElapsed < _showDuration)
        {
            float percentPosition = _showCurve.Evaluate(timeElapsed / _showDuration);
            _previewBase.transform.position = Vector3.Lerp(startPosition, targetPosition, percentPosition);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(_pauseDuration);
        startPosition = _previewBase.transform.position;
        targetPosition = new Vector3(_previewBase.transform.position.x, -Screen.height / 2);
        timeElapsed = 0;
        while (timeElapsed < _showDuration)
        {
            float percentPosition = _showCurve.Evaluate(timeElapsed / _showDuration);
            _previewBase.transform.position = Vector3.Lerp(startPosition, targetPosition, percentPosition);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        //gameObject.SetActive(false);
    }
}
