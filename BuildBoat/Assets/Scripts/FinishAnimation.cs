using UnityEngine;
using DG.Tweening;

public class FinishAnimation : MonoBehaviour
{
    [SerializeField] private ParticleSystem _goldEffect;
    [SerializeField] private ParticleSystem _confettiEffect;
    
    [SerializeField] private Transform _chestTopPart;
    [SerializeField] private Transform _chest;
    [SerializeField] private GameObject _lock;

    [SerializeField] private float _xAngleTopPart;

    public void Finish()
    {
        _lock.SetActive(true);
        DOTween.Sequence()
            .Append(_chest.DOMoveY(0.5f, 0.09f))
            .Append(_chest.DOMoveY(0f, 0.09f))
            .Append(_chest.DOMoveY(0.5f, 0.09f))
            .Append(_chest.DOMoveY(0f, 0.09f))
            .Append(_chestTopPart.DORotate(new Vector3(_xAngleTopPart, 0, 0), 0.06f))
            .AppendCallback(GoldEffectOn)
            .AppendInterval(1.1f)
            .OnComplete(() => _confettiEffect.Play());
    }

    private void GoldEffectOn()
    {
        _goldEffect.Play();
    }
}
