using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerAssigner : MonoBehaviour
{   
    [SerializeField] private string _layerName;
    [SerializeField] private bool _applyToChildren = true; // Галочка "Применить к детям"

    private  void Awake()
    {
        // Преобразуем имя слоя в его числовой идентификатор
        int layer = LayerMask.NameToLayer(_layerName);
        if (layer != -1) // Проверяем, существует ли слой
        {
            if (_applyToChildren)
            {
                // Если галочка "Применить к детям" активна, применяем слой рекурсивно
                SetLayerRecursively(gameObject, layer);
            }
            else
            {
                // Если галочка не активна, применяем слой только к текущему объекту
                gameObject.layer = layer;
            }
        }
        else
        {
            Debug.LogError("Layer not found: " + _layerName);
        }
    }

    // Рекурсивная функция для применения слоя ко всем дочерним объектам
    void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer; // Применяем слой к текущему объекту
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, layer); // Рекурсивно применяем к детям
        }
    }


}
