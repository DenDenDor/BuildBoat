using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryRouter : IRouter
{
    private InventoryIcon _prefab;
    private InventoryWindow Window => UiController.Instance.GetWindow<InventoryWindow>();

    private List<InventoryModelGroup> _items;
    public void Init()
    {
        _prefab = FactoryController.Instance.FindPrefab<InventoryIcon>();
        
        BlockType[] allValues = (BlockType[])Enum.GetValues(typeof(BlockType));

        _items = new List<InventoryModelGroup>();
        
        foreach (var blockType in allValues) 
            _items.Add(new InventoryModelGroup(new InventoryItem(blockType), 0));
        
        var items =  SDKMediator.Instance.GenerateSaveData().Items;

        Debug.LogError("ITEMS " + items.Length);

        if (items.Length == 0)
        {
            UpdateBlockAmount(BlockType.Grass, 15);
            UpdateBlockAmount(BlockType.Rock, 25);
            UpdateBlockAmount(BlockType.Sand, 25);
            UpdateBlockAmount(BlockType.Wood, 25);
            UpdateBlockAmount(BlockType.Motor, 1);
            
            List<SavableItem> savableItems = new List<SavableItem>();

            foreach (var item in _items)
            {
                savableItems.Add(new SavableItem(){Amount = item.Amount, BlockType = item.InventoryItem.BlockType});
            }
            
            SDKMediator.Instance.SaveItems(savableItems.ToArray());
        }
        else
        {
            foreach (var item in items)
            {
                UpdateBlockAmount(item.BlockType, item.Amount);
            }
        }

        foreach (var item in _items)
        {
            InventoryIcon icon = Window.Create(_prefab, item);
            icon.UpdateIcon(InventoryController.Instance.GetSprite(item.InventoryItem.BlockType));
            icon.UpdateText(item.Amount);
            
            icon.Clicked += OnClicked;
        }
        
        InventoryController.Instance.Add(_items);

        InventoryController.Instance.Select(_items.FirstOrDefault());
        
        InventoryController.Instance.Updated += OnUpdated;
    }

    private void UpdateBlockAmount(BlockType blockType, int amount)
    {
        _items.FirstOrDefault(x => x.InventoryItem.BlockType == blockType).Amount = amount;
    }

    private void OnClicked(InventoryIcon obj)
    {
        InventoryController.Instance.Select(Window.GetModelGroup(obj));
    }

    private void OnUpdated(InventoryModelGroup obj)
    {
        Debug.LogError("A D D   I N VE N T O R Y " + obj.InventoryItem + " : " + obj.Amount);

        _items.FirstOrDefault(x => x.InventoryItem.BlockType == obj.InventoryItem.BlockType).Amount = obj.Amount;

        List<SavableItem> savableItems = new List<SavableItem>();

        foreach (var item in _items)
        {
            savableItems.Add(new SavableItem(){Amount = item.Amount, BlockType = item.InventoryItem.BlockType});
        }
        
        SDKMediator.Instance.SaveItems(savableItems.ToArray());

        Window.GetIcon(obj).UpdateText(obj.Amount);
    }

    public void Exit()
    {
        
    }
}