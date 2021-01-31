using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Note : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI charname;
    [SerializeField] private Image portrait;
    [SerializeField] private TextMeshProUGUI clues;

    public void PopulateData(string name, Sprite img, string clue)
    {
        charname.text = name;
        portrait.sprite = img;
        portrait.SetNativeSize();
        clues.text = clue;
    }

    public void RefreshClues(string clue)
    {
        clues.text = clue;
    }
}
