using UnityEngine;

public class MergeManager : MonoBehaviour
{
    public static MergeManager Instance;

    // ballPrefabs 배열로 단계별 프리팹을 관리
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

        // 두 공을 제거하고 다음 단계 공으로 교체
        Destroy(a.gameObject);
        Destroy(b.gameObject);

        if (nextLevel < ballPrefabs.Length)
        {
            GameObject newBall = Instantiate(ballPrefabs[nextLevel], pos, Quaternion.identity);
            //GameManager.Instance.AddScore((nextLevel + 1) * 10);  // 예시 점수
        }
        else
        {
            Debug.Log("최고 단계입니다!");
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