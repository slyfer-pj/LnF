using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notebook : MonoBehaviour
{
    [SerializeField] private CharacterData characterData;
    [SerializeField] private DialogueDisplay dialogueDisplay;
    [SerializeField] private GameObject characterNotePrefab;
    [SerializeField] private Transform characterNotesParent;
    [SerializeField] private GameObject turnPageButton;

    private List<string> charactersWithNotes = new List<string>();
    private List<GameObject> notesList = new List<GameObject>();

    private int currentPageIndex = 0;

    private void OnEnable()
    {
        CheckIfNewNoteNeedsToBeAdded();

        if (notesList.Count > 0)
        {
            notesList[0].SetActive(true);
            currentPageIndex = 0;
        }
        if (notesList.Count > 1)
            turnPageButton.SetActive(true);
    }

    private void CheckIfNewNoteNeedsToBeAdded()
    {
        foreach(CharacterSaveData saveData in dialogueDisplay.CharSavedData.consolidatedCharSaveData)
        {
            if(saveData.hasBeenDiscovered)
            {
                if(!CheckIfNoteAlreadyExists(saveData.charName))
                {
                    AddNewNote(GetCharData(saveData.charName), saveData);
                }
            }
        }
    }

    private CharacterDataFields GetCharData(string name)
    {
        foreach(CharacterDataFields data in characterData.allCharactersData)
        {
            if (data.charName.Equals(name))
                return data;
        }
        Debug.LogError("no data found");
        return null;
    }

    private bool CheckIfNoteAlreadyExists(string incomingname)
    {
        foreach(string name in charactersWithNotes)
        {
            if (name.Equals(incomingname))
                return true;
        }
        return false;
    }

    private void AddNewNote(CharacterDataFields data, CharacterSaveData saveData)
    {
        charactersWithNotes.Add(data.charName);
        GameObject obj = Instantiate(characterNotePrefab, characterNotesParent);

        string cluestring = string.Empty;
        for(int i=0; i<saveData.discoveredCluesIndex.Count; i++)
        {
            cluestring += data.clueArray[saveData.discoveredCluesIndex[i]] + "\n";
        }

        obj.GetComponent<Note>().PopulateData(data.charName, data.portraitSprite, cluestring);
        notesList.Add(obj);

    }

    public void OnClickNextPage()
    {
        currentPageIndex++;
        if(currentPageIndex >= notesList.Count)
        {
            notesList[notesList.Count -1].SetActive(false);
            currentPageIndex = 0;
        }

        notesList[currentPageIndex].SetActive(true);
        if (currentPageIndex - 1 > -1)
            notesList[currentPageIndex - 1].SetActive(false);
    }

    private void OnDisable()
    {
        notesList[currentPageIndex].SetActive(false);
    }
}
