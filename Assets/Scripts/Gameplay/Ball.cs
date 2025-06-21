using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public int ballLevel; // 0���� ����
    private bool isMerging = false;

    void OnCollisionEnter(Collision collision)
    {
        if (isMerging) return;

        Ball other = collision.gameObject.GetComponent<Ball>();
        if (other != null && other.ballLevel == ballLevel && !other.isMerging)
        {
            isMerging = true;
            other.isMerging = true;
            //���� ������ ���� �浹�ϸ� ���� ��û�� MergeManager�� ����
            MergeManager.Instance.Merge(this, other);
        }
    }
}
