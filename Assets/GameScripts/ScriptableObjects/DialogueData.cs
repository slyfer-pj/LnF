using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueSet", menuName = "ScriptableObject/DialougeData")]
public class DialogueData : ScriptableObject
{
    public string[] participatingCharacters;
    public DialogueContainer[] container;
}

[System.Serializable]
public class DialogueContainer
{
    public string entityName;
    [TextArea] public string dialogueSaid;
}
