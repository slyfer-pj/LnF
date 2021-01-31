using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InspectObject : MonoBehaviour
{
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

    public ObjectData InspectedObjData { get; set; }

    private List<string> characterSelectArray = new List<string>();

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
                    response = data.positiveResponse;
                else
                    response = data.negativeResponse;
            }
        }

        dialogueDisplay.CurrentDialogueSet = response;
        dialogueDisplay.gameObject.SetActive(true);
    }

}
