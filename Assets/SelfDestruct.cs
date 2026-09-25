using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    public float lifetime = 0.5f; // Thời gian tồn tại (0.5 giây)

    void Start()
    {
        // Tự động xóa GameObject này sau 0.5 giây
        Destroy(gameObject, lifetime);
    }
}