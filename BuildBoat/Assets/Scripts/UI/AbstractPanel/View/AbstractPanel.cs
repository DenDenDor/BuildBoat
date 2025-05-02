using UnityEngine;

public abstract class AbstractPanel : MonoBehaviour
{
    private AbstractPanelState _state;
    private AbstractPanelAnimation _animation;
    private AbstractPanelChildrenHandler _childrenHandler;
    private CanvasGroup _canvasGroup;
    private Vector3 _originalScale;

    private void Start()
    {
        _childrenHandler = gameObject.AddComponent<AbstractPanelChildrenHandler>();
        Transform childrenHolder = _childrenHandler.Initialize();
        _originalScale = childrenHolder.localScale;

        CreateBackgroundPanel();
        
        _animation = gameObject.AddComponent<AbstractPanelAnimation>();
        _animation.Initialize(_canvasGroup, childrenHolder, _originalScale);
        
        _state = gameObject.AddComponent<AbstractPanelState>();
        _state.Initialize(_canvasGroup, _animation);
        
        _state.ForceClose();
    }

    private void CreateBackgroundPanel()
    {
        BackgroundPanel prefab = FactoryController.Instance.FindPrefab<BackgroundPanel>();
        Canvas canvas = GetComponentInParent<Canvas>();
        BackgroundPanel background = Instantiate(prefab, canvas.transform);
        background.transform.SetParent(transform);
        _canvasGroup = background.GetComponent<CanvasGroup>();
        background.transform.SetAsFirstSibling();
    }

    public void OpenPanel() => 
        _state.OpenPanel();
    
    public void ClosePanel() => 
        _state.ClosePanel();
}
