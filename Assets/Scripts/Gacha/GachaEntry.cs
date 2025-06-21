using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GachaEntry
{
    public CharacterData character;     // 실제 캐릭터 데이터
    [Range(0f, 100f)]
    public float weight = 1.0f;         // 등장 확률 가중치
    public string rarity = "Common";    // 희귀도 구분
    public string group = "Default";    // 그룹 (예: 시즌한정 등)
}
