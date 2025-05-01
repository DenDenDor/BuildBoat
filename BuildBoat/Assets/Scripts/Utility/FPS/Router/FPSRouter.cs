using UnityEngine;

public class FPSRouter : IRouter
{
    private FPSWindow Window => UiController.Instance.GetWindow<FPSWindow>();

    public void Init()
    {
        FPSController.Instance.LowFPS += OnGetLowFPS;
    }

    private void OnGetLowFPS()
    {
        Window.UpdateMobileProfile();
        FPSController.Instance.LowFPS -= OnGetLowFPS;
    }

    public void Exit()
    {
        
    }
}