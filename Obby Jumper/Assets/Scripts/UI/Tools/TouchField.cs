using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchField : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Vector3 TouchDist
    {
        get => _touchDist; 
    }

    [SerializeField] private float _minThreshold;

    private Vector3 _touchDist;
    private Vector2 _pointerOld;
    private bool _pressed;
    private int _pointerId;
    private float _currentLenght;

    private void Start()
    {
        if (!FindObjectOfType<PlayerProfile>().RunOnMobile())
        {
            Destroy(gameObject);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _pointerId = eventData.pointerId;
        _pressed = true;
        _pointerOld = eventData.position;
        _currentLenght = 0;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _pressed = false;
    }

    public bool MinThresholdReached()
    {
        return _currentLenght >= _minThreshold;
    }
    
    private void Update()
    {
        if (_pressed)
        {
            if (_pointerId >= 0 && _pointerId < Input.touches.Length)
            {
                _touchDist = Input.touches[_pointerId].position - _pointerOld;
                _pointerOld = Input.touches[_pointerId].position;
                _currentLenght = _touchDist.magnitude;
            }
        }
        else
        {
            _touchDist = Vector3.zero;
        }
    }

    


}
