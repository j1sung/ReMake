using UnityEngine;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider; // min=0, max=1, wholeNumbers=false

    private float startVolume;

    void OnEnable()
    {
        // 패널 열릴 때 현재값 표시 & 복구용 원본 저장
        startVolume = SettingManager.Instance.CurrentVolume;
        volumeSlider.SetValueWithoutNotify(startVolume);
    }

    // 슬라이더 OnValueChanged에 연결
    public void OnVolumeChanged(float v)
    {
        SettingManager.Instance.PreviewVolume(v); // 실시간 반영(미리듣기)
    }

    // 확인 버튼
    public void OnClickConfirm()
    {
        SettingManager.Instance.SaveVolume();     // 확정 저장
        gameObject.SetActive(false);
    }

    // 취소 버튼
    public void OnClickCancel()
    {
        SettingManager.Instance.ForceSetWithoutSave(startVolume); // 원상복구
        gameObject.SetActive(false);
    }

        public void OnClickBacktoTitle()
    {
        if (GameManager.instance != null)
        GameManager.instance.GoMain();   // = MainTitle 로드
    }
}