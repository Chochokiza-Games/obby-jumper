using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using YG;
using Random = UnityEngine.Random;

public class SpinWheel : MonoBehaviour
{
    public enum RewardType
    {
        Money, 
        PetEgg,
        Other
    }
    [SerializeField] private float _cellDegrees;
    [SerializeField] private int _delay;
    [Header("GameObjects")]
    [SerializeField] private UIHider _hider;
    [SerializeField] private GameObject _wheelBase;
    [SerializeField] private GameObject _wheel;
    [SerializeField] private GameObject[] _previews;
    [SerializeField] private RewardPreview _rewardPreview;
    [SerializeField] private PetOpening _petOpening;
    [SerializeField] private GameObject _closeButton;
    [SerializeField] private GameObject _spinWheelPanel;
    [SerializeField] private Image _lockCircle;
    [SerializeField] private Image _spinButton;
    [SerializeField] private TextMeshProUGUI _spinButtonText;
    [SerializeField] private Color _lockColor;
    [SerializeField] private Color _lockTextColor;
    [SerializeField] private AdInitiator _adInitiator;
    [SerializeField] private AnimatedButton _buttonsSpinButton;
    [Header("Animations")]
    [SerializeField] private float _idleRotationSpeed;
    [SerializeField] private AnimationCurve _spinCurve;
    [SerializeField] private float _spinDuration;
    [SerializeField] private AnimationCurve _switchToRewardCurve;
    [SerializeField] private float _switchToRewardDuration;
    [Header("Reward Info")]
    [SerializeField] private SpinWheelRewardInfo[] _info;
    [Header("Events")]
    [SerializeField] private UnityEvent _adReloaded;

    private int _rewardSpinId = 1;
    private bool _idle = true;
    private PlayerProfile _profile;
    private Vector3 _wheelBaseStartPosition;
    private bool _locked = false;
    private Color _buttonColor;
    private LanguageTranslator.Languages _currentLanguage;

    private void Awake()
    {
        _wheelBaseStartPosition = _wheelBase.transform.position;
        _currentLanguage = FindObjectOfType<LanguageTranslator>().CurrentLangunage;
    }

    private void Start()
    {
        StartCoroutine(WaitDelayRoutine());
        _buttonColor = _spinButton.color;
        _profile = FindObjectOfType<PlayerProfile>();
        for (int i = 0; i < _info.Length; i++)
        {
            _previews[i].GetComponent<SpinWheelRewardPreview>().InitFromInfo(_info[i]);
        }


    }

    private void Update()
    {   
        if (_idle)
        {
            _wheel.transform.eulerAngles -= Vector3.forward * _idleRotationSpeed * Time.deltaTime;
        }
    }

    private void Spin()
    {
        _idle = false;
        _profile.SaveCloud();
        _adInitiator.ShowRewardAd(_rewardSpinId);
    }

    private IEnumerator SpinRoutine()
    {
        _spinButton.gameObject.SetActive(false);
        float timeElapsed = 0;
        while (timeElapsed < _spinDuration)
        {
            _wheel.transform.eulerAngles += Vector3.forward * _spinCurve.Evaluate(timeElapsed / _spinDuration);
            if (_wheel.transform.eulerAngles.z >= 360)
            {
                _wheel.transform.eulerAngles = Vector3.zero;
            }
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        int cell_id = (int)(Mathf.Ceil(_wheel.transform.eulerAngles.z / _cellDegrees));
        if (cell_id >= 6)
        {
            cell_id = 0;
        }

        timeElapsed = 0;
        while (timeElapsed < _switchToRewardDuration)
        {
            float percentPosition = _switchToRewardCurve.Evaluate(timeElapsed / _switchToRewardDuration);
            _wheelBase.transform.position = new Vector3(_wheelBase.transform.position.x, _wheelBase.transform.position.y - Screen.height * percentPosition, 0);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        Debug.Log(_info[cell_id].RewardType);
        switch (_info[cell_id].RewardType)
        {
            case RewardType.Money:
                _profile.IncreaseMoney(_info[cell_id].Amount);
                _hider.ShowOther(gameObject);
                _rewardPreview.Show(_info[cell_id].Icon, _currentLanguage == LanguageTranslator.Languages.Russian ? _info[cell_id].RuName : _info[cell_id].EnName);
                break;
            case RewardType.PetEgg:
                _petOpening.Show();
                break;

        }
        StartCoroutine(WaitDelayRoutine());
        _spinWheelPanel.SetActive(false);
    }

    public void OnSpinButtonClick() 
    {
        if (!_locked)
        {
            Spin();
        }
    }

    public void Show()
    {
        _idle = true;
        _spinWheelPanel.SetActive(true);
        _spinButton.gameObject.SetActive(true);
        _closeButton.SetActive(true);
        _wheelBase.transform.position = _wheelBaseStartPosition;
        _wheel.transform.eulerAngles = Vector3.forward * Random.Range(0f, 360f);
/*        if (_lockCircle.fillAmount != 0f)
        {
            _lockCircle.color = Color.white;
        }
        else
        {
            _lockCircle.color = new Color(0, 0, 0, 0);
        }*/
    }

    public void OnRewardSpinGet(int id)
    {
        if (id == _rewardSpinId)
        {
            _spinButton.gameObject.SetActive(false);
            _closeButton.SetActive(false);
            StartCoroutine(SpinRoutine());
        }
    }

    private IEnumerator WaitDelayRoutine()
    {
        _locked = true;
        _spinButton.color = _lockColor;
        _spinButtonText.color = _lockTextColor;
        for (int i = 0; i < _delay; i++)
        {
            yield return new WaitForSeconds(1);
            _lockCircle.fillAmount = 1f - (float)((float)(i) / (float)(_delay));
        }
        _lockCircle.fillAmount = 0f;
        _spinButton.color = _buttonColor;
        _spinButtonText.color = Color.white;
        _locked = false;

        _adReloaded.Invoke();
        StartCoroutine(TryStartPulsateSpinButton());
    }

    private IEnumerator TryStartPulsateSpinButton()
    {
        while(!_buttonsSpinButton.gameObject.activeInHierarchy)
        {
            yield return new WaitForSeconds(1);
        }

        _buttonsSpinButton.StartPulsate();
    }
}
