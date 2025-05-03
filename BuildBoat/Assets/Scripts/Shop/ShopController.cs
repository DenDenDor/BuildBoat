using UnityEngine;
using System;

public class ShopController : MonoBehaviour
{
    private static ShopController _instance;

    public static ShopController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ShopController>();

                if (_instance == null)
                {
                    throw new NotImplementedException("ShopController not found!");
                }
            }

            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        _instance = this;
    }

    public void BuyPayment(ShopItemType shopItemType)
    {
        switch (shopItemType)
        {
            case ShopItemType.TwoHundred:
                GoldController.Instance.AddGold(200);
                break;
            case ShopItemType.SixHundredFifty:
                GoldController.Instance.AddGold(650);
                break;
            case ShopItemType.OneThousand:
                GoldController.Instance.AddGold(1000);
                break;
            case ShopItemType.ThreeThousand:
                GoldController.Instance.AddGold(3000);
                break;
            case ShopItemType.Cannons:
                for (int i = 0; i < 3; i++)
                {
                    InventoryController.Instance.AddBlock(BlockType.Cannon);
                }
                break;
            case ShopItemType.Blocks:
                for (int i = 0; i < 10; i++)
                {
                    InventoryController.Instance.AddBlock(BlockType.Gold);
                }
                break;
            case ShopItemType.Motors:
                for (int i = 0; i < 2; i++)
                {
                    InventoryController.Instance.AddBlock(BlockType.Motor);
                }
                break;
        }
    }
}