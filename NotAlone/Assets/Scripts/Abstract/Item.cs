using UnityEngine;

public enum ItemState
{
    UnEquiped,
    Equiped
}

public class Item : MonoBehaviour
{

    [field : SerializeField] public ItemState ItemState { get; private set; }

    public virtual void Equip() => ItemState = ItemState.Equiped;

    public virtual void UnEquip() => ItemState = ItemState.UnEquiped;

}
