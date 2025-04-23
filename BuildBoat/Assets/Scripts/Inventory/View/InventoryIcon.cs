using TMPro;
using UnityEngine;

public class InventoryIcon : MonoBehaviour
{
   [SerializeField] private TextMeshProUGUI _text;

   public void UpdateText(int amount)
   {
      _text.text = amount.ToString();
   }
}
