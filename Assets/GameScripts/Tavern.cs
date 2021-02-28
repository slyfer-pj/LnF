using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Tavern : MonoBehaviour
{
    [SerializeField] private InspectObject inspectObject;
    [SerializeField] private GetObjectData objData;
    [SerializeField] private TableConfig[] tableConfigsForEachDay;
    [SerializeField] private Tables[] tables;
    [SerializeField] private TextMeshProUGUI dayInfo;
    [SerializeField] private GameObject endDayButton;
    [SerializeField] private Button lostAndFound;
    [SerializeField] private Button tavern;
    private int currentDay;

    private int convosHad = 0;
    private const int maxNumberOfDays = 6;

    private void Start()
    {
        if (PlayerPrefs.HasKey(GameConstants.PREFS_CURRENTDAY))
            currentDay = PlayerPrefs.GetInt(GameConstants.PREFS_CURRENTDAY);
        else
        {
            currentDay = 1;
            PlayerPrefs.SetInt(GameConstants.PREFS_CURRENTDAY, 1);
        }

        dayInfo.text = "Day " + currentDay + ": " + GetTimeOfDay();

        SetTables();
    }

    private void SetTables()
    {
        for(int i=0; i<tables.Length; i++)
        {
            if (tableConfigsForEachDay[currentDay - 1].dialogueSetsForTables[i] != null)
            {
                tables[i].GetComponent<Selectable>().interactable = true;
                tables[i].TablesDialogueSet = tableConfigsForEachDay[currentDay - 1].dialogueSetsForTables[i];
                tables[i].SetCharSprites();
            }
            else
            {
                tables[i].ClearCharSprites();
                tables[i].GetComponent<Selectable>().interactable = false;
            }
        }
    }

    public void TableClicked()
    {
        convosHad++;
        dayInfo.text = "Day " + currentDay + ": " + GetTimeOfDay();
        if (convosHad == 2)
        {
            foreach (Tables table in tables)
            {
                table.GetComponent<Selectable>().interactable = false;
                table.ReduceCharAlpha();
            }

            if (currentDay == maxNumberOfDays)
                endDayButton.GetComponentInChildren<TextMeshProUGUI>().text = "End day (last day!)";
            endDayButton.SetActive(true);
        }
    }

    public void OnClickEndDay()
    {
        endDayButton.SetActive(false);
        convosHad = 0;
        currentDay++;

        if(currentDay > maxNumberOfDays)
        {
            GoToEndGame();
        }

        dayInfo.text = "Day " + currentDay + ": " + GetTimeOfDay();
        objData.CheckIfObjectIsToBeUnlocked(currentDay);
        SetTables();
    }
    
    private string GetTimeOfDay()
    {
        if (convosHad == 0)
            return "Morning";
        else if (convosHad == 1)
            return "Evening";
        else
            return "Closing Time";
    }

    private void GoToEndGame()
    {
        lostAndFound.onClick.Invoke();
        lostAndFound.interactable = false;
        tavern.interactable = false;
        objData.StartCoroutine(objData.ForcePlayerToReturnAllBags());
    }
}

[System.Serializable]
public struct TableConfig
{
    public int dayNumber;
    public DialogueData[] dialogueSetsForTables;
}
