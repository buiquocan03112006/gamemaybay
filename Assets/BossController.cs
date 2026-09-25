using UnityEngine;
using UnityEngine.UI; // Cần thiết để sử dụng UI Slider

public class BossController : MonoBehaviour
{
    [Header("Máu & Di chuyển")]
    public int maxHealth = 100;
    private int currentHealth;
    public float speed = 2f;
    public float leftLimit = -3f;
    public float rightLimit = 3f;
    private bool movingRight = true;

    [Header("Giao diện UI")]
    public Slider healthBar; // Kéo BossHealthBar vào đây

    [Header("Tấn công")]
    public GameObject bossBulletPrefab;
    public Transform firePoint;
    public float fireRate = 1.5f;
    private float nextFireTime = 0f;

    [Header("Âm thanh")]
    public AudioClip shootSound;     // File âm thanh bắn đạn của Boss
    public AudioClip explosionSound; // File âm thanh tiếng nổ khi Boss diệt
    private AudioSource audioSource; // Component phát âm thanh

    [Header("Hiệu ứng")]
    public GameObject explosionPrefab;

    void Start()
    {
        currentHealth = maxHealth;

        // Cấu hình thanh máu UI ban đầu
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
            healthBar.gameObject.SetActive(true); // Hiện thanh máu khi Boss xuất hiện
        }

        // Lấy component AudioSource được gắn trên Boss
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // 1. Boss di chuyển qua lại lơ lửng trên cao
        MoveLoop();

        // 2. Tự động bắn đạn theo thời gian
        if (Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void MoveLoop()
    {
        if (movingRight)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
            if (transform.position.x >= rightLimit)
                movingRight = false;
        }
        else
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
            if (transform.position.x <= leftLimit)
                movingRight = true;
        }
    }

    void Shoot()
    {
        if (bossBulletPrefab != null)
        {
            Vector3 spawnPos = firePoint != null ? firePoint.position : transform.position;
            Instantiate(bossBulletPrefab, spawnPos, Quaternion.identity);
        }

        // Phát âm thanh bắn đạn của Boss
        if (audioSource != null && shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Bị đạn Player bắn trúng (Nhận diện theo Tag hoặc Tên)
        bool isPlayerBullet = other.CompareTag("Bullet") || other.gameObject.name.ToLower().Contains("bullet");

        if (isPlayerBullet)
        {
            TakeDamage(1);
            Destroy(other.gameObject);
        }
        // Boss tông vào Player -> Trừ máu Player
        else if (other.CompareTag("Player") || other.gameObject.name.ToLower().Contains("player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // Cập nhật giá trị thanh máu UI
        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // 1. Hiệu ứng nổ
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }

        // 2. Phát tiếng nổ
        if (explosionSound != null)
        {
            AudioSource.PlayClipAtPoint(explosionSound, Camera.main.transform.position);
        }

        // 3. Ẩn thanh máu khi Boss chết
        if (healthBar != null)
        {
            healthBar.gameObject.SetActive(false);
        }

        // 4. Cộng điểm & Hiển thị màn hình BẠN ĐÃ THẮNG
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddScore(1000);
            GameManager.Instance.GameVictory(); // Kích hoạt UI Thắng
        }

        Destroy(gameObject);
    }
}