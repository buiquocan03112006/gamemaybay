using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Tốc độ bay của đạn
    public float speed = 10f;

    void Update()
    {
        // Mỗi khung hình, di chuyển lên trên theo trục Y
        transform.Translate(Vector3.up * speed * Time.deltaTime);

        // Tự hủy đạn khi bay ra khỏi màn hình (mẹo nâng cao)
        if (transform.position.y > 6f) 
        {
            Destroy(gameObject);
        }
    }
}