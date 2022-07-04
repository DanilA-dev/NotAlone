using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Linq;
using System;

[CreateAssetMenu(menuName ="Data/Inventory")]
public class Inventory : ScriptableObject
{
    [SerializeField] private List<InventoryItem> _inventoryItems = new List<InventoryItem>();

    public List<InventoryItem> InventoryItems => _inventoryItems; 

    public void AddItem(InventoryItem item)
    {
        if (_inventoryItems.Contains(item))
            return;

        item.state = InventoryItemState.In_backpack;
        _inventoryItems.Add(item);
    }

    public void DeleteItem(InventoryItem item)
    {
        _inventoryItems.Remove(item);
    }


    public List<InventoryItem> GetEquipedItems()
    {
        List<InventoryItem> returnList = new List<InventoryItem>();
        if (_inventoryItems.Count > 0)
            returnList = _inventoryItems.Where(i => i.state == InventoryItemState.Equiped).ToList();

        return returnList;
    }

    [Button]
    private void Clear()
    {
        _inventoryItems.Clear();
    }    
}
