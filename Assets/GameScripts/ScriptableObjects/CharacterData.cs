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
    public Sprite portraitSprite;
}
