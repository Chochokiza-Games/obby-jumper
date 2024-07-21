using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StoreSlot : MonoBehaviour
{
    [SerializeField] private StoreProduct _baseProduct;
    [SerializeField] private Store.StoreProducts _id;
    [SerializeField] private float _priceGrowPercent;
    [SerializeField] private TextMeshProUGUI _priceLabel;
    [SerializeField] private Store _store;
    [SerializeField] private PlayerProfile _profile;

    private int _currentPrice;

    public void TryBuy()
    {
        StoreProduct product = Instantiate(_baseProduct);
        product.Price = _currentPrice;

        _store.TryBuy(product);
    }

    public void OnBuy(Store.StoreProducts id)
    {
        if (id == _id)
        {
            float increaseValue = _currentPrice * (_priceGrowPercent / 100f);
            if (increaseValue < 1)
            {
                increaseValue = 1;
            }
            _currentPrice += (int)increaseValue;
            _priceLabel.text = _currentPrice.ToString();
        }
    }
    public void OnSaveEvent()
    {
        YG.YandexGame.savesData.shopPrices[(int)_id] = _currentPrice;
        _priceLabel.text = _currentPrice.ToString();

    }

    public void OnLoadEvent()
    {
        int price = YG.YandexGame.savesData.shopPrices[(int)_id];
        if (price == 0)
        {
            _currentPrice = _baseProduct.Price;
        }
        else
        {
            _currentPrice = price;
        }

        _priceLabel.text = _currentPrice.ToString();
    }
}
