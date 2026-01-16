using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ResultFileUI : MonoBehaviour, IPointerClickHandler
{
    public GameObject journalObj;
    public GameObject stampObj;

    public AudioClip stampClip; // 스템프 클립

    private bool isProcessing = false; // 코루틴 실행 여부
   
    void Start()
    {
        // 업무일지 상태 변환
        journalObj.SetActive(true);

        if (ResultManager.instance.CurrentStageInfo == 1
            && OfficeStateMachine.currentState != OfficeState.ReadyStage2
            && OfficeStateMachine.currentState != OfficeState.ReadyStage3)
        {
            // 결과 이미지, 점수 세팅
            StartCoroutine(ResultFile()); // 결과 표시

            OfficeStateMachine.SetState(OfficeState.Stage1Clear);
        }
        else if (ResultManager.instance.CurrentStageInfo == 2 
            && OfficeStateMachine.currentState != OfficeState.ReadyStage3)
        {
            // 결과 이미지, 점수 세팅
            StartCoroutine(ResultFile()); // 결과 표시

            if (!ResultManager.instance.IsFirstCredit)
                ResultManager.instance.IsFirstCredit = true;

            ResultManager.instance.SetNextStage();
            OfficeStateMachine.SetState(OfficeState.ReadyStage3);
        }

        // 결과쪽에서 항상 저장되게 -> 임시라서 나중에 구조적으로 중복 저장을 풀어야함
        DataManager.Instance.SaveGame();
    }
    private IEnumerator ResultFile()
    {
        isProcessing = true;

        yield return new WaitForSeconds(0.7f);

        GetComponentInChildren<JournalUI>().OnUnblur();

        yield return new WaitForSeconds(1f);

        stampObj.SetActive(true); // 완료 도장

        SFXPlayer.Instance.PlaySFX(stampClip); // 소리 재생

        isProcessing = false;
    }

    public void OnPointerClick(PointerEventData eventData) 
    {
        if (isProcessing)
            return; // 코루틴 도중이면 클릭 무시

        if (ResultManager.instance.CurrentStageInfo == 3 && ResultManager.instance.IsFirstCredit) // 마지막 결과 이후 -> 엔딩 크래딧
        {
            gameObject.GetComponent<NextClick>().enabled = true;
            this.enabled = false;
            gameObject.GetComponent<NextClick>().OnPointerClick(eventData);
        }
        else // 다시 사무실로 다시 이동
        {
            GameManager.Instance.MoveScene(SceneData.Office);
        }
    }
}
