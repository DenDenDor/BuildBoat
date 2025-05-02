using System;
using UnityEngine;
using UnityEngine.UI;

public class ShopButtonOpen : MonoBehaviour
{
    [SerializeField] private Button _close;

    public event Action Opened;
    
    private void Awake()
    {
        _close.onClick.AddListener(() =>
        {
            Debug.LogError("OPENED!");
            Opened?.Invoke();
        });
    }
}
