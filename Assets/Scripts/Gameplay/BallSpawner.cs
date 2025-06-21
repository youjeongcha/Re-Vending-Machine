using UnityEngine;

/*공 생성 & 드래그 제어 */

public class BallSpawner : MonoBehaviour
{
    public Transform spawnPoint;

    private GameObject currentBall;

    /*    public void SpawnBall(int index)
        {
            // Instantiate()로 랜덤 공 생성
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
            
            // Instantiate()로 랜덤 공 생성
            currentBall = Instantiate(
                 MergeManager.Instance.ballPrefabs[rand],
                 spawnPoint.position,
                 Quaternion.identity
            );
            currentBall.GetComponent<Rigidbody>().isKinematic = true;  // 드래그 중에는 중력 없음
        }

        // 마우스 드래그
        if (Input.GetMouseButton(0))
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;
            currentBall.transform.position = new Vector3(pos.x, spawnPoint.position.y, 0);
        }

        // 버튼에서 손을 떼면 중력을 켜서 낙하시킴
        if (Input.GetMouseButtonUp(0))
        {
            currentBall.GetComponent<Rigidbody>().isKinematic = false;
            currentBall = null;
        }
    }
}