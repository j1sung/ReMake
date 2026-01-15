using System.Collections;
using TMPro;
using UnityEngine;

public class SaveStateUI : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private int MaxObj = 16;

    private void OnEnable()
    {
        // 1) 스테이지 진행 정보는 현재 클리어한 스테이지가 어디까지인지를 기준으로 판별
        // 현재 클리어한 스테이지가 어디인지는 OfficeState -> stage1clear, stage2clear 를 기준으로 봄
        string stage;

        switch (OfficeStateMachine.currentState)
        {
            case OfficeState.Stage1Clear:
                stage = "2"; break;
            case OfficeState.ReadyStage2:
                stage = "2"; break;
            case OfficeState.Stage2Clear:
                stage = "3"; break;
            case OfficeState.ReadyStage3:
                stage = "3"; break;
            default:
                stage = "1"; break;
        }

        // 2) 앨범 진행도는 Max 오브제 갯수와 현재 앨범 오브제 갯수를 비교해서 만듦
        var result = ResultManager.instance;
        int currentObjCount = 0;

        for (int i=0; i<result.endingObjes.Count; i++)
        {
            currentObjCount += result.endingObjes[i].objs.Count;
        }
        float progress = (float)currentObjCount / MaxObj * 100f;
        progress = Mathf.Clamp(progress, 0f, 100f);

        // 진행 정보 표시
        text.text = $"진행: Stage {stage}\n앨범 진행도: {progress:0}%";       
    }
}
