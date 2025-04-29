using System;
using DG.Tweening;
using UnityEngine;
using Object = UnityEngine.Object;

public abstract class BlockView : MonoBehaviour
{
   [SerializeField] private Transform _child;
  
   private Color _color;
   private ParticleSystem _particleSystem;

   public void UpdateColor(Color color, BlockParticle blockParticle)
   {
      _color = color;

      _particleSystem = blockParticle.GetComponent<ParticleSystem>();
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
      particleSystem1.startColor = _color;

      foreach (var part in particleSystem1.GetComponentsInChildren<ParticleSystem>())
      {
         part.startColor = _color;

         if (part.name == "Linger")
         {
            Debug.Log("Linger + !!! ");
         }
      }
      
      Destroy(particleSystem1.gameObject, 5);
      _child.transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBounce).OnComplete(() => Destroy(gameObject));
   }
}
