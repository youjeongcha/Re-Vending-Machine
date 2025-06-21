using UnityEngine;

/*�� ���� & �巡�� ���� */

public class BallSpawner : MonoBehaviour
{
    public Transform spawnPoint;

    private GameObject currentBall;

    /*    public void SpawnBall(int index)
        {
            // Instantiate()�� ���� �� ����
            Instantiate(
                     MergeManager.Instance.ballPrefabs[rand],
                     spawnPoint.position,
                     Quaternion.identity
                );
        }*/

    void Update()
    {
        if (currentBall == null)
        {
            int rand = Random.Range(0, MergeManager.Instance.ballPrefabs.Length);
            //currentBall = Instantiate(ballPrefabs[rand], spawnPoint.position, Quaternion.identity);
            
            // Instantiate()�� ���� �� ����
            currentBall = Instantiate(
                 MergeManager.Instance.ballPrefabs[rand],
                 spawnPoint.position,
                 Quaternion.identity
            );
            currentBall.GetComponent<Rigidbody>().isKinematic = true;  // �巡�� �߿��� �߷� ����
        }

        // ���콺 �巡��
        if (Input.GetMouseButton(0))
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;
            currentBall.transform.position = new Vector3(pos.x, spawnPoint.position.y, 0);
        }

        // ��ư���� ���� ���� �߷��� �Ѽ� ���Ͻ�Ŵ
        if (Input.GetMouseButtonUp(0))
        {
            currentBall.GetComponent<Rigidbody>().isKinematic = false;
            currentBall = null;
        }
    }
}