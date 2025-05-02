using UnityEngine;

public class ShopWindow : AbstractWindowUi
{
    [SerializeField] private ShopButtonOpen _shopButtonOpen;
    [SerializeField] private ShopPanel _shopPanel;

    public ShopButtonOpen ShopButtonOpen => _shopButtonOpen;

    public ShopPanel ShopPanel => _shopPanel;

    public override void Init()
    {
        _shopButtonOpen = FindObjectOfType<ShopButtonOpen>();
        _shopPanel = FindObjectOfType<ShopPanel>();
        
        Debug.Log("WINDOW");
    }
}