using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutEffect : MonoBehaviour
{
    [SerializeField] private float _lifetime;
    [SerializeField] private AnimationCurve _sizeCurve;
    [SerializeField] private AnimationCurve _alphaCurve;
    [SerializeField] private float _horizontalOffset;
    [SerializeField] private float _verticalOffset;
    [SerializeField] private float _horizontalSpeed;
    [SerializeField] private float _verticalSpeed;
    [SerializeField] private Image _image;

    public void Initialize(PetOpening.SwipeDirection direction)
    {
        StartCoroutine(CutAnimationRoutine(direction));
    }

    private IEnumerator CutAnimationRoutine(PetOpening.SwipeDirection direction)
    {
        switch (direction)
        {
            case PetOpening.SwipeDirection.Up:
                transform.position -= Vector3.up * _verticalOffset;
                transform.eulerAngles = Vector3.forward * 90;
                break;
            case PetOpening.SwipeDirection.Down:
                transform.position += Vector3.up * _verticalOffset;
                transform.eulerAngles = Vector3.forward * -90;
                break;
            case PetOpening.SwipeDirection.Left:
                transform.position += Vector3.right * _horizontalOffset;
                transform.eulerAngles = Vector3.forward * 180   ;
                break;
            case PetOpening.SwipeDirection.Right:
                transform.position -= Vector3.right * _horizontalOffset;
                break;  
        }

        float timeElapsed = 0;
        while (timeElapsed < _lifetime)
        {
            transform.localScale = Vector3.one * _sizeCurve.Evaluate(timeElapsed / _lifetime);
            _image.color = new Color(1, 1, 1, _alphaCurve.Evaluate(timeElapsed / _lifetime));
            timeElapsed += Time.deltaTime;
            switch (direction)
            {
                case PetOpening.SwipeDirection.Up:
                    transform.position += Vector3.up * _verticalSpeed * Time.deltaTime;
                    break;
                case PetOpening.SwipeDirection.Down:
                    transform.position -= Vector3.up * _verticalSpeed * Time.deltaTime;
                    break;
                case PetOpening.SwipeDirection.Left:
                    transform.position -= Vector3.right * _horizontalSpeed * Time.deltaTime;
                    break;
                case PetOpening.SwipeDirection.Right:
                    transform.position += Vector3.right * _horizontalSpeed * Time.deltaTime;
                    break;
            }
            yield return null;
        }

        _image.color = new Color(1, 1, 1, 0);
        Destroy(gameObject);
    }
}
