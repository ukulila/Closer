using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class CharacterArray
{
    public char[] charactersArray;
}

[System.Serializable]
public class ListOfCharacterArray
{
    public List<CharacterArray> lines;
}







[System.Serializable]
public class ListOfListOfArray
{
    public List<ListOfCharacterArray> dialogueLines;
}

