using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterDatabase", menuName = "Scriptable Objects/CharacterDatabase", order = 2)]
public class CharacterDatabase : SingletonScriptableObject<CharacterDatabase>
{
    [SerializeField] List<CharacterData> ListOfCharacters;
    public CharacterData GetCharacterData(int _idCharacter)
    {
        return ListOfCharacters[_idCharacter];
    }
    public int GetTotalCharacter()
    {
        return ListOfCharacters.Count;
    }
}

