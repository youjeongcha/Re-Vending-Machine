using UnityEngine;

public class MergeManager : MonoBehaviour
{
    public static MergeManager Instance;

    public InGameManager InGameMgr;

    // ballPrefabs �迭�� �ܰ躰 �������� ����
    //public GameObject[] ballPrefabs;

    public BallData[] ballDataList;  // �ε��� = ����

    private int CurrentMaxLevel = 3; // ����. ���� �÷��� �߿� ���� ���� ����.


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

        // ������ �ʱ�ȭ: ƨ�� ����
        Rigidbody rbA = a.GetComponent<Rigidbody>();
        Rigidbody rbB = b.GetComponent<Rigidbody>();

        //velocity�� angularVelocity�� zero�� �����ؼ� �浹 �� Ƣ�� �� ����

        if (rbA != null) { rbA.velocity = Vector3.zero; rbA.angularVelocity = Vector3.zero; rbA.isKinematic = true; }
        if (rbB != null) { rbB.velocity = Vector3.zero; rbB.angularVelocity = Vector3.zero; rbB.isKinematic = true; }

        Vector3 mergePos = (a.transform.position + b.transform.position) / 2.0f;
        mergePos.z = 0f; // ���� �� ���� ��ġ�� Z���� 0���� ����

        int nextLevel = a.data.level + 1;

        // �� ���� �����ϰ� ���� �ܰ� ������ ��ü
        Destroy(a.gameObject);
        Destroy(b.gameObject);

        // ���� - ���� ����
        SoundManager.Instance.PlayMergeSFXByEffect("normal");

        if (nextLevel < ballDataList.Length)
        {
            BallData nextData = ballDataList[nextLevel];
            GameObject newBallObj = Instantiate(nextData.prefab, mergePos, Quaternion.identity);
            Ball newBall = newBallObj.GetComponent<Ball>();
            newBall.data = nextData; // ��ü�, ����������

            CurrentMaxLevel = newBall.data.level; // ���� �ִ� ���� �� Level����

            // ���� �浹 ���Ŀ� ���ʿ��ϰ� �и��ų� ƨ�ܼ� ���� �߻��ϴ� �� ����
            Rigidbody rbNew = newBall.GetComponent<Rigidbody>();
            if (rbNew != null) { rbNew.velocity = Vector3.zero; rbNew.angularVelocity = Vector3.zero; }

            // ���� ����Ʈ
            /*            if (nextData.mergeEffect != null)
                        {
                            var effect = Instantiate(nextData.mergeEffect, mergePos, Quaternion.identity);
                            Destroy(effect.gameObject, 0.7f); // 0.7�� �� �ڵ� ����
                        }*/

            // ���� �߰�
            InGameMgr.AddScore(nextData.score);
        }
        else
        {
            Debug.Log("�ְ� �ܰ��Դϴ�!");
        }
    }

    public BallData GetRandomBallData()
    {
        int makeMaxLevel = Mathf.Max(0, CurrentMaxLevel - 2); // 2���� �Ʒ������� ���� ���
        makeMaxLevel = Mathf.Min(makeMaxLevel, ballDataList.Length - 1); // �ִ� ������ ��ü ballDataList ������ ���� �ʵ��� ����

        int baseWeight = 10;  // ���� 0�� �⺻ Ȯ�� ����ġ
        int step = 3;         // ������ �ö󰥼��� Ȯ�� ������ // 10~20 ������ �����ϸ� Ȯ�� ���̰� ���ƽ

        int totalWeight = 0;
        for (int i = 0; i <= makeMaxLevel; i++)
        {
            int weight = Mathf.Max(1, baseWeight - i * step); // ���� ����
            totalWeight += weight;
        }

        // totalWeight�� 0�̸� Random.Range ���� �߻��ϹǷ� ��� �ڵ� �߰�
        if (totalWeight <= 0)
        {
            Debug.LogWarning("GetRandomBallData: totalWeight�� 0 �����Դϴ�. fallback ��ȯ.");
            return ballDataList[0];
        }

        // ���� ���, ���� 0�� Ȯ�� ������ 0~5��� rand < 6�̸� 0���� ����.
        int rand = Random.Range(0, totalWeight);
        int current = 0;

        for (int i = 0; i <= makeMaxLevel; i++)
        {
            int weight = Mathf.Max(1, baseWeight - i * step);
            current += weight;
            if (rand < current)
                return ballDataList[i];
        }

        Debug.LogWarning("GetRandomBallData: fallback ����.");
        return ballDataList[0]; // fallback

        /*������ �������� �ξ� �� ���� ����

        ���� ��� baseWeight = 10, step = 3�̸�:
        ���� 0 �� 10
        ���� 1 �� 7
        ���� 2 �� 4
        ���� 3 �� 1
        */
    }


}