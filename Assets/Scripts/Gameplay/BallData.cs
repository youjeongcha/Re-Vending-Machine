using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBallData", menuName = "Ball/Ball Data")]
public class BallData : ScriptableObject
{
    public int level;               // ���� ����
    //public string ballName;         // �̸� (��: ���, ������)
    public GameObject prefab;       // ������
    public int score;               // ���� �� ����
    //public Sprite icon;             // UI ������ ��
    //public ParticleSystem mergeEffect; // ���� �� ����Ʈ
}