using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public float speed = 5f;

    void Update()
    {
        // Đạn luôn bay thẳng xuống dưới
        transform.Translate(Vector3.down * speed * Time.deltaTime, Space.World);

        // Ra khỏi màn hình phía dưới thì tự xóa
        if (transform.position.y < -6f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Bỏ qua va chạm với Boss và đạn khác
        if (other.gameObject.name.ToLower().Contains("boss") || 
            other.gameObject.name.ToLower().Contains("bullet") || 
            other.CompareTag("Bullet"))
        {
            return;
        }

        // Tìm script PlayerHealth trên Object va chạm hoặc trên Parent (nếu Collider nằm ở con)
        PlayerHealth playerHealth = other.GetComponentInParent<PlayerHealth>();
        if (playerHealth == null)
        {
            playerHealth = other.GetComponent<PlayerHealth>();
        }

        // Nếu tìm thấy PlayerHealth -> Trừ máu và xóa đạn ngay
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(1);
            Destroy(gameObject);
        }
    }
}