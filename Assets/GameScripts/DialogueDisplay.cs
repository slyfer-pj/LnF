using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI charName;
    [SerializeField] private TextMeshProUGUI dialogueField;
    [SerializeField] private Image[] portraits;
    [SerializeField] private CharacterData characterData;

    public DialogueData CurrentDialogueSet { get; set; }

    private int currentDialogueIndex;

    private void OnEnable()
    {
        currentDialogueIndex = 0;

        int index = 0;
        foreach(string charName in CurrentDialogueSet.participatingCharacters)
        {
            SetPortrait(charName, index);
            index++;
        }

        ShowDialogue();
    }

    private void ShowDialogue()
    {
        charName.text = CurrentDialogueSet.container[currentDialogueIndex].entityName;
        dialogueField.text = CurrentDialogueSet.container[currentDialogueIndex].dialogueSaid;
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
        }
    }

    private void SetPortrait(string charName, int index)
    {
        foreach(CharacterDataFields data in characterData.allCharactersData)
        {
            if(data.charName.Equals(charName))
            {
                portraits[index].sprite = data.portraitSprite;
                
                return;
            }
        }
        Debug.LogError("no character exists of name = " + charName);
    }
}
