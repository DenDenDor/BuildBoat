using UnityEngine;
using System;
using System.Collections.Generic;

public class BuildController : MonoBehaviour
{
    private static BuildController _instance;

    public static BuildController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<BuildController>();

                if (_instance == null)
                {
                    throw new NotImplementedException("BuildController not found!");
                }
            }

            return _instance;
        }
    }

    private readonly Dictionary<BlockView, BlockInfo> _blocksByTypes = new();
    
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        _instance = this;
    }

    public void Add(BlockView blockView, BlockInfo type)
    {
        Debug.Log("BLOCK " + blockView.name + " " + type.Type);
        _blocksByTypes.Add(blockView, type);   
    }

    public BlockInfo GetBlockType(BlockView blockView)
    {
        return _blocksByTypes[blockView];
    }
}