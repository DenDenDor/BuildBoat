using UnityEngine;

public class ShopRouter : IRouter
{
    private ShopWindow Window => UiController.Instance.GetWindow<ShopWindow>();

    public void Init()
    {
        Debug.Log("INIT");
        Window.ShopButtonOpen.Opened += OnOpen;
        Window.ShopPanel.Closed += OnClosed;
    }

    private void OnOpen()
    {
        Debug.Log("RO ! ");
        Window.ShopPanel.OpenPanel();
    }

    private void OnClosed()
    {
        Window.ShopPanel.ClosePanel();
    }

    public void Exit()
    {
        
    }
}