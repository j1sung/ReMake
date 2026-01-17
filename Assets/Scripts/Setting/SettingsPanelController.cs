using UnityEngine;
using UnityEngine.UI;

public class SettingsPanelController : MonoBehaviour
{
    public static SettingsPanelController Instance;

    [Header("UI")]
    [SerializeField] private GameObject settingPanel; // 설정창 루트
    public GameObject exitPanel; // 설정창 루트
    [SerializeField] private Slider bgmSlider;        // 0~1, wholeNumbers=false
    [SerializeField] private Slider sfxSlider;        // 0~1, wholeNumbers=false

    [SerializeField] private GameObject album;
    [SerializeField] private GameObject albumBook;

    [Header("Audios")]
    [SerializeField] private AudioClip buttonClickClip;  // 버튼 클릭 소리
    [SerializeField] private AudioClip x_ButtonClickClip;  // X 버튼 클릭 소리

    float startBgm, startSfx; // 되돌리기용 원래 값 저장

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


    // ====== 외부에서 '설정' 버튼이 호출 ======
    public void OpenSettingPanel()
    {
        if (!settingPanel) return;

        SFXPlayer.Instance.PlaySFX(buttonClickClip);
        // 현재 값 불러와 슬라이더 갱신 + 되돌리기 백업
        startBgm = AudioManager.Instance.CurrentBgm;
        startSfx = AudioManager.Instance.CurrentSfx;
        Debug.Log("startBgm: "+startBgm+ "startSFX: "+ startSfx);

        settingPanel.SetActive(true);

        bgmSlider.SetValueWithoutNotify(startBgm);
        sfxSlider.SetValueWithoutNotify(startSfx);
    }

    // 슬라이더 변경 시 실시간 반영
    public void OnBgmChanged(float v) => AudioManager.Instance.PreviewBgm(v);
    public void OnSfxChanged(float v) => AudioManager.Instance.PreviewSfx(v);

    // 확인 버튼 → 현재 볼륨 저장 후 패널만 닫기
    public void OnClickSave()
    {   
        SFXPlayer.Instance.PlaySFX(x_ButtonClickClip);
        AudioManager.Instance.SaveVolumes();
        Instance.settingPanel.SetActive(false);
        DataManager.Instance.SaveSettings();
    }

    // 취소 버튼 → 원래 값으로 복원 후 패널만 닫기
    public void OnClickCancel()
    {
        AudioManager.Instance.ForceSetWithoutSave(startBgm, startSfx);
        Instance.settingPanel.SetActive(false);
    }

    // 타이틀로 돌아가기(패널 닫기 + 씬 전환)
    public void OnClickBackToTitle()
    {
        SFXPlayer.Instance.PlaySFX(buttonClickClip);
        if (GameManager.Instance != null)
        {
            GameManager.Instance.MoveScene(SceneData.MainMenu);
        }
        // UI 전체가 아니라 '패널만' 닫음
        if (settingPanel) Instance.settingPanel.SetActive(false);
    }

    public void OnclickAlbum()
    {
        SFXPlayer.Instance.PlaySFX(buttonClickClip);
        album.SetActive(true);
        albumBook.SetActive(true);
    }

    // ====== 종료 UI ======
    public void OpenExitPanel()
    {
        if (!exitPanel) return;
        SFXPlayer.Instance.PlaySFX(buttonClickClip);
        Instance.exitPanel.SetActive(true);
    }

    public void ClosePanel()
    {   
        SFXPlayer.Instance.PlaySFX(x_ButtonClickClip);
        Instance.exitPanel.SetActive(false);
    }
    
    public void QuitGame()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }
}