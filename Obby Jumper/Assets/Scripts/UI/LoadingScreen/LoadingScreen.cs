using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private Image _background;
    [SerializeField] private Image _pattern;
    [SerializeField] private float _popUpAnimationDuration;
    [SerializeField] private GameObject _loadingPanel;
    [SerializeField] private AnimationCurve _loadingPanelCurve;
    [SerializeField] private Transform _LoadingPanelTarget;
    [SerializeField] private AnimationCurve _backgroundScalingAnimationCurve;
    [SerializeField] private Image _loadingIcon;
    [SerializeField] private float _imageWiggleDuration;
    [SerializeField] private AnimationCurve _animationWiggleCurve;
    [SerializeField] private TextMeshProUGUI _tips;
    [SerializeField] private string[] _enTips;
    [SerializeField] private string[] _ruTips;
    [SerializeField] private LanguageTranslator _language;
    [SerializeField] private UnityEvent _started;
    [SerializeField] private UnityEvent _ended;

    private float _loadingPanelLiftingHeight;

    private void Start()
    {
        _loadingPanelLiftingHeight = _LoadingPanelTarget.position.y;
    }
    public void Show()
    {
        StopAllCoroutines();
        _started.Invoke();
        StartCoroutine(LoadingScreenPopUpRoutine());
    }

    private IEnumerator LoadingScreenPopUpRoutine()
    {
        Coroutine wiggle = StartCoroutine(IconWiggleRoutine());

        _tips.SetText(_language.CurrentLangunage == LanguageTranslator.Languages.Russian? _ruTips[Random.Range(0, _ruTips.Length)] : _enTips[Random.Range(0, _enTips.Length)]);
        
        float timeElapsed = 0;
        Vector3 startPatternScale = _pattern.transform.localScale;
        while (timeElapsed < _popUpAnimationDuration)
        {
            _background.transform.localScale = new Vector3(
                _backgroundScalingAnimationCurve.Evaluate(timeElapsed / _popUpAnimationDuration), 1, 1);

            _pattern.transform.localScale = new Vector3(
                _background.transform.localScale.x <= 0.001f ? 0.001f : startPatternScale.x / _background.transform.localScale.x, 
                _pattern.transform.localScale.y,
                _pattern.transform.localScale.z);

            _loadingPanel.transform.position = new Vector3(
                _loadingPanel.transform.position.x,

                _loadingPanelCurve.Evaluate(timeElapsed / _popUpAnimationDuration) * _loadingPanelLiftingHeight,
                _loadingPanel.transform.position.z);

            timeElapsed += Time.deltaTime;
            yield return null;
        }
        StopCoroutine(wiggle);
        _background.transform.localScale *= 0;
        _pattern.transform.localScale = startPatternScale;
        _ended.Invoke();
    }
    private IEnumerator IconWiggleRoutine()
    {

        while (true)
        {
            float timeElapsed = 0;
            while (timeElapsed < _imageWiggleDuration)
            {
                _loadingIcon.transform.eulerAngles = Vector3.forward * _animationWiggleCurve.Evaluate(timeElapsed / _imageWiggleDuration);


                timeElapsed += Time.deltaTime;
                yield return null;
            }
        }
    }

}

