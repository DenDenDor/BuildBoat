using UnityEngine;

public class GoldWindow : AbstractWindowUi
{
    [SerializeField] private DisplayGoldValue _displayGoldValue;
    
    public override void Init()
    {
        
    }

    public void UpdateAmount(int amount)
    {
        _displayGoldValue.UpdateAmount(amount);
    }
}