using System;
using System.Collections;
using System.Collections.Generic;
using PolyverseSkiesAsset;
using TMPro;
using UnityEngine;

public class PlayerRecordInfo : MonoBehaviour
{
    public float AnimationDuration 
    {
        get => _duration;
    }

    [SerializeField] private TextMeshProUGUI _label;
    [SerializeField] private string _ruTemplate;
    [SerializeField] private string _enTemplate;
    [SerializeField] private AnimationCurve _sizeCurve;
    [SerializeField] private AnimationCurve _alphaCurve;
    [SerializeField] private float _duration;
    [SerializeField] private string _ruLevelIncreaseTemplate;
    [SerializeField] private string _enLevelIncreaseTemplate;
    [SerializeField] private LoadingScreen _loadingScreen;

    private LanguageTranslator.Languages _currentLanguage;

    private void Start()
    {
        _currentLanguage = FindObjectOfType<LanguageTranslator>().CurrentLangunage;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
    
    public void Hide()
    {
        StopAllCoroutines();
        _label.alpha = 0;
    }

    public void Show(int record)
    {
        gameObject.SetActive(true);
        _label.text = $"{(_currentLanguage == LanguageTranslator.Languages.Russian ? _ruTemplate : _enTemplate)} {record}!";
        StartCoroutine(ShowRoutine());
    }

    public void ShowLevel(int level)
    {
        StartCoroutine(ShowLevelRoutine(level));
    }

    private IEnumerator ShowLevelRoutine(int level)
    {
        while (_loadingScreen.Opened)
        {
            yield return null;
        }
       _label.text = $"{(_currentLanguage == LanguageTranslator.Languages.Russian ? _ruLevelIncreaseTemplate : _enLevelIncreaseTemplate)} {level}!";
        StartCoroutine(ShowRoutine());
    }

    private IEnumerator ShowRoutine()
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
