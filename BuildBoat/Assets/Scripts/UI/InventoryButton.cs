using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class InventoryButton : MonoBehaviour
{
    private Button _button;

    private bool _selected;
    
    [SerializeField] private Color _selectedColor = Color.white;
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
        if (_selected)
            return;
        
        _selected = true;
        _button.image.color = _selectedColor;
        float scale = 0.03f;

        transform.DOScale(Vector3.one + new Vector3(scale, scale, scale), 0.3f).SetEase(Ease.OutBounce);
        InventorySelector.Instance.SelectButton(this);
    }

    public void Deselect()
    {
        transform.DOScale(Vector3.one , 0.3f).SetEase(Ease.InBounce);

        _selected = false;
        _button.image.color = _deselectedColor;
    }
}
