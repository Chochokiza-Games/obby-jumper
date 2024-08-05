using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class AdInitiator : MonoBehaviour
{
    [SerializeField] private int _adDelay;
    [SerializeField] private GameObject _adInitiatorWindow;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private UnityEvent<int> _rewardAdShowed;
    [SerializeField] private int _adShowdelay;
    [SerializeField] private PlayerMovement _movement;
    [SerializeField] private CameraPivot _pivot;
    //[SerializeField] private UIHider _hider;
    [SerializeField] private BotMovement[] _bots;
    [SerializeField] private PetStation _petStation;

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

            _movement.Locked = true;
            _pivot.Locked = true;
            //_hider.HideOther(gameObject);
            DisableBots();
            _petStation.DisablePets();

            _adInitiatorWindow.SetActive(true);

            for (int i = _adShowdelay; i > 0; i--)
            {
                _text.SetText($"{i}");
                yield return new WaitForSeconds(1);
            }

            YG.YandexGame.FullscreenShow();

            _adInitiatorWindow.SetActive(false);
            _movement.Locked = false;
            _pivot.Locked = false;
            //_hider.ShowOther(gameObject);
            EnableBots();
            _petStation.EnablePets();
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