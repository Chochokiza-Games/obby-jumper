using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IncreasePowerButton : MonoBehaviour
{
    [SerializeField] private float _delay;
    [SerializeField] private Image _slicedImage;
    [SerializeField] private Button _button;
    [SerializeField] private Image[] _imagesToDisable;

    private void OnDisable()
    {
        _slicedImage.fillAmount = 0;
        _button.interactable = true;
    }

    public void OnClick()
    {
        StartCoroutine(LockRoutine());
    }

    public void SetActiveButton(bool state) {
        foreach(Image img in _imagesToDisable) {
            img.enabled = state;
        }
    }

    private IEnumerator LockRoutine()
    {
        _button.interactable = false;
        float timeElapsed = 0;
        _slicedImage.fillAmount = 1f;
        while (timeElapsed < _delay)
        {
            _slicedImage.fillAmount = 1f - (timeElapsed / _delay);

            timeElapsed += Time.deltaTime;  
            yield return null;
        }
        _slicedImage.fillAmount = 0;
        _button.interactable = true;
    }
}
