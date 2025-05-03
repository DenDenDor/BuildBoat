using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChestButton : MonoBehaviour
{
    [SerializeField] private LootType _chestRare;
    [SerializeField] private Color _rareColor;
    [SerializeField] private Sprite _chestImage;
    private Button _button;
    private int _amount;
    
    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(Open);

        _amount = int.Parse(GetComponentInChildren<TextMeshProUGUI>().text);
    }

    private void Open()
    {
        if (_amount <= SDKMediator.Instance.GenerateSaveData().GoldAmount)
        {
            LootPanel.Instance.LootPanelOpen(_chestImage, _rareColor, _chestRare);
            GoldController.Instance.RemoveGold(_amount);
        }
    }
}
