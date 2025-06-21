using UnityEngine;

public class MergeManager : MonoBehaviour
{
    public GameObject[] mergedPrefabs;

    public void TryMerge(GameObject a, GameObject b)
    {
        int index = GetNextIndex(a);
        if (index >= 0 && index < mergedPrefabs.Length)
        {
            Vector3 pos = (a.transform.position + b.transform.position) / 2;
            Destroy(a); Destroy(b);
            Instantiate(mergedPrefabs[index], pos, Quaternion.identity);
            GameManager.Instance.AddScore((index + 1) * 10);
        }
    }

    int GetNextIndex(GameObject ball)
    {
        for (int i = 0; i < mergedPrefabs.Length; i++)
        {
            if (ball.name.Contains(mergedPrefabs[i].name)) return i + 1;
        }
        return -1;
    }
}