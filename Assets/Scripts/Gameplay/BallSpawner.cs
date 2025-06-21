using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public GameObject[] ballPrefabs;
    public Transform spawnPoint;

    public void SpawnBall(int index)
    {
        Instantiate(ballPrefabs[index], spawnPoint.position, Quaternion.identity);
    }
}