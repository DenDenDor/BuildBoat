using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public abstract class AbstractFactoryWindow : AbstractWindowUi
{
    protected T CreatePrefab<T>(T prefab, Transform point, bool isUi = false) where T : MonoBehaviour
    {
        T created =  Instantiate(prefab, point);

        return created;
    }     
    
    protected T CreatePrefabByRotation<T>(T prefab, Transform point, bool isUi = false) where T : MonoBehaviour
    {
        T created =  Instantiate(prefab, point.position, point.rotation);

        return created;
    }  
    
    protected T CreatePrefab<T>(T prefab, Vector3 position, bool isUi = false) where T : MonoBehaviour
    {
        T created =  Instantiate(prefab, position, Quaternion.identity);

        CreatePrefab(created.gameObject);
        
        return created;
    }

    private void CreatePrefab(GameObject prefab, bool isUi = false)
    {
        if (isUi)
        {
            FactoryController.Instance.CreateUi(prefab);
        }
        else
        {
            FactoryController.Instance.CreateGameObject(prefab);
        }
    }
    
    public override void Init()
    {

    }


}