using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Seller : MonoBehaviour
{
    [SerializeField] private GameObject _coinPrefab;
    [SerializeField] private int _coinPerMoney;
    [SerializeField] private float _playerProfileDelay;
    [SerializeField] private int _maxCoinsCount;
    [SerializeField] private UnityEvent _sell;

    private bool _loadingScreenOpened = false;

    public void Sell(int money)
    {
        StartCoroutine(SellRoutine(money));
    }

    public void LoadingScreenOpened(bool opened) 
    { 
        _loadingScreenOpened = opened;
    }

    private IEnumerator SellRoutine(int money)
    {
        if (money > 0 )
        {
            int coinsCount = money / _coinPerMoney;

            if (coinsCount > _maxCoinsCount)
            {
                coinsCount = _maxCoinsCount;
            }

            while (_loadingScreenOpened)
            {
                yield return null;
            }



            for (int i = 0; i < coinsCount; i++)
            {
                Instantiate(_coinPrefab, transform.position, Quaternion.identity);
            }

            yield return new WaitForSeconds(_playerProfileDelay);
            
            _sell.Invoke();
        }
    }
}
