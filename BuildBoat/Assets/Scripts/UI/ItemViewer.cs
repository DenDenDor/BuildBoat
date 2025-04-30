using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemViewer : MonoBehaviour
{
    [SerializeField] private Image _itemImage;
    [SerializeField] private TMP_Text _countText;

    public void View(Sprite sprite, int count, Color color)
    {
        _countText.text = count.ToString();
        _itemImage.sprite = sprite;
    }
}