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
    [SerializeField] private DialogueData endGameStarting;
    [SerializeField] private DialogueData superEndGame;
    [SerializeField] public GameObject confirmDialogueBox;

    public ObjectData InspectedObjData { get; set; }
    public int SiblingIndex { get; set; }
    public string CurrentSelectedCharacter { get; set; }

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
                obj.GetComponentInChildren<Image>().sprite = data.iconSprite;
                obj.GetComponentInChildren<Image>().SetNativeSize();
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
        DialogueData response = null;
        foreach (ObjectData data in objData.objData)
        {
            if (data.objectName.Equals(InspectedObjData.objectName))
            {
                if (data.correctOwnerName.Equals(charName))
                {
                    response = GetBagReturnDialogueSet(charName, true);
                    getObjectData.UpdateReturnStatus(data.objectName, true);
                }
                else
                {
                    response = GetBagReturnDialogueSet(charName, false);
                }
            }
        }
        

        dialogueDisplay.CurrentDialogueSet = response;
        dialogueDisplay.gameObject.SetActive(true);
    }

    public IEnumerator PlayEndGameDialogues()
    {
        DialogueData set = null;
        EndGameDialogueInteractionDone = false;

        dialogueDisplay.CurrentDialogueSet = endGameStarting;
        dialogueDisplay.gameObject.SetActive(true);

        while (!EndGameDialogueInteractionDone)
            yield return null;

        yield return new WaitForSeconds(0.5f);

        foreach (ObjectSaveData data in getObjectData.objSaveData.saveData)
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

        EndGameDialogueInteractionDone = false;
        dialogueDisplay.CurrentDialogueSet = superEndGame;
        dialogueDisplay.gameObject.SetActive(true);

        while (!EndGameDialogueInteractionDone)
            yield return null;

        thanksForPlaying.SetActive(true);

    }

    private DialogueData GetBagReturnDialogueSet(string name, bool positive)
    {
        foreach(CharacterDataFields data in dialogueDisplay.characterData.allCharactersData)
        {
            if(name.Equals(data.charName))
            {
                if (positive)
                    return data.positiveBagReturnResponse;
                else
                    return data.negativeBagReturnResponse;
            }
        }
        return null;
    }


    private DialogueData GetDialogueSet(string name, bool positive)
    {
        foreach(ObjectData dat in objData.objData)
        {
            if(name.Equals(dat.objectName))
            {
                if (positive)
                    return dat.endGameSuccessDialogueSet;
                else
                    return dat.endGameFailDialogueSet;
            }
        }
        return null;
    }

    public void RemoveBag()
    {
        gameObject.SetActive(false);
        //getObjectData.gameObject.SetActive(false);
        getObjectData.objHolder.GetChild(SiblingIndex).GetComponent<Selectable>().interactable = false;
    }

    public void ClearSubObjects()
    {
        foreach(Transform child in subObjHolder)
        {
            Destroy(child.gameObject);
        }
    }

    public void GiveBagToCharacter()
    {
        if (CurrentSelectedCharacter != null)
        {
            CharacterResponse(CurrentSelectedCharacter);
            RemoveBag();
        }
    }

    private void OnDisable()
    {
        ClearSubObjects();
    }

}
