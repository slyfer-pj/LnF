using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueDisplay : MonoBehaviour
{
    [SerializeField] private InspectObject inspectObject;
    [SerializeField] private TextMeshProUGUI charName;
    [SerializeField] private TextMeshProUGUI dialogueField;
    [SerializeField] private Image[] portraits;
    public CharacterData characterData;

    public DialogueData CurrentDialogueSet { get; set; }

    private int currentDialogueIndex;
    public AllCharacterSaveData CharSavedData { get; set; }

    private void Awake()
    {
        if (!FileOps.CheckIfFileExists(GameConstants.DATA_CHARACTERDATA_FILEPATH))
            SaveFirstTimeData();
        else
            CharSavedData = FileOps.Load<AllCharacterSaveData>(GameConstants.DATA_CHARACTERDATA_FILEPATH);
    }

    private void OnEnable()
    {
        currentDialogueIndex = 0;

        int index = 0;
        foreach(string charName in CurrentDialogueSet.participatingCharacters)
        {
            SetPortrait(charName, index);
            index++;
        }

        for (; index < portraits.Length; index++)
            portraits[index].enabled = false;


        ShowDialogue();
        CheckIfNewCharacterIsDiscovered();
    }

    private void ShowDialogue()
    {
        charName.text = CurrentDialogueSet.container[currentDialogueIndex].entityName;
        dialogueField.text = CurrentDialogueSet.container[currentDialogueIndex].dialogueSaid;

        CheckIfClueIsDiscovered(CurrentDialogueSet.container[currentDialogueIndex].clueUnlockIfAny);
    }

    public void OnClickNext()
    {
        currentDialogueIndex++;
        if (currentDialogueIndex < CurrentDialogueSet.container.Length)
            ShowDialogue();
        else
        {
            Debug.Log("end of dialogue");
            gameObject.SetActive(false);
            inspectObject.EndGameDialogueInteractionDone = true;
        }
    }

    private void SetPortrait(string charName, int index)
    {
        foreach(CharacterDataFields data in characterData.allCharactersData)
        {
            if(data.charName.Equals(charName))
            {
                portraits[index].enabled = true;
                portraits[index].sprite = data.portraitSprite;
                portraits[index].SetNativeSize();
                return;
            }
        }
        Debug.LogError("no character exists of name = " + charName);
    }

    private void CheckIfNewCharacterIsDiscovered()
    {
        foreach(string charName in CurrentDialogueSet.participatingCharacters)
        {
            foreach(CharacterSaveData data in CharSavedData.consolidatedCharSaveData)
            {
                if(data.charName.Equals(charName))
                {
                    data.hasBeenDiscovered = true;
                    break;
                }
            }
        }

        FileOps.Save(CharSavedData, GameConstants.DATA_CHARACTERDATA_FILEPATH);
    }

    private void CheckIfClueIsDiscovered(ClueUnlock unlock)
    {
        if(!unlock.charName.Equals(string.Empty))
        {
            foreach (CharacterSaveData data in CharSavedData.consolidatedCharSaveData)
            {
                if (data.charName.Equals(unlock.charName))
                {
                    data.discoveredCluesIndex.Add(unlock.clueIndex);
                    break;
                }
            }

            FileOps.Save(CharSavedData, GameConstants.DATA_CHARACTERDATA_FILEPATH);
        }
    }

    private void SaveFirstTimeData()
    {
        AllCharacterSaveData saveData = new AllCharacterSaveData();
        saveData.consolidatedCharSaveData = new CharacterSaveData[characterData.allCharactersData.Length];

        for(int i=0; i < characterData.allCharactersData.Length; i++)
        {
            CharacterSaveData charData = new CharacterSaveData();
            charData.discoveredCluesIndex = new List<int>();

            charData.charName = characterData.allCharactersData[i].charName;
            charData.hasBeenDiscovered = false;

            saveData.consolidatedCharSaveData[i] = charData;
        }

        CharSavedData = saveData;

        FileOps.Save(saveData, GameConstants.DATA_CHARACTERDATA_FILEPATH);
    }
}
