using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    //public int ballLevel; // 0���� ����
    public BallData data;  // level, prefab �� ��� ������ ���⼭ ������
    private bool isMerging = false;
    //���� �̺�Ʈ�� �߰��� �� ���� Ball�� �浹�� ��� �ܺ�(MergeManager ��)�� ���� ��û
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

            //���� ������ ���� �浹�ϸ� ���� ��û�� MergeManager�� ����
            //MergeManager.Instance.Merge(this, other);
        }
    }

    public void ResetMergeState()
    {
        isMerging = false;
    }
}