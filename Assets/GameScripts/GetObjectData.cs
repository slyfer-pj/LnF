using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GetObjectData : MonoBehaviour
{
    [SerializeField] private LostObjectData lostObjData;
    [SerializeField] private GameObject objPrefab;
    [SerializeField] public Transform objHolder;
    [SerializeField] private TextMeshProUGUI notification;
    public InspectObject inspectObjectScreen;
    [HideInInspector] public AllObjectSaveData objSaveData;
    [SerializeField] public TextMeshProUGUI firstTimeOnly;


    private void Awake()
    {
        GetSaveData();
        
        if(!PlayerPrefs.HasKey("FirstTimeOnly"))
        {
            firstTimeOnly.gameObject.SetActive(true);
            PlayerPrefs.SetInt("FirstTimeOnly", 1);
        }
    }

    private void OnEnable()
    {
        PopulateObjectsOnScreen();
    }

    private void PopulateObjectsOnScreen()
    {
        for(int i=objHolder.childCount; i<objSaveData.saveData.Length; i++)
        {
            if(objSaveData.saveData[i].hasBeenDiscovered)
            {
                GameObject obj = Instantiate(objPrefab, objHolder);
                obj.GetComponent<Image>().sprite = lostObjData.objData[i].objectSprite;
                obj.GetComponent<Image>().SetNativeSize();
                obj.GetComponent<ObjDataHolder>().ObjData = lostObjData.objData[i];
            }
        }
    }

    private void GetSaveData()
    {
        if(FileOps.CheckIfFileExists(GameConstants.DATA_OBJECTSDATA_FILEPATH))
        {
            objSaveData = FileOps.Load<AllObjectSaveData>(GameConstants.DATA_OBJECTSDATA_FILEPATH);
        }
        else
        {
            SaveFirstTimeData();
        }
    }

    private void SaveFirstTimeData()
    {
        AllObjectSaveData consolidatedSaveData = new AllObjectSaveData();
        consolidatedSaveData.saveData = new ObjectSaveData[lostObjData.objData.Length];

        for (int i = 0; i < lostObjData.objData.Length; i++)
        {
            ObjectSaveData saveData = new ObjectSaveData()
            {
                objectName = lostObjData.objData[i].objectName,
                hasBeenDiscovered = lostObjData.objData[i].dayUnlocked == 1 ? true : false
            };

            consolidatedSaveData.saveData[i] = saveData;
        }

        objSaveData = consolidatedSaveData;

        FileOps.Save(objSaveData, GameConstants.DATA_OBJECTSDATA_FILEPATH);
    }

    public void CheckIfObjectIsToBeUnlocked(int dayNumber)
    {
        for(int i=0; i<lostObjData.objData.Length; i++)
        {
            if(lostObjData.objData[i].dayUnlocked == dayNumber)
            {
                objSaveData.saveData[i].hasBeenDiscovered = true;
                notification.text = "New bag available!";
                notification.gameObject.SetActive(true);
            }
        }

        FileOps.Save(objSaveData, GameConstants.DATA_OBJECTSDATA_FILEPATH);
    }

    public void UpdateReturnStatus(string objName, bool status)
    {
        foreach(ObjectSaveData data in objSaveData.saveData)
        {
            if (objName.Equals(data.objectName))
            {
                data.returnedSuccessfully = status;
                break;
            }
        }

        FileOps.Save(objSaveData, GameConstants.DATA_OBJECTSDATA_FILEPATH);
    }

    public IEnumerator ForcePlayerToReturnAllBags()
    {
        bool allBagsReturned = false;
        while(!allBagsReturned)
        {
            foreach (Transform child in objHolder)
            {
                allBagsReturned = true;
                if (child.GetComponent<Selectable>().interactable)
                {
                    allBagsReturned = false;
                    break;
                }
            }

            yield return null;

        }

        yield return new WaitForEndOfFrame();
        StartCoroutine(inspectObjectScreen.PlayEndGameDialogues());
    }
}
