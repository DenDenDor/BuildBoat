using DG.Tweening;
using UnityEngine;

public class AbstractPanelAnimation : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 0.3f;
    [SerializeField] private float scaleDuration = 0.6f;
    [SerializeField] private Ease fadeEase = Ease.OutQuad;
    [SerializeField] private Ease scaleEase = Ease.OutBack;

    private CanvasGroup _canvasGroup;
    private Transform _animatedTransform;
    private Vector3 _originalScale;
    private Tween _fadeTween;
    private Tween _scaleTween;

    public void Initialize(CanvasGroup canvasGroup, Transform animatedTransform, Vector3 originalScale)
    {
        _canvasGroup = canvasGroup;
        _animatedTransform = animatedTransform;
        _originalScale = originalScale;
    }

    public void PlayOpenAnimation()
    {
        KillAllTweens();

        _canvasGroup.alpha = 0f;
        _canvasGroup.blocksRaycasts = false;
        _animatedTransform.localScale = _originalScale * 0;
        
        _fadeTween = _canvasGroup.DOFade(1f, fadeDuration)
            .SetEase(fadeEase)
            .OnStart(() => _canvasGroup.blocksRaycasts = true)
            .OnKill(() => {
                _canvasGroup.alpha = 1f;
                _canvasGroup.blocksRaycasts = true;
            });

        _scaleTween = _animatedTransform.DOScale(_originalScale, scaleDuration)
            .SetEase(scaleEase)
            .OnKill(() => _animatedTransform.localScale = _originalScale);
    }

    public void PlayCloseAnimation()
    {
        KillAllTweens();

        _fadeTween = _canvasGroup.DOFade(0f, fadeDuration)
            .SetEase(fadeEase)
            .OnStart(() => _canvasGroup.blocksRaycasts = false)
            .OnComplete(() => {
                _canvasGroup.alpha = 0f;
                _canvasGroup.blocksRaycasts = false;
            })
            .OnKill(() => {
                _canvasGroup.alpha = 0f;
                _canvasGroup.blocksRaycasts = false;
            });

        _scaleTween = _animatedTransform.DOScale(_originalScale * 0, scaleDuration)
            .SetEase(scaleEase)
            .OnKill(() => _animatedTransform.localScale = _originalScale * 0);
    }

    public void ForceClose()
    {
        KillAllTweens();
        _canvasGroup.alpha = 0f;
        _canvasGroup.blocksRaycasts = false;
        _animatedTransform.localScale = _originalScale * 0;
    }

    private void KillAllTweens()
    {
        _fadeTween?.Kill();
        _scaleTween?.Kill();
        _fadeTween = null;
        _scaleTween = null;
    }

    private void OnDestroy()
    {
        KillAllTweens();
    }
}