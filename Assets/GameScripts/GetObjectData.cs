using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetObjectData : MonoBehaviour
{
    [SerializeField] private LostObjectData lostObjData;
    [SerializeField] private GameObject objPrefab;
    [SerializeField] private Transform objHolder;
    private ObjectSaveData[] objSaveData;


    private void Start()
    {
        GetSaveData();
        PopulateObjectsOnScreen();
    }

    private void PopulateObjectsOnScreen()
    {
        for(int i=0; i<objSaveData.Length; i++)
        {
            if(objSaveData[i].hasBeenDiscovered)
            {
                GameObject obj = Instantiate(objPrefab, objHolder);
                obj.GetComponent<Image>().sprite = lostObjData.objData[i].objectSprite;
            }
        }
    }

    private void GetSaveData()
    {
        if(FileOps.CheckIfFileExists(GameConstants.DATA_OBJECTSDATA_FILEPATH))
        {
            objSaveData = FileOps.Load<AllObjectSaveData>(GameConstants.DATA_OBJECTSDATA_FILEPATH).saveData;
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
                hasBeenDiscovered = lostObjData.objData[i].discoveredByDefault
            };

            consolidatedSaveData.saveData[i] = saveData;
        }

        objSaveData = consolidatedSaveData.saveData;

        FileOps.Save(consolidatedSaveData, GameConstants.DATA_OBJECTSDATA_FILEPATH);
    }
}
