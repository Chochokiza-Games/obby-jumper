using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Store : MonoBehaviour
{
    public enum StoreProducts
    {
        Egg,
        Max,
    }

    [SerializeField] private UnityEvent<StoreProducts> _productBought;
    [SerializeField] private PlayerProfile _playerProfile;
    [SerializeField] private GameObject _toastPrefab;
    [SerializeField] private GameObject _confirmWindow;

    private StoreProduct _pickedProduct;
    private GameObject _toast;

    private void OnDisable()
    {
        if (_toast != null)
        {
            Destroy(_toast);
            _toast = null;
        }
    }

    public void TryBuy(StoreProduct product)
    {
        if (_playerProfile.CanBuy(product.Price))
        {
            _pickedProduct = product;
            _confirmWindow.SetActive(true);
        }
        else
        {
            _toast = Instantiate(_toastPrefab, transform);
        }
    } 

    public void OnBuy()
    {
        if (_playerProfile.Buy(_pickedProduct.Price))
        {
            _productBought.Invoke(_pickedProduct.Id);
        }
    }
}
