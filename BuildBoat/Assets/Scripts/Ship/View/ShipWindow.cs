using UnityEngine;

public class ShipWindow : AbstractWindowUi
{
    [SerializeField] private ShipButton _shipButton;

    public ShipButton ShipButton => _shipButton;

    public override void Init()
    {
        
    }
}