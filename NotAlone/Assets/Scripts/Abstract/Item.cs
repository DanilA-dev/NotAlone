using UnityEngine;

public enum ItemType
{
   Equipable,
   Collectable
}

public class Item : MonoBehaviour
{
    [field : SerializeField] public ItemType ItemType { get; private set; }
    [field: SerializeField] public int ID { get; private set; }


    public virtual void Equip() { }

    public virtual void UnEquip() { }

}
