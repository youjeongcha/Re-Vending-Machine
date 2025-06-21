using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * �����Ϳ��� �巡�׸����� ĳ���͸� ���� ����

GachaManager���� ���� �̱� ������� ���� ����

�ɷ�ġ ���뵵 ���ϰ� GameManager�� BallSpawner���� ���� ����
*/

[CreateAssetMenu(menuName = "Character/Character Data", fileName = "NewCharacterData")]
public class CharacterData : ScriptableObject
{
    public string characterName;
    public Sprite icon;
    public string description;

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