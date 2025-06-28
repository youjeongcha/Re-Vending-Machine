
using UnityEngine;

// 각 공이 일정한 Z축으로 천천히 정렬되도록 유도하는 컴포넌트
public class BallZAligner : MonoBehaviour
{
    public float zAlignSpeed = 2f;
    public float targetZ = 0f;

    Rigidbody rb;


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Vector3 pos = rb.position;
        pos.z = Mathf.Lerp(pos.z, targetZ, Time.fixedDeltaTime * zAlignSpeed);
        rb.MovePosition(pos);
    }

}
