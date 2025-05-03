using UnityEngine;
using System;

public class GoldController : MonoBehaviour
{
    public event Action UpdatedCount;
    
    private static GoldController _instance;

    public static GoldController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GoldController>();

                if (_instance == null)
                {
                    throw new NotImplementedException("GoldController not found!");
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

    public void AddGold(int amount)
    {
        int result = SDKMediator.Instance.GenerateSaveData().GoldAmount + amount;
        
        SDKMediator.Instance.SaveGoldAmount(result);
        
        UpdatedCount?.Invoke();
    }   
    
    public void RemoveGold(int amount)
    {
        int result = SDKMediator.Instance.GenerateSaveData().GoldAmount - amount;

        if (result < 0)
        {
            result = 0;
        }
        
        SDKMediator.Instance.SaveGoldAmount(result);
        
        UpdatedCount?.Invoke();
    }
}