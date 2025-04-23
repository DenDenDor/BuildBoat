using UnityEngine;
using System;
using System.Collections.Generic;

public class InventoryController : MonoBehaviour
{
    private InventoryModelGroup _selectedGroupItem;
    
    private readonly List<InventoryModelGroup> _items = new();

    public List<InventoryModelGroup> Items => _items;

    private static InventoryController _instance;

    public static InventoryController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<InventoryController>();

                if (_instance == null)
                {
                    throw new NotImplementedException("InventoryController not found!");
                }
            }

            return _instance;
        }
    }

    public int CountSelectedBlocks => _selectedGroupItem.Amount;

    public event Action<InventoryModelGroup> Updated;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        _instance = this;
    }

    public void Add(List<InventoryModelGroup> items)
    {
        _items.AddRange(items);
    }

    public void TakeBlock()
    {
        _selectedGroupItem.Amount--;
        
        Updated?.Invoke(_selectedGroupItem);
    }

    public void Select(InventoryModelGroup groupItem)
    {
        _selectedGroupItem = groupItem;
    }

    public void AddBlock()
    {
        _selectedGroupItem.Amount++;
        
        Updated?.Invoke(_selectedGroupItem);
    }
}