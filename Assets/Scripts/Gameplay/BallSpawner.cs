using System.Collections.Generic;
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
        if (GameManager.Instance.currentState != GameState.Playing)
            return;

        if (currentBall == null)
        {
            BallData selectedData = MergeManager.Instance.GetRandomBallData();

            // Instantiate()�� ���� �� ����
            currentBall = Instantiate(selectedData.prefab, spawnPoint.position, Quaternion.identity);

            // Ball ��ũ��Ʈ�� ������ ���� (��ü�, ����������)
            Ball ballComponent = currentBall.GetComponent<Ball>();
            if (ballComponent != null)
            {
                ballComponent.data = selectedData;
            }

            currentBall.GetComponent<Rigidbody>().isKinematic = true;  // �巡�� �߿��� �߷� ����
        }

        // ���콺 �巡��
        if (Input.GetMouseButton(0))
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;
            currentBall.transform.position = new Vector3(pos.x, spawnPoint.position.y, 0);
            //currentBall.transform.position = new Vector3(pos.x, spawnPoint.position.y, pos.z);
        }

        // ��ư���� ���� ���� �߷��� �Ѽ� ���Ͻ�Ŵ
        if (Input.GetMouseButtonUp(0))
        {
            // ���� - �� ������ ��
            SoundManager.Instance.PlaySFX(SoundManager.Instance.dropSfxClip);

            currentBall.GetComponent<Rigidbody>().isKinematic = false;
            currentBall = null;
        }
    }


    public void SpawnReviveBall()
    {
        // �߾ӿ� �� �ϳ� ����
        GameObject ball = Instantiate(currentBall, spawnPoint.position, Quaternion.identity);
        // �Ǵ� ������ �ܰ躸�� ���� �ܰ� ������ ������ ����
    }
}