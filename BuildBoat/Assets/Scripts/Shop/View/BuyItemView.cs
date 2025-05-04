using UnityEngine;
using UnityEngine.UI;

public class BuyItemView : AbstractPaymentView
{
    [SerializeField]  private Button _button;
    
    private void Awake()
    {
        _button.onClick.AddListener(() => ShopController.Instance.BuyPayment(_shopItemType));
    }
}
