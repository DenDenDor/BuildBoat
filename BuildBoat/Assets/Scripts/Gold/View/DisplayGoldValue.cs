using TMPro;
using UnityEngine;

public class DisplayGoldValue : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _amount;

    public void UpdateAmount(int amount)
    {
        _amount.text = amount.ToString();
    }
}
