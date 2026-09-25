using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Kẻ thù để sinh ra
    public float spawnRate = 3f;  // Cứ 3 giây tạo 1 con quái
    public float xMin = -7f;        // Vị trí mép trái
    public float xMax = 7f;         // Vị trí mép phải
    public float spawnY = 6f;       // Độ cao xuất hiện ở trên đỉnh màn hình

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnRate)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    void SpawnEnemy()
    {
        if (enemyPrefab != null)
        {
            // Chọn vị trí ngẫu nhiên hàng ngang từ xMin đến xMax
            float randomX = Random.Range(xMin, xMax);
            Vector3 spawnPos = new Vector3(randomX, spawnY, 0f);
            Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        }
    }
}