using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
    public static BGMPlayer Instance;

    private AudioSource source;
    private Coroutine fadeRoutine;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        source = GetComponent<AudioSource>();
        if (source == null) source = gameObject.AddComponent<AudioSource>();
        source.loop = true;
    }

    /// <summary>새로운 BGM으로 자연스럽게 전환 (페이드 아웃 → 클립 교체 → 페이드 인)</summary>
    public void PlayBGM(AudioClip clip, float fadeSeconds = 0.5f)
    {
        if (source.clip == clip && source.isPlaying) return;

        if (fadeRoutine != null) StopCoroutine(fadeRoutine);
        fadeRoutine = StartCoroutine(FadeSwap(clip, fadeSeconds));
    }

    /// <summary>현재 재생 중인 BGM을 서서히 끄기</summary>
    public void StopBGM(float fadeSeconds = 0.5f)
    {
        if (!source.isPlaying) return;

        if (fadeRoutine != null) StopCoroutine(fadeRoutine);
        fadeRoutine = StartCoroutine(FadeOutStop(fadeSeconds));
    }

    // -------- 내부 보조 --------
    private System.Collections.IEnumerator FadeSwap(AudioClip next, float t)
    {
        float prevVol = source.volume;

        // 1) 기존 BGM 페이드아웃
        if (source.isPlaying && t > 0f)
            yield return FadeVolume(prevVol, 0f, t * 0.5f);

        // 2) 클립 교체 후 재생
        source.clip = next;
        source.Play();

        // 3) 새 BGM 페이드인
        if (t > 0f)
            yield return FadeVolume(0f, 1f, t * 0.5f);
        else
            source.volume = 1f;

        fadeRoutine = null;
    }

    private System.Collections.IEnumerator FadeOutStop(float t)
    {
        float prevVol = source.volume;

        if (t > 0f)
            yield return FadeVolume(prevVol, 0f, t);

        source.Stop();
        source.volume = 1f; // 다음 재생 대비 초기화
        fadeRoutine = null;
    }

    private System.Collections.IEnumerator FadeVolume(float from, float to, float duration)
    {
        if (duration <= 0f) { source.volume = to; yield break; }

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;  // 일시정지 중에도 페이드되게
            source.volume = Mathf.Lerp(from, to, elapsed / duration);
            yield return null;
        }
        source.volume = to;
    }
}