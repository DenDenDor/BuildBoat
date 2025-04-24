using System;
using DG.Tweening;
using UnityEngine;
using Object = UnityEngine.Object;

public class BlockView : MonoBehaviour
{
   [SerializeField] private MeshRenderer _meshRenderer;
   [SerializeField] private Transform _child;
   [SerializeField] private ParticleSystem _particleSystem;
   
   public void UpdateRenderer(Material newMaterial)
   {
      _meshRenderer.material = newMaterial;
   }

   private void Awake()
   {
      Vector3 pos = _child.transform.localScale;

      _child.transform.localScale = Vector3.zero;
      
      _child.transform.DOScale(pos, 0.25f).SetEase(Ease.OutBounce);
   }

   public void DestroyBlock()
   {
      ParticleSystem particleSystem1 = Instantiate(_particleSystem, transform.position, Quaternion.identity);
      Destroy(particleSystem1.gameObject, 5);
      _child.transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBounce).OnComplete(() => Destroy(gameObject));
   }
}
