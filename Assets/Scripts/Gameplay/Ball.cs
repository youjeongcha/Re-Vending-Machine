using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    //public int ballLevel; // 0부터 시작
    public BallData data;  // level, prefab 등 모든 정보는 여기서 가져옴
    private bool isMerging = false;
    //정적 이벤트를 추가해 두 개의 Ball이 충돌할 경우 외부(MergeManager 등)로 병합 요청
    public static event Action<Ball, Ball> OnMergeRequested;

    void OnCollisionEnter(Collision collision)
    {
        if (isMerging) return;

        Ball other = collision.gameObject.GetComponent<Ball>();
        if (other != null && other.data.level == data.level && !other.isMerging)
        {
            isMerging = true;
            other.isMerging = true;

            OnMergeRequested?.Invoke(this, other);

            //같은 레벨의 공이 충돌하면 병합 요청을 MergeManager로 보냄
            //MergeManager.Instance.Merge(this, other);
        }
    }

    public void ResetMergeState()
    {
        isMerging = false;
    }
}