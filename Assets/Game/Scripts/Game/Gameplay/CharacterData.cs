using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Scriptable Objects/CharacterData", order = 1)]
public class CharacterData : ScriptableObject
{
    [SerializeField] Sprite spriteCharacter;
    [SerializeField] Sprite spriteCharacterHead;
    [SerializeField] Sprite spriteCharacterBody;
    [SerializeField] Sprite spriteCharacterWing;
    [SerializeField] int costToUnlock;
    public Sprite SpriteCharacter { get => spriteCharacter; set => spriteCharacter = value; }
    public int CostToUnlock { get => costToUnlock; set => costToUnlock = value; }
    public Sprite SpriteCharacterHead { get => spriteCharacterHead; set => spriteCharacterHead = value; }
    public Sprite SpriteCharacterBody { get => spriteCharacterBody; set => spriteCharacterBody = value; }
    public Sprite SpriteCharacterWing { get => spriteCharacterWing; set => spriteCharacterWing = value; }
}

