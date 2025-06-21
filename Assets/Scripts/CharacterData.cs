using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 에디터에서 드래그만으로 캐릭터를 정의 가능

GachaManager에서 랜덤 뽑기 대상으로 쓰기 쉬움

능력치 적용도 편하게 GameManager나 BallSpawner에서 적용 가능
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
    public float mergeBonusMultiplier = 1.0f; // 예: 점수 배수 등

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