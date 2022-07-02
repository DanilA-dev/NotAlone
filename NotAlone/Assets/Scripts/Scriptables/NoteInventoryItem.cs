using UnityEngine;

[CreateAssetMenu(menuName ="Data/Item/Note")]
public class NoteInventoryItem : InventoryItem
{
    [TextArea]
    public string noteDescription;
}