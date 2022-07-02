using UnityEngine;

public enum InventoryItemState
{
    None,
    Not_in_backpack,
    In_backpack,
    Equiped
}

public enum InventoryItemType
{
    None,
    Collectable,
    Note,
    Equipable
}


[CreateAssetMenu(menuName ="Data/Item/Item")]
public class InventoryItem : ScriptableObject
{
    public int id;
    public string itemName;
    public InventoryItemType type;
    public InventoryItemState state;
}
