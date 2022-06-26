using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class EquipmentSystem : MonoBehaviour
{
    [SerializeField] private List<Item> _equipableItemsInBack = new List<Item>();
    [SerializeField] private List<Item> _collectedItemsFromScene = new List<Item>();

    private PlayerController _player;

    public static event Action<Item> OnItemCollect;

    private void Awake()
    {
        _player = GetComponentInParent<PlayerController>();
    }

    public List<Item> Items => _collectedItemsFromScene;

    public void AddItem(Item newItem)
    {
        _collectedItemsFromScene.Add(newItem);
        OnItemCollect?.Invoke(newItem);
       var tryToEquip = newItem.ItemType == ItemType.Equipable;
        if (tryToEquip)
            FindInBackAndEquip(newItem,2000);
    }

    public void RemoveItem(Item newItem)
    {
        _collectedItemsFromScene.Remove(newItem);
    }
    private async void FindInBackAndEquip(Item newItem, int milisec)
    {
        var sameItemInBack = _equipableItemsInBack.Where(i => i.ID == newItem.ID).FirstOrDefault();
        if (sameItemInBack)
        {
            await Task.Delay(milisec);
            sameItemInBack.Equip(_player);
        }
    }
}
