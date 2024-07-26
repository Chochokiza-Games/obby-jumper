using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Profiling;
using UnityEngine.UI;

public class PetOpening : MonoBehaviour
{
    public enum SwipeDirection
    {
        Left,
        Right,
        Up,
        Down
    }

    [SerializeField] private UnityEvent _windowOpened;
    [SerializeField] private UnityEvent _cutMade;
    [Header("Egg Stats")]
    [SerializeField] private int _maxHealth;
    [Header("Objects")]
    [SerializeField] private GameObject _egg;
    [SerializeField] private GuideHand _hand;
    [SerializeField] private Image _whiteScreen;
    [SerializeField] private GameObject _base;
    [SerializeField] private PlayerInventory _petInventory;
    [SerializeField] private RewardPreview _rewardPreview;
    [SerializeField] private UIHider _hider;
    [SerializeField] private LanguageTranslator _translator;
    [Header("Animations")]
    [SerializeField] private AnimationCurve _eggWiggleCurve;
    [SerializeField] private float _eggWiggleDuration;
    [SerializeField] private float _eggWigglePauseTime;
    [SerializeField] private AnimationCurve _eggHurtCurve;
    [SerializeField] private float _eggHurtDuration;
    [SerializeField] private float _whiteScreenShowDuration;
    [SerializeField] private AnimationCurve _whiteScreenAlphaCurve;
    [Header("Swiping")]
    [SerializeField] private float _swipeMinLenght;
    [SerializeField] private float _swipeThreshold;
    [Header("Prefabs")]
    [SerializeField] private GameObject _cutPrefab;

    private int _health;

    private bool _swiping = false;
    private Vector2 _swipeStartPosition = Vector2.zero;
    private Vector2 _swipeCurrentPosition = Vector2.zero;

    private PlayerProfile _profile;

    private void Start()
    {
        _profile = FindObjectOfType<PlayerProfile>();

        _health = _maxHealth;
        StartCoroutine(EggWiggleRoutine());
    }

    public void OnEggPickedFromPopup(int id)
    {
        Show();
    }

    private void Update()
    {
        if (_health <= 0)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (!_swiping)
            {
                _swiping = true;
                _swipeStartPosition = Input.mousePosition;
            }
        }
        if (Input.GetMouseButton(0))
        {
            if (_swiping)
            {
                _swipeCurrentPosition = Input.mousePosition;
                int direction = -1;

                if (Vector2.Distance(_swipeStartPosition, _swipeCurrentPosition) >= _swipeMinLenght)
                {
                    if (Mathf.Abs(_swipeStartPosition.y - _swipeCurrentPosition.y) <= _swipeThreshold)
                    {
                        if (_swipeStartPosition.x > _swipeCurrentPosition.x)
                        {
                            Debug.Log("To the left");
                            direction = (int)SwipeDirection.Left;
                        }
                        else
                        {
                            Debug.Log("To the right");
                            direction = (int)SwipeDirection.Right;
                        }

                        _swiping = false;
                    }
                    else if (Mathf.Abs(_swipeStartPosition.x - _swipeCurrentPosition.x) <= _swipeThreshold)
                    {
                        if (_swipeStartPosition.x > _swipeCurrentPosition.x)
                        {
                            Debug.Log("To the Up");
                            direction = (int)SwipeDirection.Up;
                        }
                        else
                        {
                            Debug.Log("To the Down");
                            direction = (int)SwipeDirection.Down;
                        }

                        _swiping = false;
                    }
                }

                if (direction != -1)
                {
                    EggHurt((SwipeDirection)direction);
                }
            }
        }
        else
        {
            _swiping = false;
        }
    }

    public void Show()
    {
        _windowOpened.Invoke();
        _whiteScreen.gameObject.SetActive(false);
        _health = _maxHealth;
        gameObject.SetActive(true);
        _hand.Enable();
        _swiping = false;
        _base.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(EggWiggleRoutine());
    }

    private void EggHurt(SwipeDirection direction)
    {
        _cutMade.Invoke();
        _hand.Disable();
        Instantiate(_cutPrefab, _base.transform).GetComponent<CutEffect>().Initialize(direction);
        _health--;
        if (_health <= 0)
        {
            StartCoroutine(ShowWhiteScreenRoutine());
        }
        else
        {
            StopCoroutine(EggHurtRoutine());
            StartCoroutine(EggHurtRoutine());
        }
    }

    private IEnumerator ShowWhiteScreenRoutine()
    {
        float timeElapsed = 0;
        _whiteScreen.gameObject.SetActive(true);
        while (timeElapsed < _whiteScreenShowDuration)
        {
            if (timeElapsed >= _whiteScreenShowDuration / 2 && _base.activeSelf)
            {
                _base.SetActive(false);
            }
            _whiteScreen.color = new Color(1,1,1, _whiteScreenAlphaCurve.Evaluate(timeElapsed / _whiteScreenShowDuration));
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        int id;
        if (_profile.PetDropOrderCurrentId < _profile.PetDropOrder.Length)
        {
            id = _petInventory.PushItem((BaseInventoryItem.ItemId)_profile.PetDropOrder[_profile.PetDropOrderCurrentId]);
            _profile.IncreasePetDropOrderCurrentId();

        }
        else
        {
            id = _petInventory.PushItem((BaseInventoryItem.ItemId)Random.Range(0, (int)BaseInventoryItem.ItemId.PetMax));
        }

        PetItem item = (PetItem)_petInventory.GetItemById(id);
        _rewardPreview.Show(item.Icon, _translator.CurrentLangunage == LanguageTranslator.Languages.Russian ? item.RuLabel : item.EnLabel);
        _hider.ReturnToDefault();
        //_hider.ShowOther(gameObject);
        gameObject.SetActive(false);
    }

    private IEnumerator EggHurtRoutine()
    {
        float timeElapsed = 0;
        while (timeElapsed < _eggHurtDuration)
        {
            _egg.transform.localScale = Vector3.one * _eggHurtCurve.Evaluate(timeElapsed / _eggHurtDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator EggWiggleRoutine()
    {
        while (true)
        {
            float timeElapsed = 0;
            while (timeElapsed < _eggWiggleDuration)
            {
                _egg.transform.eulerAngles = Vector3.forward * _eggWiggleCurve.Evaluate(timeElapsed / _eggWiggleDuration);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            _egg.transform.eulerAngles = Vector3.zero;
            yield return new WaitForSeconds(_eggWigglePauseTime);
        }
    }
}
