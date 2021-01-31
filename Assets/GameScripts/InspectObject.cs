using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InspectObject : MonoBehaviour
{
    [SerializeField] private GetObjectData getObjectData;
    [SerializeField] private DialogueDisplay dialogueDisplay;
    [SerializeField] private Image objImage;
    public TextMeshProUGUI descBox;
    [SerializeField] private GameObject insideView;
    [SerializeField] private GameObject subObjectPrefab;
    [SerializeField] private Transform subObjHolder;
    [SerializeField] private GameObject characterSelectPrefab;
    [SerializeField] private Transform characterSelectHolder;
    [SerializeField] private GameObject returnScreen;
    [SerializeField] private LostObjectData objData;
    [SerializeField] private GameObject thanksForPlaying;

    public ObjectData InspectedObjData { get; set; }

    private List<string> characterSelectArray = new List<string>();

    public bool EndGameDialogueInteractionDone { get; set; }

    private void OnEnable()
    {
        objImage.sprite = InspectedObjData.objectSprite;
    }

    public void OnClickLookInside()
    {
        PopulateSubObjects();
        insideView.SetActive(true);
    }

    public void OnClickReturnBag()
    {
        PopulateCharSelect();
        returnScreen.SetActive(true);
    }

    private void PopulateCharSelect()
    {
        for(int i=0; i < dialogueDisplay.CharSavedData.consolidatedCharSaveData.Length; i++)
        {
            CharacterSaveData data = dialogueDisplay.CharSavedData.consolidatedCharSaveData[i];
            if (data.hasBeenDiscovered && !characterSelectArray.Contains(data.charName))
            {
                characterSelectArray.Add(data.charName);
                AddItem(data.charName);
            }
        }
    }

    private void AddItem(string name)
    {
        GameObject obj = Instantiate(characterSelectPrefab, characterSelectHolder);
        foreach(CharacterDataFields data in dialogueDisplay.characterData.allCharactersData)
        {
            if(name == data.charName)
            {
                obj.GetComponentInChildren<Image>().sprite = data.portraitSprite;
                obj.GetComponentInChildren<TextMeshProUGUI>().text = data.charName;
                obj.GetComponentInChildren<CharSelectDataHolder>().charName = data.charName;
            }
        }
    }

    private void PopulateSubObjects()
    {
        foreach (SubObjectData subData in InspectedObjData.subObjData)
        {
            GameObject obj = Instantiate(subObjectPrefab, subObjHolder);
            obj.GetComponent<SubObjDataHolder>().SubObjData = subData;
        }
    }

    public void CharacterResponse(string charName)
    {
        string trueOwner = string.Empty;
        foreach (ObjectData data in objData.objData)
        {
            if (data.objectName.Equals(InspectedObjData.objectName))
            {
                trueOwner = data.correctOwnerName;
                break;
            }
        }

        DialogueData response = null;

        foreach(CharacterDataFields data in dialogueDisplay.characterData.allCharactersData)
        {
            if(data.charName.Equals(charName))
            {
                if (trueOwner.Equals(data.charName))
                {
                    response = data.positiveResponse;
                    getObjectData.UpdateReturnStatus(InspectedObjData.objectName, true);
                }
                else
                {
                    response = data.negativeResponse;
                }
            }
        }

        dialogueDisplay.CurrentDialogueSet = response;
        dialogueDisplay.gameObject.SetActive(true);
    }

    public IEnumerator PlayEndGameDialogues()
    {
        DialogueData set = null;
        foreach(ObjectSaveData data in getObjectData.objSaveData.saveData)
        {
            EndGameDialogueInteractionDone = false;

            if (data.returnedSuccessfully)
                set = GetDialogueSet(data.objectName, true);
            else
                set = GetDialogueSet(data.objectName, false);

            dialogueDisplay.CurrentDialogueSet = set;
            dialogueDisplay.gameObject.SetActive(true);

            while (!EndGameDialogueInteractionDone)
                yield return null;

            yield return new WaitForSeconds(0.5f);
        }

        thanksForPlaying.SetActive(true);
    }

    public DialogueData GetDialogueSet(string name, bool positive)
    {
        foreach(ObjectData dat in objData.objData)
        {
            if(name.Equals(dat.objectName))
            {
                if (positive)
                    return dat.successDialogueSet;
                else
                    return dat.failDialogueSet;
            }
        }
        return null;
    }

}
