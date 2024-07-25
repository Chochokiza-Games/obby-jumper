using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerRecordInfo : MonoBehaviour
{
    public float AnimationDuration 
    {
        get => _duration;
    }

    [SerializeField] private TextMeshProUGUI _label;
    [SerializeField] private string _template;
    [SerializeField] private AnimationCurve _sizeCurve;
    [SerializeField] private AnimationCurve _alphaCurve;
    [SerializeField] private float _duration;

    public void Show(int record)
    {
        _label.text = $"{_template} {record}!";

        StartCoroutine(ShowRoutine(record));
    }

    private IEnumerator ShowRoutine(int record)
    {
        float timeElapsed = 0;
        Vector3 startScale = transform.localScale;
        while(timeElapsed < _duration)
        {
            transform.localScale = startScale * _sizeCurve.Evaluate(timeElapsed / _duration);
            _label.alpha = _alphaCurve.Evaluate(timeElapsed / _duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        _label.alpha = 0;
    }

}
