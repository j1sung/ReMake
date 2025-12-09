using UnityEngine;

public class SFXPlayer : MonoBehaviour
{
    public static SFXPlayer Instance;

    private AudioSource source;           // 이 오브젝트의 AudioSource

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        source = GetComponent<AudioSource>();
    }

    // 외부에서 호출해서 재생할 수 있는 함수
    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;
        source.PlayOneShot(clip);
    }
}