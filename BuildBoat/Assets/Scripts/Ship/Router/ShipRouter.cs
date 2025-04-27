using UnityEngine;

public class ShipRouter : IRouter
{
    private ShipWindow Window => UiController.Instance.GetWindow<ShipWindow>();

    public void Init()
    {
        // Window.ShipButton
    }

    public void Exit()
    {
        
    }
}