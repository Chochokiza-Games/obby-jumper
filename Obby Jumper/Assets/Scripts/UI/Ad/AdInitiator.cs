using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using YG;

public class AdInitiator : MonoBehaviour
{
    [SerializeField] private int _adDelay;
    [SerializeField] private GameObject _adInitiatorWindow;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private UnityEvent _fullscreenEnded;
    [SerializeField] private UnityEvent<int> _rewardAdShowed;
    [SerializeField] private int _adShowdelay;
    [SerializeField] private PlayerMovement _movement;
    [SerializeField] private CameraPivot _pivot;
    [SerializeField] private UIComposer _composer;
    [SerializeField] private BotMovement[] _bots;
    [SerializeField] private PetStation _petStation;
    [SerializeField] private Joystick _joystick;
    [SerializeField] private DistractionFromGameplayChecker _distractFromGameplay;

    private bool _playerInHub = true;

    private void Start()
    {
        StopAllCoroutines();
        StartCoroutine(AdInitiatorRoutine());
        YG.YandexGame.RewardVideoEvent += RewardCallback;
    }

    private void DisableBots() 
    {
        foreach(BotMovement bot in _bots) 
        {
            bot.Locked = true;
        }
    }

    private void EnableBots()
    {
        foreach(BotMovement bot in _bots) 
        {
            bot.Locked = false;
        }
    }

    private IEnumerator AdInitiatorRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(_adDelay);

            while (!_distractFromGameplay.CanDistractFromGameplay())
            {
                yield return null;
            }

            _movement.LockForEducation();
            _movement.Locked = true;
            _pivot.Locked = true;
            _composer.OpeningSomeUI();
            DisableBots();
            _petStation.DisablePets();

            _adInitiatorWindow.SetActive(true);

            for (int i = _adShowdelay; i > 0; i--)
            {
                _text.SetText($"{i}");
                yield return new WaitForSeconds(1);
            }

            YG.YandexGame.FullscreenShow();

            while (YandexGame.nowAdsShow || YandexGame.nowFullAd || YandexGame.nowVideoAd)
            {
                yield return null;
            }

            _adInitiatorWindow.SetActive(false);
            _movement.Locked = false;
            _movement.UnlockForEducation();
            _pivot.Locked = false;
            _composer.ClosingSomeUI();
            if(_joystick.gameObject)
            {
                _joystick.ResetJoystick();
            }
            EnableBots();
            _petStation.EnablePets();
            _fullscreenEnded.Invoke();
        }
    }

    public void ShowRewardAd(int id)
    {
        StartCoroutine(ShowRewardAdRoutine(id));
    }

    private IEnumerator ShowRewardAdRoutine(int id)
    {
        _adInitiatorWindow.SetActive(true);

        for (int i = _adShowdelay; i > 0; i--)
        {
            _text.SetText($"{i}");
            yield return new WaitForSeconds(1);
        }
        YG.YandexGame.RewVideoShow(id);

        _adInitiatorWindow.SetActive(false);
    }

    private void RewardCallback(int id)
    {
        _rewardAdShowed.Invoke(id);
    }
}

// public class AdInitiator : MonoBehaviour
// {
//     [SerializeField] private int _adDelay;
//     [SerializeField] private GameObject _adInitiatorWindow;
//     [SerializeField] private TextMeshProUGUI _text;
//     [SerializeField] private UnityEvent<int> _rewardAdShowed;
//     [SerializeField] private int _adShowdelay;
//     [SerializeField] private PlayerMovement _movement;
//     [SerializeField] private CameraPivot _pivot;
//     [SerializeField] private BotMovement[] _bots;
//     [SerializeField] private PetStation _petStation;

//     private void Start()
//     {
//         StopAllCoroutines();
//         StartCoroutine(AdInitiatorRoutine());
//         YG.YandexGame.RewardVideoEvent += RewardCallback;
//     }

//     private void DisableBots() 
//     {
//         foreach(BotMovement bot in _bots) 
//         {
//             bot.Locked = true;
//         }
//     }

//     private void EnableBots()
//     {
//         foreach(BotMovement bot in _bots) 
//         {
//             bot.Locked = false;
//         }
//     }

//     private IEnumerator AdInitiatorRoutine()
//     {
//         while (true)
//         {
//             yield return new WaitForSeconds(_adDelay);

//             _movement.Locked = true;
//             _pivot.Locked = true;
//             //_hider.HideOther(gameObject);
//             DisableBots();
//             _petStation.DisablePets();

//             _adInitiatorWindow.SetActive(true);

//             for (int i = _adShowdelay; i > 0; i--)
//             {
//                 _text.SetText($"{i}");
//                 yield return new WaitForSeconds(1);
//             }

//             YG.YandexGame.FullscreenShow();

//             _adInitiatorWindow.SetActive(false);
//             _movement.Locked = false;
//             _pivot.Locked = false;
//             EnableBots();
//             _petStation.EnablePets();
//         }
//     }

//     public void ShowRewardAd(int id)
//     {
//         StartCoroutine(ShowRewardAdRoutine(id));
//     }

//     private IEnumerator ShowRewardAdRoutine(int id)
//     {
//         _adInitiatorWindow.SetActive(true);

//         for (int i = _adShowdelay; i > 0; i--)
//         {
//             _text.SetText($"{i}");
//             yield return new WaitForSeconds(1);
//         }
//         YG.YandexGame.RewVideoShow(id);

//         _adInitiatorWindow.SetActive(false);
//     }

//     private void RewardCallback(int id)
//     {
//         _rewardAdShowed.Invoke(id);
//     }
// }