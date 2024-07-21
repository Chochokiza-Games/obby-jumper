using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AnimatedButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool MouseHovered
    {
        get => _mouseHovered;
    }


    [Range(1f, 2f)]
    [SerializeField] private float _scaleFactor;
    [Range(1f, 4f)]
    [SerializeField] private float _pulseScaleFactor;
    [Range(1f, 2f)]
    [SerializeField] private float _labelScaleFactor;
    [SerializeField] private float _iconRotation;
    [SerializeField] private float _animationDuration;
    [SerializeField] private float _pulsateAnimationDuration;
    [SerializeField] private Transform _iconTransform;
    [SerializeField] private Transform _labelTransform;

    private Vector3 _startScale;
    private Vector3 _endScale;
    private Vector3 _endPulseScale;
    private Vector3 _currentScale;
    private Vector3 _labelStartScale;
    private Vector3 _labelEndScale;
    private Vector3 _labelCurrentScale;
    private float _iconStartRotation;
    private float _iconEndRotation;
    private float _iconCurrentRotation;

    private bool _mouseHovered = false;
    private float _timeElapsed = 0;

    private void OnEnable()
    {
        _timeElapsed = 0;
        transform.localScale = Vector3.one;
        _iconTransform.eulerAngles = Vector3.zero;
        _currentScale = transform.localScale;
        _iconCurrentRotation = _iconTransform.eulerAngles.z;
        _mouseHovered = false;
    }

    private void Start()
    {
        _startScale = transform.localScale; 
        _endScale = transform.localScale * _scaleFactor;
        _endPulseScale = transform.localScale * _pulseScaleFactor;
        _currentScale = _startScale;
        _labelStartScale = _labelTransform.localScale;
        _labelEndScale = _labelTransform.localScale * _labelScaleFactor;
        _labelCurrentScale = _labelStartScale;
        _iconStartRotation = _iconTransform.eulerAngles.z;
        _iconEndRotation = _iconRotation;
        _iconCurrentRotation = _iconStartRotation;
    }

    private void Update()
    {
        if (_timeElapsed < _animationDuration)
        {
            if (_mouseHovered)
            {
                transform.localScale = Vector3.Lerp(_currentScale, _endScale, _timeElapsed / _animationDuration);
                _labelTransform.localScale = Vector3.Lerp(_labelCurrentScale, _labelEndScale, _timeElapsed / _animationDuration);
                _iconTransform.eulerAngles = Vector3.Lerp(new Vector3(0, 0, _iconCurrentRotation), new Vector3(0, 0, _iconRotation), _timeElapsed / _animationDuration);
            }
            else
            {
                transform.localScale = Vector3.Lerp(_currentScale, _startScale, _timeElapsed / _animationDuration);
                _labelTransform.localScale = Vector3.Lerp(_labelCurrentScale, _labelStartScale, _timeElapsed / _animationDuration);
                _iconTransform.eulerAngles = Vector3.Lerp(new Vector3(0, 0, _iconCurrentRotation), new Vector3(0, 0, _iconStartRotation), _timeElapsed / _animationDuration);
            }
            _timeElapsed += Time.deltaTime;
        }
        else
        {
            if (_mouseHovered)
            {
                transform.localScale = _endScale;
                _labelTransform.localScale = _labelEndScale;
                _iconTransform.eulerAngles = Vector3.forward * _iconRotation;
            }
            else
            {
                transform.localScale = _startScale;
                _labelTransform.localScale = _labelStartScale;
                _iconTransform.eulerAngles = Vector3.forward * _iconStartRotation;
            }
        }
    }

    private void ResetAll()
    {
        _timeElapsed = 0;
        _currentScale = transform.localScale;
        _labelCurrentScale = _labelTransform.localScale;
        _iconCurrentRotation = _iconTransform.eulerAngles.z;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _mouseHovered = true;
        ResetAll();

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _mouseHovered = false;
        ResetAll();
    }

    public void StartPulsate()
    {
        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(StartPulsateRoutine());
        }
    }

    public void StopPulsate()
    {
        StopAllCoroutines();
        transform.localScale = _startScale;
    }
    private IEnumerator StartPulsateRoutine()
    {
        while (true)
        {
            float timeElapsed = 0;

            while (timeElapsed < _pulsateAnimationDuration)
            {
                transform.localScale = Vector3.Lerp(_startScale, _endPulseScale, timeElapsed / _pulsateAnimationDuration);
                timeElapsed += Time.deltaTime;
                yield return null;

            }
            timeElapsed = 0;
            while (timeElapsed < _pulsateAnimationDuration)
            {
                transform.localScale = Vector3.Lerp(_endPulseScale, _startScale, timeElapsed / _pulsateAnimationDuration);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
        }
    }


}
