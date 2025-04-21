using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangerShaderByDevice : MonoBehaviour
{
    [SerializeField] private PlayerDamageFlash _playerDamageFlash;
    [SerializeField] private Transform _child;

    private void Awake()
    {
        Material material = _playerDamageFlash.GetMaterialByDevice();
        
        foreach (var skinnedMeshRenderer in _child.GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            skinnedMeshRenderer.material = material;
        }
    }
}
