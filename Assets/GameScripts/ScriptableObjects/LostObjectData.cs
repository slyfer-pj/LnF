using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LostObjectData", menuName = "ScriptableObject/LostObjectData")]
public class LostObjectData : ScriptableObject
{
    public ObjectData[] objData;
}

[System.Serializable]
public class ObjectData
{
    public string objectName;
    public Sprite objectSprite;
    [TextArea] public string objectDescription;
    public bool discoveredByDefault;
}

[System.Serializable]
public class AllObjectSaveData
{
    public ObjectSaveData[] saveData;
}

[System.Serializable]
public class ObjectSaveData
{
    public string objectName;
    public bool hasBeenDiscovered;
}