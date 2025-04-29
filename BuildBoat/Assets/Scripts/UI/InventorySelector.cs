using System;
using System.Collections.Generic;
using UnityEngine;

public class InventorySelector : MonoBehaviour
{
    public static InventorySelector Instance { private set; get; }
    
    [SerializeField] private List<InventoryButton>  _inventoryButtons;
    [SerializeField] private InventoryButton _selectedButton;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else 
            Destroy(gameObject);
    }

    public void AddButton(InventoryButton newButton)
    {
        _inventoryButtons.Add(newButton);
    }

    public void SelectButton(InventoryButton button)
    {
        _selectedButton?.Deselect();
        _selectedButton = button;
    }
}
