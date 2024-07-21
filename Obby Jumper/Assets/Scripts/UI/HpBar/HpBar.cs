using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _label;
    [SerializeField] private Image[] _images;
    [SerializeField] private float _lifetime;
    [SerializeField] private float _showHpDuration;
    [SerializeField] private Image _hpBar;
    [SerializeField] private Image _hpShadow;

    private void OnEnable()
    {
        StopAllCoroutines();
        SetAlphaImages(0);
        _label.color = new Color(1, 1, 1, 0);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        SetAlphaImages(0);
        _label.color = new Color(1, 1, 1, 0);
    }

    private void SetAlphaImages(int alpha)
    {
        foreach (Image image in _images) 
        { 
            image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);    
        }
    }

    public void OnHpDecrease(int currentHp, int oldHp, int maxHp)
    {
        if (!gameObject.activeInHierarchy)
        {
            return;
        }

        _label.color = new Color(1,1,1,1);
        _label.text = $"{currentHp}/{maxHp}";
        SetAlphaImages(1);
        StopAllCoroutines();

        StartCoroutine(LifeRoutine());
        StartCoroutine(ShowHpRoutine(currentHp, oldHp, maxHp));
    }

    private IEnumerator LifeRoutine()
    {
        yield return new WaitForSeconds(_lifetime);
        SetAlphaImages(0);
        _label.color = new Color(1, 1, 1, 0);
    }
    
    private IEnumerator ShowHpRoutine(float currentHp, float oldHp, float maxHp)
    {
        float timeElapsed = 0;
        float startFill = oldHp / maxHp;
        float endFill =  currentHp / maxHp;
        _hpBar.fillAmount = currentHp / maxHp;

        while (timeElapsed < _showHpDuration)
        {
            float t = timeElapsed / _showHpDuration;
            t = t * t * (3f - 2f * t);

            _hpShadow.fillAmount = Mathf.Lerp(startFill, endFill, t);

            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }
}
