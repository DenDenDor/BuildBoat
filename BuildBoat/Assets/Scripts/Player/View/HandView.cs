using System;
using DG.Tweening;
using UnityEngine;

public class HandView : MonoBehaviour
{
  private void Awake()
  {
    transform.localScale = Vector3.zero;
  }

  public void Appear()
  {
    transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBounce);
  }

  public void Disappear()
  {
    transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBounce);
  }
}
