using UnityEngine;

public class InventoryModelGroup
{
    public InventoryItem InventoryItem;
    public int Amount;

    public InventoryModelGroup(InventoryItem inventoryItem, int amount)
    {
        InventoryItem = inventoryItem;
        Amount = amount;
    }
}
