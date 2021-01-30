using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tables : MonoBehaviour
{
    public DialogueData TablesDialogueSet { get; set; }
    [SerializeField] private DialogueDisplay dialogueScreen;

    public void OnClickTable()
    {
        GetComponent<Selectable>().interactable = false;
        dialogueScreen.CurrentDialogueSet = TablesDialogueSet;
        dialogueScreen.gameObject.SetActive(true);
    }
}
