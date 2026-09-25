using UnityEngine;
using UnityEngine.InputSystem; // Sử dụng Input System mới

public class Player : MonoBehaviour
{
    public GameObject bulletPrefab;

    [Header("Tự động bắn đạn")]
    public float fireRate = 0.15f; // Tốc độ bắn (0.15 giây bắn 1 viên đạn)
    private float nextFireTime = 0f;

    [Header("Âm thanh Bắn Đạn")]
    public AudioClip shootSound; // File âm thanh tiếng bắn
    private AudioSource audioSource;

    // Các biến giới hạn viền màn hình
    private float minX, maxX, minY, maxY;
    public float padding = 0.5f; // Khoảng đệm để máy bay không bị chệch ra ngoài

    private Camera mainCam;

    void Start()
    {
        // Lấy component AudioSource được gắn trên Player
        audioSource = GetComponent<AudioSource>();
        mainCam = Camera.main;

        // Tự động tính giới hạn màn hình dựa trên Main Camera
        if (mainCam != null)
        {
            Vector3 minBounds = mainCam.ViewportToWorldPoint(new Vector3(0, 0, 0));
            Vector3 maxBounds = mainCam.ViewportToWorldPoint(new Vector3(1, 1, 0));

            minX = minBounds.x + padding;
            maxX = maxBounds.x - padding;
            minY = minBounds.y + padding;
            maxY = maxBounds.y - padding;
        }
    }

    void Update()
    {
        // 1. TỰ ĐỘNG DI CHUYỂN THEO CHUỘT (Dùng New Input System)
        if (mainCam != null && Mouse.current != null)
        {
            // Lấy vị trí con trỏ chuột từ Mouse.current
            Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
            Vector3 mousePos = new Vector3(mouseScreenPos.x, mouseScreenPos.y, -mainCam.transform.position.z);

            Vector3 targetPos = mainCam.ScreenToWorldPoint(mousePos);

            // Giới hạn vị trí máy bay nằm trong khung màn hình (Clamp)
            targetPos.x = Mathf.Clamp(targetPos.x, minX, maxX);
            targetPos.y = Mathf.Clamp(targetPos.y, minY, maxY);
            targetPos.z = transform.position.z; // Giữ nguyên Z của Player

            // Cho máy bay di chuyển theo vị trí chuột (Lerp mượt mà)
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 25f);
        }

        // 2. TỰ ĐỘNG BẮN ĐẠN LIÊN TỤC
        if (Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        if (bulletPrefab != null)
        {
            // Tạo đạn ngay phía trên Player 0.8 đơn vị
            Vector3 spawnPos = transform.position + new Vector3(0, 0.8f, 0);
            Instantiate(bulletPrefab, spawnPos, Quaternion.identity);
        }

        // Phát âm thanh bắn đạn
        if (audioSource != null && shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }
    }
}