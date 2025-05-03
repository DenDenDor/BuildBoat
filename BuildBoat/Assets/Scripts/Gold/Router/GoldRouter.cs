using UnityEngine;

public class GoldRouter : IRouter
{
    private GoldWindow Window => UiController.Instance.GetWindow<GoldWindow>();

    public void Init()
    {
        GoldController.Instance.UpdatedCount += OnUpdateAmount;
        OnUpdateAmount();
    }

    private void OnUpdateAmount()
    {
        Window.UpdateAmount(SDKMediator.Instance.GenerateSaveData().GoldAmount);
    }

    public void Exit()
    {
        
    }
}