using System;
using UnityEngine;
using UnityEngine.UI;

public class ShopPanel : AbstractPanel
{
    [SerializeField] private Button _close;

    public event Action Closed;
    
    private void Awake()
    {
        _close.onClick.AddListener(() => Closed?.Invoke());
    }
}