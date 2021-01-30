using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tables : MonoBehaviour
{
    public DialogueData TablesDialogueSet { get; set; }
    [SerializeField] private DialogueDisplay dialogueScreen;

    private Tavern tavern;

    private void Awake()
    {
        tavern = GetComponentInParent<Tavern>();
    }

    public void OnClickTable()
    {
        tavern.TableClicked();
        GetComponent<Selectable>().interactable = false;
        dialogueScreen.CurrentDialogueSet = TablesDialogueSet;
        dialogueScreen.gameObject.SetActive(true);
    }
}
