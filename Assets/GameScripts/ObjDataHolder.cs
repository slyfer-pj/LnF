using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjDataHolder : MonoBehaviour
{
    public ObjectData ObjData { get; set; }

    public void OnClickObject()
    {
        GetObjectData getobjData = GetComponentInParent<GetObjectData>();
        getobjData.inspectObjectScreen.InspectedObjData = ObjData;
        getobjData.inspectObjectScreen.gameObject.SetActive(true);
    }
}
