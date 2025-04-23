using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryWindow : AbstractUiCreatorWindow
{
    [SerializeField] private Transform _point;

    private Dictionary<InventoryIcon, InventoryModelGroup> _viewsByModels = new();
    
    public override void Init()
    {
        
    }

    public InventoryIcon Create(InventoryIcon prefab, InventoryModelGroup modelGroup)
    {
        InventoryIcon icon = CreatePrefab(prefab, _point);
        
        _viewsByModels.Add(icon, modelGroup);

        return icon;
    }

    public InventoryIcon GetIcon(InventoryModelGroup modelGroup)
    {
       return _viewsByModels.FirstOrDefault(x=>x.Value == modelGroup).Key;
    }
}