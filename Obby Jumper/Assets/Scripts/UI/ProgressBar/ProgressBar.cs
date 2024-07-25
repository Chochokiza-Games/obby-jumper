using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _currentLevel;
    [SerializeField] private TextMeshProUGUI _nextLevel;
    [SerializeField] private Image _bar;
    [SerializeField] private float _refreshDuration;

    private float _newAmount;
    
    public void OnLoadEvent()
    {
        _bar.fillAmount = YandexGame.savesData.progressBarAmount;
    }

    public void OnSaveEvent()
    {
        YandexGame.savesData.progressBarAmount = _bar.fillAmount;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        _bar.fillAmount = _newAmount;
    }

    public void RefreshBar(float barPercent)
    {
        StopAllCoroutines();
        _newAmount = barPercent;
        StartCoroutine(RefreshBarRoutine());
    }

    private IEnumerator RefreshBarRoutine()
    {
        float timeElapsed = 0;
        float startPosition = _bar.fillAmount;
        while (timeElapsed < _refreshDuration)
        {
            float t = timeElapsed / _refreshDuration;
            t = t * t * (3f - 2f * t);

            _bar.fillAmount = Mathf.Lerp(startPosition, _newAmount, t);
            timeElapsed += Time.deltaTime;
            yield return null;
            
        }

        _bar.fillAmount = _newAmount;
    }

}
