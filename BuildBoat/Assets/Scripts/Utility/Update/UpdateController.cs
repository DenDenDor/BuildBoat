using UnityEngine;
using System;
using System.Collections.Generic;

public class UpdateController : MonoBehaviour
{
    private List<Action> _actions = new();
    
    private static UpdateController _instance;

    public static UpdateController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<UpdateController>();

                if (_instance == null)
                {
                    throw new NotImplementedException("UpdateController not found!");
                }
            }

            return _instance;
        }
    }

    public event Action StoppedTime;
    public event Action ContinueMovingTime;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        _instance = this;
    }

    public void UpdateActions()
    {
        foreach (var action in _actions)
        {
            action.Invoke();
        }
    }

    public void Add(Action action)
    {
        _actions.Add(action);
    }

    public void StopTime()
    {
        StoppedTime?.Invoke();
    }    
    
    public void ContinueTime()
    {
        ContinueMovingTime?.Invoke();
    }
}