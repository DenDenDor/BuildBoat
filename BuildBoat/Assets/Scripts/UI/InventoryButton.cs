using UnityEngine;
using UnityEngine.UI;

public class InventoryButton : MonoBehaviour
{
    private Button _button;

    private bool _selected;
    
    private Color _selectedColor = Color.white;
    private Color _deselectedColor;
    
    private void Start()
    {
        InventorySelector.Instance.AddButton(this);
        
        _button = GetComponent<Button>();
        _button.onClick.AddListener(Select);
        
        _deselectedColor = _button.image.color;
    }

    private void Select()
    {
        _selected = true;
        _button.image.color = _selectedColor;
        InventorySelector.Instance.SelectButton(this);
    }

    public void Deselect()
    {
        _selected = false;
        _button.image.color = _deselectedColor;
    }
}
