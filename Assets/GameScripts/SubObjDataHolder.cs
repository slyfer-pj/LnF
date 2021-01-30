using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubObjDataHolder : MonoBehaviour
{
    public SubObjectData SubObjData { get; set; }
    private InspectObject inspectObject;

    private void Awake()
    {
        inspectObject = GetComponentInParent<InspectObject>();
    }

    private void OnEnable()
    {
        GetComponent<Image>().sprite = SubObjData.subObjSprite;
        //GetComponent<Button>().onClick.AddListener(OnClickObject);
    }

    public void OnClickObject()
    {
        inspectObject.descBox.transform.parent.gameObject.SetActive(true);
        inspectObject.descBox.text = SubObjData.subObjDescription;
    }

    //private void OnDisable()
    //{
    //    GetComponent<Button>().onClick.RemoveListener(OnClickObject);
    //}
}
