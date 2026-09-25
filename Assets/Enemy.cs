using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 3f;
    public GameObject explosionPrefab;

    [Header("Âm thanh")]
    public AudioClip explosionSound; // File âm thanh tiếng nổ

    void Update()
    {
        // Quái tự động rơi xuống
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        // Chạm đáy màn hình -> Thua Game
        if (transform.position.y < -6f)
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.GameOver();
            }
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        bool isBullet = other.CompareTag("Bullet") || other.gameObject.name.ToLower().Contains("bullet");

        // Bị Đạn bắn trúng -> Nổ, Cộng 10 điểm, xóa Đạn và Quái
        if (isBullet)
        {
            Die();
            Destroy(other.gameObject);
        }
        // Tông vào Player -> Sinh hiệu ứng nổ, Trừ 1 tim, Xóa Quái
        else if (other.CompareTag("Player") || other.gameObject.name.ToLower().Contains("player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1);
            }

            Die();
        }
    }

    // Hàm xử lý khi kẻ địch bị tiêu diệt
    void Die()
    {
        // 1. Sinh ra hiệu ứng nổ tại vị trí kẻ địch
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }

        // 2. Phát tiếng nổ độc lập tại vị trí kẻ địch (không bị ngắt khi Enemy bị Destroy)
        if (explosionSound != null)
        {
            AudioSource.PlayClipAtPoint(explosionSound, Camera.main.transform.position);
        }

        // 3. Cộng điểm
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddScore(10);
        }

        // 4. Xóa kẻ địch
        Destroy(gameObject);
    }
}