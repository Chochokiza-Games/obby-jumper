using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public bool Opened
    {
        get => _opened;
    }

    [SerializeField] private Image _background;
    [SerializeField] private float _popUpAnimationDuration;
    [SerializeField] private GameObject _loadingPanel;
    [SerializeField] private AnimationCurve _loadingPanelCurve;
    [SerializeField] private Transform _LoadingPanelTarget;
    [SerializeField] private AnimationCurve _backgroundScalingAnimationCurve;
    [SerializeField] private Image _loadingIcon;
    [SerializeField] private float _imageWiggleDuration;
    [SerializeField] private AnimationCurve _animationWiggleCurve;
    [SerializeField] private UnityEvent _started;
    [SerializeField] private UnityEvent _halfLoaded;
    [SerializeField] private UnityEvent _ended;

    private bool _opened;

    private float _loadingPanelLiftingHeight;

    private void Start()
    {
        _loadingPanelLiftingHeight = _LoadingPanelTarget.position.y;
    }
    public void Show()
    {
        StopAllCoroutines();
        _started.Invoke();
        _opened = true;
        StartCoroutine(LoadingScreenPopUpRoutine());
    }

    private IEnumerator LoadingScreenPopUpRoutine()
    {
        Coroutine wiggle = StartCoroutine(IconWiggleRoutine());
        bool eventInvoked = false;

        float timeElapsed = 0;
        while (timeElapsed < _popUpAnimationDuration)
        {
            _background.transform.localScale = new Vector3(
                _backgroundScalingAnimationCurve.Evaluate(timeElapsed / _popUpAnimationDuration), 1, 1);


            if (timeElapsed >= _popUpAnimationDuration / 2 && eventInvoked == false)
            {
                _halfLoaded.Invoke();
                eventInvoked = true;
            }
            
            _loadingPanel.transform.position = new Vector3(
                _loadingPanel.transform.position.x,

                _loadingPanelCurve.Evaluate(timeElapsed / _popUpAnimationDuration) * _loadingPanelLiftingHeight,
                _loadingPanel.transform.position.z);

            timeElapsed += Time.deltaTime;
            yield return null;
        }
        StopCoroutine(wiggle);
        _background.transform.localScale *= 0;
        _ended.Invoke();
        _opened = false;
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

