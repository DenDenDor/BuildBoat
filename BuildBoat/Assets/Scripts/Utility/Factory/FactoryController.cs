using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

public class FactoryController : MonoBehaviour
{
    public event Action<GameObject> CreatedUi;
    public event Action<GameObject> CreatedGameObject;
    
    private List<GameObject> _prefabs = new();

    private static FactoryController _instance;

    public static FactoryController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<FactoryController>();

                if (_instance == null)
                {
                    throw new NotImplementedException("FactoryController not found!");
                }
            }

            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;


        GameObject[] allPrefabs = Resources.LoadAll<GameObject>("");

        foreach (var prefab in allPrefabs)
        {
            if (prefab.gameObject.scene.name == null)
            {
                _prefabs.Add(prefab);
            }
        }
    }
    
    public T FindPrefab<T>() where T : MonoBehaviour
    {
        return _prefabs.FirstOrDefault(x => x.GetComponent<T>()).GetComponent<T>();
    }

    public IEnumerable<T> FindAllPrefab<T>() where T : MonoBehaviour
    {
        List<T> prefabs = new();

        foreach (var prefab in _prefabs.Where(x => x.GetComponent<T>()))
        {
            prefabs.Add(prefab.GetComponent<T>());
        }
        
        return prefabs;
    }

    public void CreateGameObject(GameObject prefab)
    {
        CreatedUi?.Invoke(prefab);
    }

    public void CreateUi(GameObject prefab)
    {
        CreatedUi?.Invoke(prefab);
    }
}