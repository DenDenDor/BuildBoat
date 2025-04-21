using System;
using System.Collections;
using System.Collections.Generic;

#if DOTWEEN
using DG.Tweening;
#endif

using UnityEngine;
using UnityEngine.EventSystems;

public class InputButton : MonoBehaviour, IPointerClickHandler
{
    private Vector3 _originalScale;

    private bool _isClicked = true;

    private Coroutine _coroutine;
    
    public event Action Clicked;
    public event Action StopClicked;
    
    private void Start()
    {
        _originalScale = transform.localScale;

        Clicked += OnClick;
    }

    private void OnClick()
    {
        #if DOTWEEN
        transform.DOScale(_originalScale * 1.1f, 0.2f) 
            .OnComplete(() =>
            {
                transform.DOScale(_originalScale, 0.2f); 
            });
            #endif
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }

        if (_isClicked)
        {
            Clicked?.Invoke();
        }
        
        _coroutine = StartCoroutine(Cooldown());
    }

    private IEnumerator Cooldown()
    {
        _isClicked = false;
        yield return new WaitForSeconds(0.2f);
        StopClicked?.Invoke();
        _isClicked = true;
    }
}
