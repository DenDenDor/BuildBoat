using UnityEngine;
using System;
using System.Collections;

public class FPSController : MonoBehaviour
{
    private static FPSController _instance;

    public static FPSController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<FPSController>();

                if (_instance == null)
                {
                    throw new NotImplementedException("FPSController not found!");
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
        
        DontDestroyOnLoad(gameObject);
    }
    
    private float _count;

    public float Count => _count;

    private IEnumerator Start()
    {
        while (true)
        {
            Debug.LogError("FPS " + _count);
            _count = 1f / Time.unscaledDeltaTime;
            yield return new WaitForSeconds(0.5f);
        }
    }
    
    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 200, 20), $"FPS: {_count:0.}");
    }

}