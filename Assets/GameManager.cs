using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Giao diện UI")]
    public Text scoreText;
    public GameObject gameOverPanel; // Bảng Game Over (Thua)
    public GameObject victoryPanel;  // Bảng Victory (Thắng)

    private int score = 0;
    private bool isGameOver = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        Time.timeScale = 1f; // Trả lại tốc độ thời gian bình thường khi bắt đầu
    }

    void Update()
    {
        // Nhấn R để chơi lại nhanh khi Thua hoặc Thắng
        if (isGameOver && Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
        {
            RestartGame();
        }
    }

    public void AddScore(int amount)
    {
        if (isGameOver) return;
        score += amount;
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }

    // Hàm gọi khi Thua (Hết tim)
    public void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        Time.timeScale = 0f; // Dừng thời gian game
    }

    // Hàm gọi khi Thắng (Diệt xong Boss)
    public void GameVictory()
    {
        if (isGameOver) return;
        isGameOver = true;

        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
        }

        Time.timeScale = 0f; // Dừng thời gian game
    }

    // Hàm gọi khi bấm nút CHƠI LẠI (Restart)
    public void RestartGame()
    {
        Time.timeScale = 1f; // Khôi phục thời gian trước khi load lại Scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Hàm gọi khi bấm nút THOÁT -> Load lại game từ đầu
    public void QuitGame()
    {
        Time.timeScale = 1f; // Mở lại thời gian bình thường
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Chạy lại game từ đầu
    }

    // Kiểm tra trạng thái Game Over
    public bool IsGameOver()
    {
        return isGameOver;
    }
}