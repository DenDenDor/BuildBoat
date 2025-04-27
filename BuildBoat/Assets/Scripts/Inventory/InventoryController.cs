using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private SerializedDictionary<BlockType, DataBlock> _spritesByTypes;
    
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
    public BlockType Selected => _selectedGroupItem.InventoryItem.BlockType;

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

    public Sprite GetSprite(BlockType type)
    {
        return _spritesByTypes[type].Sprite;
    }   
     
    public Color GetDestroyColor(BlockType type)
    {
        return _spritesByTypes[type].DestroyColor;
    }   
    
    public Material GetMaterial(BlockType type)
    {
        return _spritesByTypes[type].Material;
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

    public void AddBlock(BlockType type)
    {
        InventoryModelGroup modelGroup = _items.FirstOrDefault(x => x.InventoryItem.BlockType == type);
        
        modelGroup.Amount++;
        
        Updated?.Invoke(modelGroup);
    }
}

[Serializable]
public class DataBlock
{
    public Sprite Sprite;
    public Material Material;
    public Color DestroyColor;
}