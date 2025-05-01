using UnityEngine;
using UnityEngine.UI;

public class ChestButton : MonoBehaviour
{
    [SerializeField] private LootType _chestRare;
    [SerializeField] private Color _rareColor;
    [SerializeField] private Sprite _chestImage;
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(Open);
    }

    private void Open()
    {
        LootPanel.Instance.LootPanelOpen(_chestImage, _rareColor, _chestRare);
    }
}
