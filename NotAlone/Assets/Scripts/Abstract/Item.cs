using UnityEngine;

public enum ItemType
{
   Equipable,
   Collectable
}

public class Item : MonoBehaviour
{
    [field : SerializeField] public ItemType ItemType { get; private set; }
    [field: SerializeField] public bool Equiped { get; set; }
    [field: SerializeField] public int ID { get; private set; }


    public virtual void Equip(PlayerController player) { }

    public virtual void UnEquip() { }

}
