using System.Collections.Generic;
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
        if (GameManager.Instance.currentState != GameState.Playing)
            return;

        if (currentBall == null)
        {
            BallData selectedData = MergeManager.Instance.GetRandomBallData();

            // Instantiate()로 랜덤 공 생성
            currentBall = Instantiate(selectedData.prefab, spawnPoint.position, Quaternion.identity);

            // Ball 스크립트에 데이터 주입 (명시성, 유지보수성)
            Ball ballComponent = currentBall.GetComponent<Ball>();
            if (ballComponent != null)
            {
                ballComponent.data = selectedData;
            }

            currentBall.GetComponent<Rigidbody>().isKinematic = true;  // 드래그 중에는 중력 없음
        }

        // 마우스 드래그
        if (Input.GetMouseButton(0))
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;
            currentBall.transform.position = new Vector3(pos.x, spawnPoint.position.y, 0);
            //currentBall.transform.position = new Vector3(pos.x, spawnPoint.position.y, pos.z);
        }

        // 버튼에서 손을 떼면 중력을 켜서 낙하시킴
        if (Input.GetMouseButtonUp(0))
        {
            // 사운드 - 공 떨어질 때
            SoundManager.Instance.PlaySFX(SoundManager.Instance.dropSfxClip);

            currentBall.GetComponent<Rigidbody>().isKinematic = false;
            currentBall = null;
        }
    }


    public void SpawnReviveBall()
    {
        // 중앙에 공 하나 생성
        GameObject ball = Instantiate(currentBall, spawnPoint.position, Quaternion.identity);
        // 또는 마지막 단계보다 낮은 단계 공으로 설정도 가능
    }
}