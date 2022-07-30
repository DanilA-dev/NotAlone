using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class EquipmentSystem : MonoBehaviour
{
    [SerializeField] private Inventory _inventory;

    private List<Item> _equipableItems = new List<Item>();
    private PlayerController _player;

    public static event Action<InventoryItem> OnItemCollect;

    private void Awake()
    {
        _player = GetComponentInParent<PlayerController>();
        GetEquipableItems();
        LoadInventory();
    }

    private void GetEquipableItems()
    {
        foreach (Transform child in transform)
        {
            if (child.TryGetComponent(out Item item) && item.InventoryItem.type == InventoryItemType.Equipable)
                _equipableItems.Add(item);
        }
    }

    private void LoadInventory()
    {
        //load from save

        if (_inventory.GetEquipedItems() == null)
            return;

        for (int i = 0; i < _inventory.GetEquipedItems().Count; i++)
            FindInBackAndEquip(_inventory.GetEquipedItems()[i], 500);
    }

    public void AddItemToInventory(InventoryItem newItem)
    {
        OnItemCollect?.Invoke(newItem);
        _inventory.AddItem(newItem);
        var tryToEquip = newItem.type == InventoryItemType.Equipable;
        if (tryToEquip)
            FindInBackAndEquip(newItem,2000);
    }

   
    private async void FindInBackAndEquip(InventoryItem newItem, int milisec)
    {
        var sameItemInBack = _equipableItems.Where(i => i.ID == newItem.id).FirstOrDefault();
        if (sameItemInBack)
        {
            await Task.Delay(milisec);
            newItem.state = InventoryItemState.Equiped;
            sameItemInBack.Equip(_player);
        }
    }
}
