﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GetObjectData : MonoBehaviour
{
    [SerializeField] private LostObjectData lostObjData;
    [SerializeField] private GameObject objPrefab;
    [SerializeField] private Transform objHolder;
    [SerializeField] private TextMeshProUGUI notification;
    public InspectObject inspectObjectScreen;
    [HideInInspector] public AllObjectSaveData objSaveData;


    private void Start()
    {
        GetSaveData();
        PopulateObjectsOnScreen();
    }

    private void PopulateObjectsOnScreen()
    {
        for(int i=0; i<objSaveData.saveData.Length; i++)
        {
            if(objSaveData.saveData[i].hasBeenDiscovered)
            {
                GameObject obj = Instantiate(objPrefab, objHolder);
                obj.GetComponent<Image>().sprite = lostObjData.objData[i].objectSprite;
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
}
