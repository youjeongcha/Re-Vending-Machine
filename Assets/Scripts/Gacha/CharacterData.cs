using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character/Character Data", fileName = "NewCharacterData")]
public class CharacterData : ScriptableObject
{
    public string characterName;
    public Sprite icon;
    public string description;
    public Dictionary<string, float> stat = new(); // Ȯ�� ����Ʈ

    [Header("Gameplay Stats")]
    public float jumpPower = 1.0f;
    public float gravityScale = 1.0f;
    public float mergeBonusMultiplier = 1.0f; // ��: ���� ��� ��

    [Header("Collection Info")]
    public Rarity rarity;
    public bool isUnlockedByDefault = false;
}

public enum Rarity
{
    Common,
    Rare,
    Epic,
    Legendary
}
