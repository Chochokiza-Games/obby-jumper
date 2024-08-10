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
    [SerializeField] private UIComposer _uiComposer;

    private float _newAmount;
    
    public void OnLoadEvent()
    {
        _bar.fillAmount = YandexGame.savesData.progressBarAmount;
        // ChangeLevel(YandexGame.savesData.level);
    }

    public void ReloadBar()
    {
        StopAllCoroutines();
        _bar.fillAmount = _newAmount;
    }

    private void Start()
    {
        ChangeLevel(FindObjectOfType<PlayerProfile>().CurrentLevel);
    }

    public void OnSaveEvent()
    {
        YandexGame.savesData.progressBarAmount = _bar.fillAmount;
    }

    public void ChangeLevel(int level)
    {
        _currentLevel.text = level.ToString();
        _nextLevel.text = (level + 1).ToString();
        _bar.fillAmount = 0;
        _newAmount = 0;
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
