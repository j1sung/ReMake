using UnityEngine;
using UnityEngine.UI;

public class SettingsPanelController : MonoBehaviour
{
    public static SettingsPanelController Instance;

    [Header("UI")]
    [SerializeField] private GameObject settingPanel; // 설정창 루트
    [SerializeField] private Slider bgmSlider;        // 0~1, wholeNumbers=false
    [SerializeField] private Slider sfxSlider;        // 0~1, wholeNumbers=false

    [SerializeField] private GameObject album;
    [SerializeField] private GameObject albumBook;

    float startBgm, startSfx; // 되돌리기용 원래 값 저장

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // 처음에는 패널 닫아둠(원하면 열어둬도 됨)
        if (settingPanel) Instance.settingPanel.SetActive(false);
    }


    // ====== 외부에서 '설정' 버튼이 호출 ======
    public void OpenPanel()
    {
        if (!settingPanel) return;

        // 현재 값 불러와 슬라이더 갱신 + 되돌리기 백업
        startBgm = AudioManager.Instance.CurrentBgm;
        startSfx = AudioManager.Instance.CurrentSfx;

        bgmSlider.SetValueWithoutNotify(startBgm);
        sfxSlider.SetValueWithoutNotify(startSfx);

        Instance.settingPanel.SetActive(true);
    }

    // 슬라이더 변경 시 실시간 반영
    public void OnBgmChanged(float v) => AudioManager.Instance.PreviewBgm(v);
    public void OnSfxChanged(float v) => AudioManager.Instance.PreviewSfx(v);

    // 확인 버튼 → 현재 볼륨 저장 후 패널만 닫기
    public void OnClickConfirm()
    {
        AudioManager.Instance.SaveVolumes();
        Instance.settingPanel.SetActive(false);
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
        if (GameManager.Instance != null)
        {
            GameManager.Instance.MoveScene(SceneData.MainMenu);
        }
        // UI 전체가 아니라 '패널만' 닫음
        if (settingPanel) Instance.settingPanel.SetActive(false);
    }
    
    public void OnclickAlbum()
    {
        album.SetActive(true);
        albumBook.SetActive(true);
    }
}