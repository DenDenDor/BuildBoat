using UnityEngine;

public class AbstractPanelState : MonoBehaviour
{
    private bool _isPanelOpen;
    private CanvasGroup _canvasGroup;
    private AbstractPanelAnimation _animation;

    public void Initialize(CanvasGroup canvasGroup, AbstractPanelAnimation animation)
    {
        _canvasGroup = canvasGroup;
        _animation = animation;
    }

    public void OpenPanel()
    {
        if (_isPanelOpen) return;
        
        _isPanelOpen = true;
        _canvasGroup.gameObject.SetActive(true);
        _animation.PlayOpenAnimation();
    }

    public void ClosePanel()
    {
        if (!_isPanelOpen) return;
        
        _isPanelOpen = false;
        _animation.PlayCloseAnimation();
    }

    public void ForceClose()
    {
        _isPanelOpen = false;
        _animation.ForceClose();
        _canvasGroup.gameObject.SetActive(false);
    }

    public bool IsPanelOpen => _isPanelOpen;
}