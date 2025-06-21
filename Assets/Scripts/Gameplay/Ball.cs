using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public int ballLevel; // 0부터 시작
    private bool isMerging = false;

    void OnCollisionEnter(Collision collision)
    {
        if (isMerging) return;

        Ball other = collision.gameObject.GetComponent<Ball>();
        if (other != null && other.ballLevel == ballLevel && !other.isMerging)
        {
            isMerging = true;
            other.isMerging = true;
            //같은 레벨의 공이 충돌하면 병합 요청을 MergeManager로 보냄
            MergeManager.Instance.Merge(this, other);
        }
    }
}
