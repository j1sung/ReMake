using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Mixer 연결")]
    [SerializeField] private AudioMixer mixer;          
    [SerializeField] private string bgmParam = "BGMVol"; 
    [SerializeField] private string sfxParam = "SFXVol"; 

    public float CurrentBgm { get; private set; }
    public float CurrentSfx { get; private set; }

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // 기본값으로 초기화
        CurrentBgm = 0.5f;
        CurrentSfx = 0.5f;

        ApplyBgm(CurrentBgm);
        ApplySfx(CurrentSfx);
    }

    // 슬라이더 조작 중 실시간 반영
    public void PreviewBgm(float v) { CurrentBgm = Mathf.Clamp01(v); ApplyBgm(CurrentBgm); }
    public void PreviewSfx(float v) { CurrentSfx = Mathf.Clamp01(v); ApplySfx(CurrentSfx); }

    // 확인 버튼 시 (저장 안 함, 단순 반영)
    public void SaveVolumes()
    {
        ApplyBgm(CurrentBgm);
        ApplySfx(CurrentSfx);
    }

    // 취소 버튼 시 이전 값으로 되돌리기
    public void ForceSetWithoutSave(float bgm, float sfx)
    {
        CurrentBgm = Mathf.Clamp01(bgm);
        CurrentSfx = Mathf.Clamp01(sfx);
        ApplyBgm(CurrentBgm);
        ApplySfx(CurrentSfx);
    }

    // 실제 믹서에 반영
    void ApplyBgm(float v) => mixer.SetFloat(bgmParam, v <= 0.0001f ? -80f : 20f * Mathf.Log10(v));
    void ApplySfx(float v) => mixer.SetFloat(sfxParam, v <= 0.0001f ? -80f : 20f * Mathf.Log10(v));
}