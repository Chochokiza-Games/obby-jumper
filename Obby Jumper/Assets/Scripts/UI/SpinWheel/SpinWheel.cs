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
    [SerializeField] private int _delay;
    [Header("GameObjects")]
    [SerializeField] private GameObject _dimed;
    [SerializeField] private GameObject _wheelBase;
    [SerializeField] private GameObject _wheel;
    [SerializeField] private GameObject[] _previews;
    [SerializeField] private RewardPreview _rewardPreview;
    [SerializeField] private PetOpening _petOpening;
    [SerializeField] private GameObject _closeButton;
    [SerializeField] private Image _lockCircle;
    [SerializeField] private Button _spinButton;
    [SerializeField] private TextMeshProUGUI _spinButtonText;
    [SerializeField] private Color _lockTextColor;
    [SerializeField] private AdInitiator _adInitiator;
    [Header("Animations")]
    [SerializeField] private float _idleRotationSpeed;
    [SerializeField] private AnimationCurve _spinCurveOpening;
    [SerializeField] private float _spinOpeningDuration;
    [SerializeField] private int _guarantedLaps;
    [SerializeField] private AnimationCurve _spinCurveClosening;
    [SerializeField] private float _spinCloseningDuration;
    [SerializeField] private AnimationCurve _switchToRewardCurve;
    [SerializeField] private float _switchToRewardDuration;
    [SerializeField] private AnimationCurve _spinCurve;
    [SerializeField] private float _spinDuration;

    [Header("Reward Info")]
    [SerializeField] private SpinWheelRewardInfo[] _info;
    [Header("Events")]
    [SerializeField] private UnityEvent _adReloaded;

    private float _cellDegrees;
    private int _rewardSpinId = 1;
    private bool _idle = true;
    private PlayerProfile _profile;
    private Vector3 _wheelBaseStartPosition;
    private bool _locked = false;
    private Color _buttonColor;
    private LanguageTranslator.Languages _currentLanguage;

    private void Awake()
    {
        _cellDegrees = 360f / ((float)(_info.Length));
        _wheelBaseStartPosition = _wheelBase.transform.position;
        _currentLanguage = FindObjectOfType<LanguageTranslator>().CurrentLangunage;
    }

    private void Start()
    {
        StartCoroutine(WaitDelayRoutine());
        //_buttonColor = //_spinButton.image.color;
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
            if (_wheel.transform.eulerAngles.z < 0f)
            {
                _wheel.transform.eulerAngles = Vector3.forward * 360f;
            }
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
        float timeElapsed = 0;
        while (timeElapsed < _spinOpeningDuration)
        {
            _wheel.transform.eulerAngles = Vector3.forward * _spinCurveOpening.Evaluate(timeElapsed / _spinOpeningDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        _wheel.transform.eulerAngles = Vector3.zero;

        int cell = Random.Range(0, _info.Length);

        Debug.Log($"CELL {cell} {_info[cell].EnName}");
        float degree = (360f * _guarantedLaps) + ((cell * _cellDegrees) + (_cellDegrees / 2f));

        timeElapsed = 0;
        float startRotation = 0;
        while (timeElapsed < _spinDuration)
        {
            _wheel.transform.eulerAngles = Vector3.Lerp(new Vector3(0, 0, startRotation), new Vector3(0, 0, degree), _spinCurve.Evaluate(timeElapsed / _spinDuration));
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        _wheel.transform.eulerAngles = Vector3.forward * degree;

        timeElapsed = 0;
        Vector3 startEulers = _wheel.transform.eulerAngles;
        while (timeElapsed < _spinCloseningDuration)
        {
            _wheel.transform.eulerAngles = startEulers + (Vector3.forward * _spinCurveClosening.Evaluate(timeElapsed / _spinCloseningDuration));
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        _wheel.transform.eulerAngles = Vector3.forward * degree;

        switch (_info[cell].RewardType)
        {
            case RewardType.Money:
                Debug.Log($"Money {_info[cell].Amount}");
                _profile.IncreaseMoney(_info[cell].Amount);
                //_hider.ShowOther(gameObject);
                _rewardPreview.Show(_info[cell].Icon, _currentLanguage == LanguageTranslator.Languages.Russian ? _info[cell].RuName : _info[cell].EnName);
                break;
            case RewardType.PetEgg:
                Debug.Log("Pet Egg");
                _petOpening.Show();
                break;

        }
        _dimed.SetActive(false);
        _wheelBase.SetActive(false);
        StartCoroutine(WaitDelayRoutine());
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
        _dimed.SetActive(true);
        _wheelBase.SetActive(true);
        _closeButton.SetActive(true);
        _wheelBase.transform.position = _wheelBaseStartPosition;
        _wheel.transform.eulerAngles = Vector3.forward * Random.Range(0f, 360f);
    }

    public void OnRewardSpinGet(int id)
    {
        if (id == _rewardSpinId)
        {
            _spinButton.interactable = false;
            _spinButtonText.color = _lockTextColor;
            _closeButton.SetActive(false);
            StartCoroutine(SpinRoutine());
        }
    }

    private IEnumerator WaitDelayRoutine()
    {
        _locked = true;
        _spinButton.interactable = false;
        //_spinButton.image.color = _lockColor;
        _spinButtonText.color = _lockTextColor;
        for (int i = 0; i < _delay; i++)
        {
            yield return new WaitForSeconds(1);
            _lockCircle.fillAmount = 1f - (float)((float)(i) / (float)(_delay));
        }
        _lockCircle.fillAmount = 0f;
        //_spinButton.image.color = _buttonColor;
        _spinButtonText.color = Color.white;
        _locked = false;
        _spinButton.interactable = true;

        _adReloaded.Invoke();
    }
}
