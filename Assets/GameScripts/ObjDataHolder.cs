using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjDataHolder : MonoBehaviour
{
    public ObjectData ObjData { get; set; }

    public void OnClickObject()
    {
        GetObjectData getobjData = GetComponentInParent<GetObjectData>();
        getobjData.firstTimeOnly.gameObject.SetActive(false);
        getobjData.inspectObjectScreen.InspectedObjData = ObjData;
        getobjData.inspectObjectScreen.SiblingIndex = transform.GetSiblingIndex();
        getobjData.inspectObjectScreen.gameObject.SetActive(true);
        
    }
}
