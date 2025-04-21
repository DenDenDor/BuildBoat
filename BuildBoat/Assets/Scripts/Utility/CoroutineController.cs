using UnityEngine;
using System;
using System.Collections;

public class CoroutineController : MonoBehaviour
{
    private static CoroutineController _instance;

    public static CoroutineController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<CoroutineController>();

                if (_instance == null)
                {
                    throw new NotImplementedException("CoroutineController not found!");
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
    }
    
    public void RunCoroutine(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}