using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Education : MonoBehaviour
{
    [SerializeField] private Image _twinklePanel;
    [SerializeField] private float _panelTwinkleDuration;
    [SerializeField] private AnimationCurve _panelTwinkleCurve;
    [SerializeField] private float _panelMaxAlfa;
    [SerializeField] private TextMeshProUGUI _label;
    [SerializeField] private Transform _playerMovement;


    private PlayerProfile _profile;

    private void Start()
    {
        _profile = FindObjectOfType<PlayerProfile>();

        if (_profile.EducationShowCountCurrent < _profile.EducationShowCountMax)
        {
            _profile.IncreaseEducationShowCount();
            Show();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        _label.transform.LookAt(_playerMovement.position);
        _label.transform.Rotate(Vector3.up * -180);
    }
    public void Show()
    {
        StopAllCoroutines();
        StartCoroutine(PanelTwinkingRoutine());
    }

    public IEnumerator PanelTwinkingRoutine()
    {
        while (true)
        {
            float timeElapsed = 0;
            while (timeElapsed < _panelTwinkleDuration)
            {
                
                _twinklePanel.color = new Color(_twinklePanel.color.r, _twinklePanel.color.g, _twinklePanel.color.b, _panelTwinkleCurve.Evaluate(timeElapsed / _panelTwinkleDuration) * _panelMaxAlfa);

                timeElapsed += Time.deltaTime;
                yield return null;
            }
            _twinklePanel.color = new Color(_twinklePanel.color.r, _twinklePanel.color.g, _twinklePanel.color.b, 0);
        }

    }
    public void OnRockAdded(int currentCount, int maxCount)
    {
        if (currentCount > 0)
        {
            Destroy(gameObject);
        }
    }
}
