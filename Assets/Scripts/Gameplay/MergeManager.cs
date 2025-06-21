using UnityEngine;

public class MergeManager : MonoBehaviour
{
    public static MergeManager Instance;

    // ballPrefabs �迭�� �ܰ躰 �������� ����
    public GameObject[] ballPrefabs;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void Merge(Ball a, Ball b)
    {
        if (a == null || b == null) return;

        Vector3 pos = (a.transform.position + b.transform.position) / 2.0f;
        int nextLevel = a.ballLevel + 1; ;

        // �� ���� �����ϰ� ���� �ܰ� ������ ��ü
        Destroy(a.gameObject);
        Destroy(b.gameObject);

        if (nextLevel < ballPrefabs.Length)
        {
            GameObject newBall = Instantiate(ballPrefabs[nextLevel], pos, Quaternion.identity);
            //GameManager.Instance.AddScore((nextLevel + 1) * 10);  // ���� ����
        }
        else
        {
            Debug.Log("�ְ� �ܰ��Դϴ�!");
        }
    }

/*    int GetNextIndex(Ball ball)
    {
        for (int i = 0; i < ballPrefabs.Length; i++)
        {
            if (ball.name.Contains(ballPrefabs[i].name)) return i + 1;
        }
        return -1;
    }*/
}