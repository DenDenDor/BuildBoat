using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryIcon : MonoBehaviour
{
   [SerializeField] private TextMeshProUGUI _text;
   [SerializeField] private Image _icon;
   [SerializeField] private Button _button;

   public event Action<InventoryIcon> Clicked;
   
   private void Awake()
   {
      _button.onClick.AddListener(() => Clicked?.Invoke(this));
   }

   public void UpdateIcon(Sprite sprite)
   {
      _icon.sprite = sprite;
   }

   public void UpdateText(int amount)
   {
      _text.text = amount.ToString();
   }
}
