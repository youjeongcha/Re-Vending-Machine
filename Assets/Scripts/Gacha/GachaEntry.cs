using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GachaEntry
{
    public CharacterData character;     // ���� ĳ���� ������
    [Range(0f, 100f)]
    public float weight = 1.0f;         // ���� Ȯ�� ����ġ
    public string rarity = "Common";    // ��͵� ����
    public string group = "Default";    // �׷� (��: �������� ��)
}
