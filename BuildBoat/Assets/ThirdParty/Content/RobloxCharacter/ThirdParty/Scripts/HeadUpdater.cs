using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadUpdater : MonoBehaviour
{
  [SerializeField] private SkinnedMeshRenderer _skinnedMeshRenderer;
  [SerializeField] private Material _material;
  [SerializeField] private PlayerDamageFlash _playerDamageFlash;
  private IEnumerator Start()
  {
      _playerDamageFlash = FindObjectOfType<PlayerDamageFlash>();
    
      _skinnedMeshRenderer.sharedMaterial = _playerDamageFlash.GetMaterialByDevice();
    yield return new WaitForEndOfFrame();
    
    _skinnedMeshRenderer.sharedMaterial = _playerDamageFlash.GetMaterialByDevice();  
  }
}
