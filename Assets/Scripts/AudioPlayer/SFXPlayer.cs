using UnityEngine;
using UnityEngine.Audio;

public class SFXPlayer : MonoBehaviour
{
    public static SFXPlayer Instance;

    [Header("Audio Mixer")]
    [SerializeField] private AudioMixerGroup sfxGroup; // SFX 그룹

    private AudioSource oneShotSource;
    private AudioSource loopSource;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // 기존 AudioSource (원샷용)
        oneShotSource = GetComponent<AudioSource>();

        // 루프용 AudioSource 추가
        loopSource = gameObject.AddComponent<AudioSource>();
        loopSource.loop = true;
        loopSource.playOnAwake = false;

        // 두 소스 모두 SFX Mixer Group으로 출력
        if (sfxGroup != null)
        {
            oneShotSource.outputAudioMixerGroup = sfxGroup;
            loopSource.outputAudioMixerGroup = sfxGroup;
        }
    }

    // 효과음 한번 재생
    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;
        oneShotSource.PlayOneShot(clip);
    }

    // 효과음 중단
    public void StopSFX()
    {
        oneShotSource.Stop();
    }

    // 루프 효과음 재생
    public void PlayLoop(AudioClip clip)
    {
        if (clip == null) return;
        if (loopSource.isPlaying && loopSource.clip == clip) return;

        loopSource.clip = clip;
        loopSource.Play();
    }

    // 루프 여부 판별
    public bool IsLoopPlaying()
    {
        return loopSource.isPlaying;
    }

    // 루프 정지
    public void StopLoop()
    {
        if (!loopSource.isPlaying) return;

        loopSource.Stop();
        loopSource.clip = null;
    }
}