using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerDamageFlash : MonoBehaviour
{
    public float flashDuration = 0.1f;
    public Material mobileMaterial;
    public Material pcMaterial;
    
    private Material originalMaterial; 
    private float flashTimer; 
    private SkinnedMeshRenderer[] _renderers;
    
    public Material GetMaterialByDevice()
    {
        if (DeviceData.IsMobile)
        {
            return mobileMaterial;
        }
        else
        {
            return pcMaterial;
        }
    }

    void Start()
    {
        // Получаем рендерер врага
        _renderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        // Сохраняем оригинальный материал
        originalMaterial = _renderers.First().material;
        
        mobileMaterial.SetFloat("_WhiteLerp", 0);
        pcMaterial.SetFloat("_WhiteLerp", 0);    }

    void Update()
    {
        // Если вспышка активна, обновляем таймер
        if (flashTimer > 0)
        {
            flashTimer -= Time.deltaTime;
            if (flashTimer <= 0)
            {
                // Возвращаем оригинальный материал
                mobileMaterial.SetFloat("_WhiteLerp", 0);
                pcMaterial.SetFloat("_WhiteLerp", 0);            }
        }
    }

    public void UpdateMaterial(Material curMaterial)
    {
        foreach (var item in _renderers)
        {
            item.material = curMaterial;
        }
    }

    public void TakeDamage()
    {
        // Применяем материал вспышки
        mobileMaterial.SetFloat("_WhiteLerp", 1);
        pcMaterial.SetFloat("_WhiteLerp", 1);        // Запускаем таймер вспышки
        flashTimer = flashDuration;
    }

}
