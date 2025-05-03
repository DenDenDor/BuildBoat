using System;
using UnityEngine;
using UnityEngine.UI;

public class BuyMoneyView : MonoBehaviour
{
    [SerializeField] private ShopItemType _shopItemType;

    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        
        _button.onClick.AddListener(() => ShopController.Instance.BuyPayment(_shopItemType));
    }
}
