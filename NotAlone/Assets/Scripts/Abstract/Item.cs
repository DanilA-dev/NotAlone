using System;
using UnityEngine;


public abstract class Item : MonoBehaviour
{
    [SerializeField] private InventoryItem _inventoryItem;

    public InventoryItem InventoryItem => _inventoryItem;
    public int ID => _inventoryItem.id;

    public virtual void Equip(PlayerMovement player) { }
    public virtual void UnEquip() { }

}
