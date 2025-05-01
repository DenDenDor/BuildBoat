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
    
    private float _fps;
    private float _amount;

    public float FPS => _fps;

    public event Action LowFPS;

    private IEnumerator Start()
    {
        while (true)
        {
            Debug.LogError("FPS " + _fps);
            _fps = 1f / Time.unscaledDeltaTime;

            if (_fps < 40 && _amount < 7)
            {
                LowFPS?.Invoke();
            }

            _amount++;
            
            yield return new WaitForSeconds(0.5f);
        }
    }
    
    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 200, 20), $"FPS: {_fps:0.}");
    }

}