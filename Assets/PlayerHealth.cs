using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;

    [Header("Giao diện UI")]
    public GameObject[] hearts; // Mảng chứa 3 tim UI (Kéo Heart1, Heart1 (1), Heart1 (2) vào đây)

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHeartUI();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;

        UpdateHeartUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateHeartUI()
    {
        // Tự động ẩn/hiện tim theo currentHealth
        for (int i = 0; i < hearts.Length; i++)
        {
            if (hearts[i] != null)
            {
                hearts[i].SetActive(i < currentHealth);
            }
        }
    }

    void Die()
    {
        // 1. Báo cho GameManager biết là đã Game Over (GameManager sẽ bật Panel và dừng time)
        if (GameManager.Instance != null)
        {
            GameManager.Instance.GameOver();
        }

        // 2. Xóa Player khỏi màn hình
        Destroy(gameObject);
    }
}