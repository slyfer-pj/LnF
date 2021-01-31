using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "ScriptableObject/CharacterData")]
public class CharacterData : ScriptableObject
{
    public CharacterDataFields[] allCharactersData;
}

[System.Serializable]
public class CharacterDataFields
{
    public string charName;
    public Sprite iconSprite;
    public Sprite portraitSprite;
    public string[] clueArray;
    public DialogueData positiveResponse;
    public DialogueData negativeResponse;
}

[System.Serializable]
public class AllCharacterSaveData
{
    public CharacterSaveData[] consolidatedCharSaveData;
}

[System.Serializable]
public class CharacterSaveData
{
    public string charName;
    public bool hasBeenDiscovered;
    public List<int> discoveredCluesIndex;
}
