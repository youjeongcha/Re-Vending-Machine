using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBallData", menuName = "Ball/Ball Data")]
public class BallData : ScriptableObject
{
    public int level;               // 병합 레벨
    //public string ballName;         // 이름 (예: 사과, 오렌지)
    public GameObject prefab;       // 프리팹
    public int score;               // 병합 시 점수
    //public Sprite icon;             // UI 아이콘 등
    //public ParticleSystem mergeEffect; // 병합 시 이펙트
}