using TMPro;
using UnityEngine;

public class SaveStateUI : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private int totalStage = 3;

    private void OnEnable()
    {
        // 만약에 앨범 진행도가 도감작으로 바뀌면 나중에 못채운 오브제까지 포함해서 계산하게 변경하기
        int stage = ResultManager.instance.CurrentStageInfo;
        float progress = (float)(stage-1) / totalStage * 100f;
        progress = Mathf.Clamp(progress, 0f, 100f);

        text.text = $"진행: Stage {stage}\n앨범 진행도: {progress:0}%";
    }
}
