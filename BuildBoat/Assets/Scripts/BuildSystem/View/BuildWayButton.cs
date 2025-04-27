using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI; // Не забудь добавить using для DoTween

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(Button))]
public class BuildWayButton : MonoBehaviour
{
    private RectTransform _rectTransform;
    private Vector2 _originalSize = new Vector2(90, 90);
    private Vector2 _enlargedSize = new Vector2(110, 110);
    private float _animationDuration = 0.3f; // Длительность анимации в секундах

    public event Action Clicked;
    
    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _rectTransform.sizeDelta = _originalSize; // Устанавливаем начальный размер
        
        GetComponent<Button>().onClick.AddListener(() => Clicked?.Invoke());
    }

    // Метод для плавного увеличения размера
    public void Increase()
    {
        _rectTransform.DOSizeDelta(_enlargedSize, _animationDuration)
            .SetEase(Ease.OutBack); // Можно изменить тип анимации по желанию
    }

    // Метод для плавного уменьшения размера
    public void Decrease()
    {
        _rectTransform.DOSizeDelta(_originalSize, _animationDuration)
            .SetEase(Ease.InBack); // Можно изменить тип анимации по желанию
    }
}