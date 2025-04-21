using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryRouter : IRouter
{
    private InventoryIcon _prefab;
    private InventoryWindow Window => UiController.Instance.GetWindow<InventoryWindow>();

    public void Init()
    {
        _prefab = FactoryController.Instance.FindPrefab<InventoryIcon>();

        
        List<InventoryModelGroup> items = new List<InventoryModelGroup>()
        {
            new InventoryModelGroup(new InventoryItem() {Name = "Juno"}, 5)
        };

        foreach (var item in items)
        {
            InventoryIcon icon =Window.Create(_prefab, item);
            
            icon.UpdateText(item.Amount);
        }
        
        InventoryController.Instance.Add(items);

        InventoryController.Instance.Select(items.FirstOrDefault());
        
        InventoryController.Instance.Updated += OnUpdated;
    }

    private void OnUpdated(InventoryModelGroup obj)
    {
        Window.GetIcon(obj).UpdateText(obj.Amount);
    }

    public void Exit()
    {
        
    }
}