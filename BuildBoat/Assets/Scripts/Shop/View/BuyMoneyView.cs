using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyMoneyView : AbstractPaymentView
{
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        
        _button.onClick.AddListener(() => ShopController.Instance.BuyPayment(_shopItemType));
    }
}
