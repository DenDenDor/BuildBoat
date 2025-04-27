using System;
using UnityEngine;
using UnityEngine.UI;

public class ShipButton : MonoBehaviour
{
    [SerializeField] private Button _button;

    public event Action Clicked;

    private void Awake()
    {
        _button.onClick.AddListener(() => Clicked?.Invoke());
    }
}
