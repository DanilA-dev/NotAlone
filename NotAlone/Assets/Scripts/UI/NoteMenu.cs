using UnityEngine;
using TMPro;

public class NoteMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text _noteText;

    public void UpdateText(string text)
    {
        _noteText.SetText(text);
    }

}
