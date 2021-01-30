using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tavern : MonoBehaviour
{
    [SerializeField] private TableConfig[] tableConfigsForEachDay;
    [SerializeField] private Tables[] tables;
    [SerializeField] private TextMeshProUGUI dayInfo;
    private int currentDay;

    private void Start()
    {
        if (PlayerPrefs.HasKey(GameConstants.PREFS_CURRENTDAY))
            currentDay = PlayerPrefs.GetInt(GameConstants.PREFS_CURRENTDAY);
        else
        {
            currentDay = 1;
            PlayerPrefs.SetInt(GameConstants.PREFS_CURRENTDAY, 1);
        }

        dayInfo.text = "Day " + currentDay;

        SetTables();
    }

    private void SetTables()
    {
        for(int i=0; i<tables.Length; i++)
        {
            tables[i].TablesDialogueSet = tableConfigsForEachDay[currentDay - 1].dialogueSetsForTables[i];
        }
    }
}

[System.Serializable]
public struct TableConfig
{
    public int dayNumber;
    public DialogueData[] dialogueSetsForTables;
}
