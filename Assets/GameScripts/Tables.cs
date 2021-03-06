﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tables : MonoBehaviour
{
    public DialogueData TablesDialogueSet { get; set; } 
    [SerializeField] private DialogueDisplay dialogueScreen;
    [SerializeField] private Image[] chars;

    private Tavern tavern;

    private void Awake()
    {
        tavern = GetComponentInParent<Tavern>();
    }

    public void OnClickTable()
    {
        tavern.TableClicked();
        GetComponent<Selectable>().interactable = false;
        dialogueScreen.CurrentDialogueSet = TablesDialogueSet;
        dialogueScreen.gameObject.SetActive(true);

        ReduceCharAlpha();
    }

    public void ReduceCharAlpha()
    {
        for(int i=0; i<chars.Length; i++)
        {
            chars[i].color = new Color(chars[i].color.r, chars[i].color.g, chars[i].color.b, 0.5f);
        }
    }

    public void SetCharSprites()
    {
        int i = 0;
        for(; i < TablesDialogueSet.participatingCharacters.Length; i++)
        {
            chars[i].sprite = GetCharSprite(TablesDialogueSet.participatingCharacters[i]);
            chars[i].color = new Color(chars[i].color.r, chars[i].color.g, chars[i].color.b, 1f);
            chars[i].SetNativeSize();
            chars[i].enabled = true;
        }

        for (; i < chars.Length; i++)
            chars[i].enabled = false;
    }

    public void ClearCharSprites()
    {
        for (int i = 0; i < chars.Length; i++)
            chars[i].enabled = false;
    }

    private Sprite GetCharSprite(string name)
    {
        foreach(CharacterDataFields data in dialogueScreen.characterData.allCharactersData)
        {
            if (data.charName.Equals(name))
                return data.iconSprite;
        }
        return null;
    }

}
