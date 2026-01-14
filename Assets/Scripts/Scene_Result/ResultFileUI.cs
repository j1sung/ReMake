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

    public AudioClip nextPage; // 넘기는 소리

    private bool isProcessing = false; // 코루틴 실행 여부
   
    void Start()
    {
        // 업무일지 상태 변환
        journalObj.SetActive(true);
        
        // 결과 이미지, 점수 세팅
        StartCoroutine(ResultFile()); // 결과 표시
    }
    private IEnumerator ResultFile()
    {
        isProcessing = true;

        yield return new WaitForSeconds(1f);

        GetComponentInChildren<JournalUI>().OnUnblur();

        yield return new WaitForSeconds(1f);

        stampObj.SetActive(true); // 완료 도장

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
