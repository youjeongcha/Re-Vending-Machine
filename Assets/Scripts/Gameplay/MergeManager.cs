using UnityEngine;

public class MergeManager : MonoBehaviour
{
    public static MergeManager Instance;

    public InGameManager InGameMgr;

    // ballPrefabs 배열로 단계별 프리팹을 관리
    //public GameObject[] ballPrefabs;

    public BallData[] ballDataList;  // 인덱스 = 레벨

    private int CurrentMaxLevel = 3; // 예시. 게임 플레이 중에 점점 증가 가능.


    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void OnEnable()
    {
        Ball.OnMergeRequested += HandleMerge;
    }

    void OnDisable()
    {
        Ball.OnMergeRequested -= HandleMerge;
    }

    private void HandleMerge(Ball a, Ball b)
    {
        if (a == null || b == null) return;

        // 물리값 초기화: 튕김 방지
        Rigidbody rbA = a.GetComponent<Rigidbody>();
        Rigidbody rbB = b.GetComponent<Rigidbody>();

        //velocity와 angularVelocity를 zero로 설정해서 충돌 후 튀는 걸 막기

        if (rbA != null) { rbA.velocity = Vector3.zero; rbA.angularVelocity = Vector3.zero; rbA.isKinematic = true; }
        if (rbB != null) { rbB.velocity = Vector3.zero; rbB.angularVelocity = Vector3.zero; rbB.isKinematic = true; }

        Vector3 mergePos = (a.transform.position + b.transform.position) / 2.0f;
        mergePos.z = 0f; // 병합 후 생성 위치의 Z축을 0으로 고정

        int nextLevel = a.data.level + 1;

        // 두 공을 제거하고 다음 단계 공으로 교체
        Destroy(a.gameObject);
        Destroy(b.gameObject);

        // 사운드 - 병합 순간
        SoundManager.Instance.PlayMergeSFXByEffect("normal");

        if (nextLevel < ballDataList.Length)
        {
            BallData nextData = ballDataList[nextLevel];
            GameObject newBallObj = Instantiate(nextData.prefab, mergePos, Quaternion.identity);
            Ball newBall = newBallObj.GetComponent<Ball>();
            newBall.data = nextData; // 명시성, 유지보수성

            CurrentMaxLevel = newBall.data.level; // 내가 최대 만든 공 Level저장

            // 공이 충돌 직후에 불필요하게 밀리거나 튕겨서 버그 발생하는 걸 방지
            Rigidbody rbNew = newBall.GetComponent<Rigidbody>();
            if (rbNew != null) { rbNew.velocity = Vector3.zero; rbNew.angularVelocity = Vector3.zero; }

            // 병합 이펙트
            /*            if (nextData.mergeEffect != null)
                        {
                            var effect = Instantiate(nextData.mergeEffect, mergePos, Quaternion.identity);
                            Destroy(effect.gameObject, 0.7f); // 0.7초 후 자동 제거
                        }*/

            // 점수 추가
            InGameMgr.AddScore(nextData.score);
        }
        else
        {
            Debug.Log("최고 단계입니다!");
        }
    }

    public BallData GetRandomBallData()
    {
        int makeMaxLevel = Mathf.Max(0, CurrentMaxLevel - 2); // 2레벨 아래까지만 생성 허용
        makeMaxLevel = Mathf.Min(makeMaxLevel, ballDataList.Length - 1); // 최대 레벨이 전체 ballDataList 범위를 넘지 않도록 제한

        int baseWeight = 10;  // 레벨 0의 기본 확률 가중치
        int step = 3;         // 레벨이 올라갈수록 확률 감소폭 // 10~20 정도로 조정하면 확률 차이가 드라마틱

        int totalWeight = 0;
        for (int i = 0; i <= makeMaxLevel; i++)
        {
            int weight = Mathf.Max(1, baseWeight - i * step); // 음수 방지
            totalWeight += weight;
        }

        // totalWeight가 0이면 Random.Range 에러 발생하므로 방어 코드 추가
        if (totalWeight <= 0)
        {
            Debug.LogWarning("GetRandomBallData: totalWeight가 0 이하입니다. fallback 반환.");
            return ballDataList[0];
        }

        // 예를 들어, 레벨 0의 확률 범위가 0~5라면 rand < 6이면 0레벨 선택.
        int rand = Random.Range(0, totalWeight);
        int current = 0;

        for (int i = 0; i <= makeMaxLevel; i++)
        {
            int weight = Mathf.Max(1, baseWeight - i * step);
            current += weight;
            if (rand < current)
                return ballDataList[i];
        }

        Debug.LogWarning("GetRandomBallData: fallback 진입.");
        return ballDataList[0]; // fallback

        /*레벨이 낮을수록 훨씬 더 자주 나옴

        예를 들어 baseWeight = 10, step = 3이면:
        레벨 0 → 10
        레벨 1 → 7
        레벨 2 → 4
        레벨 3 → 1
        */
    }


}