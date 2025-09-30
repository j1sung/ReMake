using UnityEngine;
using UnityEngine.Audio;

public class SettingManager : MonoBehaviour
{
    public static SettingManager Instance { get; private set; }

    [Header("Audio")]
    [SerializeField] private AudioMixer mixer;        // Master에 연결
    [SerializeField] private string paramName = "MasterVolume";
    private const string PrefKey = "master_volume";

    public float CurrentVolume { get; private set; } = 1f; // 0~1

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // 로드 & 적용
        CurrentVolume = PlayerPrefs.GetFloat(PrefKey, 1f);
        ApplyVolume(CurrentVolume);
    }

    public void PreviewVolume(float v)    // 슬라이더 드래그 동안 호출(미리듣기)
    {
        CurrentVolume = Mathf.Clamp01(v);
        ApplyVolume(CurrentVolume);
    }

    public void SaveVolume()              // 확인 버튼에서 호출
    {
        PlayerPrefs.SetFloat(PrefKey, CurrentVolume);
        PlayerPrefs.Save();
    }

    public void SetAndSaveVolume(float v) // 필요 시 한 번에 저장까지
    {
        PreviewVolume(v);
        SaveVolume();
    }

    public void ForceSetWithoutSave(float v) // 취소 시 되돌리기용
    {
        CurrentVolume = Mathf.Clamp01(v);
        ApplyVolume(CurrentVolume);
        // 저장 X
    }

    private void ApplyVolume(float v)
    {
        float db = v <= 0.0001f ? -80f : Mathf.Log10(v) * 20f;
        mixer.SetFloat(paramName, db);
    }
}