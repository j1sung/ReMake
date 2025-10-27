using UnityEngine;

public class AlbumManager : MonoBehaviour
{
    public static AlbumManager instance { get; private set; }
    private void Awake()
    {
        // ΩÃ±€≈Ê ∫∏¿Â
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
