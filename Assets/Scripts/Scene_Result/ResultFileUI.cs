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
        journalObj.SetActive(true);
       // 결과 이미지, 점수 세팅
        StartCoroutine(ResultFile()); // 결과 표시
    }
    private IEnumerator ResultFile()
    {
        isProcessing = true;

        yield return new WaitForSeconds(1f);

        stampObj.SetActive(true);

        isProcessing = false;
    }

    public void OnPointerClick(PointerEventData eventData) 
    {
        if (isProcessing)
            return; // 코루틴 도중이면 클릭 무시

        if (ResultManager.instance.CurrentStageInfo == 3) // 마지막 결과 이후 -> 엔딩 크래딧
        {
            gameObject.GetComponent<NextClick>().enabled = true;
            this.enabled = false;
            gameObject.GetComponent<NextClick>().OnPointerClick(eventData);
        }
        else // 다시 사무실로 다시 이동
        {
            GameManager.Instance.MoveScene(SceneData.Office);
        }
        /*
        count++;

        if(count == 1) // 첫번째 클릭 -> 다음 스테이지 암시 이미지
        {
            stampObj.SetActive(false);
            SFXPlayer.Instance.PlaySFX(nextPage);
            img.color = new Color(1, 1, 1, 0);
            img.sprite = files[ResultManager.instance.CurrentStageInfo - 1]; // 다음 스테이지 암시 표시
            img.DOFade(1f, 2f);
        }
        else if(count == 2) // 두번째 클릭 -> 사무실로 다시 이동
        {
            GameManager.Instance.MoveScene(SceneData.MainMenu);
        }
        else if(count == 2 && ResultManager.instance.CurrentStageInfo == 3) // 두번째 클릭 -> 엔딩 크래딧
        {
            gameObject.GetComponent<NextClick>().enabled = true;
            this.enabled = false;
            gameObject.GetComponent<NextClick>().OnPointerClick(eventData);
        }*/
    }
}
