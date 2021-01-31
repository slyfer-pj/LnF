using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharSelectDataHolder : MonoBehaviour
{
    public string charName;
    private InspectObject inspectObject;

    private void Awake()
    {
        inspectObject = GetComponentInParent<InspectObject>();
    }

    public void OnClickCharacter()
    {
        inspectObject.CharacterResponse(charName);
    }
}
