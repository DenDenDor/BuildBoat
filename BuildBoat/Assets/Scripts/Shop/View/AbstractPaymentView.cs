using System;
using System.Linq;
using TMPro;
using UnityEngine;

public abstract class AbstractPaymentView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _price;
    [SerializeField] protected ShopItemType _shopItemType;

    private void Start()
    {
        UpdatePrice();
    }

    private void UpdatePrice()
    {
        if (_price == null)
        {
            _price = GetComponentsInChildren<TextMeshProUGUI>().FirstOrDefault(x => x.text.Contains("YAN"));
        }

        _price.text = SDKMediator.Instance.SDKAdapter.GetPrice(_shopItemType.ToString());
    }
}
