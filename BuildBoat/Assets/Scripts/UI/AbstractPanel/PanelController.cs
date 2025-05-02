using UnityEngine;
using System;

public class PanelController : MonoBehaviour
{
    private static PanelController _instance;

    public static PanelController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<PanelController>();

                if (_instance == null)
                {
                    throw new NotImplementedException("PanelController not found!");
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
}