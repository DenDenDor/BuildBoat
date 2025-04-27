using System;
using DG.Tweening;
using UnityEngine;
using Object = UnityEngine.Object;

public class BlockView : MonoBehaviour
{
   [SerializeField] private MeshRenderer _meshRenderer;
   [SerializeField] private Transform _child;
   [SerializeField] private ParticleSystem _particleSystem;
   [SerializeField] private Color _color;
   
   public void UpdateRenderer(Material newMaterial, Color color)
   {
      _meshRenderer.material = newMaterial;

      // _color = GetAverageColorFromMaterial();
      //
      // _color = GetAdjustedColor();
      _color = color;
   }

   private void Awake()
   {
      Vector3 pos = _child.transform.localScale;

      _child.transform.localScale = Vector3.zero;
      
      _child.transform.DOScale(pos, 0.25f).SetEase(Ease.OutBounce);
   }
   
   public Color GetAdjustedColor()
   {
      Color baseColor = _color;
      return AdjustColorBrightness(baseColor);
   }

   private Color AdjustColorBrightness(Color color)
   {
      // Рассчитываем яркость цвета по формуле восприятия
      float brightness = 0.299f * color.r + 0.587f * color.g + 0.114f * color.b;
    
      // Определяем коэффициент изменения яркости (20%)
      const float adjustmentFactor = 0.5f;
    
      // Если цвет темный (яркость < 0.5) - осветляем, иначе - затемняем
      if (brightness < 0.5f)
      {
         // Осветляем цвет
         return Color.Lerp(color, Color.white, adjustmentFactor);
      }
      else
      {
         // Затемняем цвет
         return Color.Lerp(color, Color.black, adjustmentFactor);
      }
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
   
   public Color GetAverageColorFromMaterial()
   {
      if (_meshRenderer == null)
      {
         Debug.LogError("MeshRenderer is not assigned!");
         return Color.white;
      }

      Material material = _meshRenderer.sharedMaterial;

      // Сначала проверяем, есть ли цвет напрямую в материале
      if (material.HasProperty("_Color"))
      {
         return material.color;
      }

      // Если нет прямого цвета, проверяем текстуру
      if (material.HasProperty("_MainTex"))
      {
         Texture2D texture = material.mainTexture as Texture2D;
         if (texture != null)
         {
            return CalculateAverageColorFromTexture(texture);
         }
      }

      Debug.LogWarning("Couldn't determine color from material, returning white");
      return Color.white;
   }

   private Color CalculateAverageColorFromTexture(Texture2D texture)
   {
      // Проверяем, доступна ли текстура для чтения
      if (!texture.isReadable)
      {
         Debug.LogError("Texture is not readable! Enable 'Read/Write Enabled' in texture import settings");
         return Color.white;
      }

      // Определяем шаг выборки для оптимизации
      int sampleStep = Mathf.Max(texture.width, texture.height) / 10;
      sampleStep = Mathf.Max(sampleStep, 1); // Минимальный шаг = 1

      Color[] pixels = texture.GetPixels();
      if (pixels.Length == 0) return Color.white;

      // Если текстура маленькая, берем все пиксели
      if (texture.width <= 20 || texture.height <= 20)
      {
         return CalculateAverageColor(pixels);
      }

      // Для больших текстур делаем выборку
      float r = 0, g = 0, b = 0;
      int count = 0;

      for (int y = 0; y < texture.height; y += sampleStep)
      {
         for (int x = 0; x < texture.width; x += sampleStep)
         {
            Color pixel = pixels[y * texture.width + x];
            r += pixel.r;
            g += pixel.g;
            b += pixel.b;
            count++;
         }
      }

      return count > 0 ? new Color(r / count, g / count, b / count) : Color.white;
   }

   private Color CalculateAverageColor(Color[] colors)
   {
      float r = 0, g = 0, b = 0;
      int count = colors.Length;

      foreach (Color color in colors)
      {
         r += color.r;
         g += color.g;
         b += color.b;
      }

      return new Color(r / count, g / count, b / count);
   }
}
